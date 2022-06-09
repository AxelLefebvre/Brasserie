using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjetBrasserie.Models;
using ProjetBrasserie.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjetBrasserie.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BrasserieController : ControllerBase
    {
        private readonly BrasserieDbContext _context;
        private readonly BrasserieRepository<Brasserie> _breweryRepo;
        private readonly BrasserieRepository<Biere> _beerRepo;
        private readonly BrasserieRepository<Grossiste> _wholesalerRepo;
        private readonly BrasserieRepository<GrossisteStock> _stockRepo;

        public BrasserieController(BrasserieDbContext context)
        {
            _context = context;
            _breweryRepo = new BrasserieRepository<Brasserie>(_context);
            _beerRepo = new BrasserieRepository<Biere>(_context);
            _wholesalerRepo = new BrasserieRepository<Grossiste>(_context);
            _stockRepo = new BrasserieRepository<GrossisteStock>(_context);
        }

        [HttpGet("brewery={id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Biere))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult<IEnumerable<Biere>> GetAllBreweryBeers(int id)
        {
            _ = _beerRepo.GetAll();
            var brewery = _breweryRepo.Get(id);
            if (brewery == null) return NoContent();
            return brewery.Bieres == null ? NoContent() : Ok(brewery.Bieres);
        }

        [HttpGet("wholesaler={id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Grossiste))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult<IEnumerable<Grossiste>> GetAllBeerWholesalers(int id)
        {
            var stocks = _stockRepo.GetAll();
            var wholesalers = _wholesalerRepo.GetAll();
            return Ok(wholesalers.Where(wh => wh.Stocks.Any(st => st.BiereId == id)));
        }

        [HttpPost("addbeertobrewery")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult AddBeerToBrewery(string name, decimal degrees, decimal price, int breweryId)
        {
            var beer = new Biere
            {
                Nom = name,
                Degre = degrees,
                Prix = price,
                BrasserieId = breweryId
            };
            try
            {
                if (_beerRepo.Add(beer))
                    return Created("The new beer has been added into DB.", beer);
                return BadRequest("The new beer couldn't be added into DB.");
            }
            catch (Exception)
            {
                return BadRequest("The new beer couldn't be added into DB.");
            }
        }

        [HttpPost("addbeertowholesaler")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult AddBeerToWholesaler(int wholesalerId, int beerId, int amount)
        {
            var stock = new GrossisteStock
            {
                GrossisteId = wholesalerId,
                BiereId = beerId,
                Quantite = amount
            };
            try
            {
                if (_stockRepo.Add(stock))
                    return Created("The new beer stock has been added into DB.", stock);
                return BadRequest("The new beer stock couldn't be added into DB.");
            }
            catch (Exception)
            {
                return BadRequest("The new beer stock couldn't be added into DB.");
            }
        }

        [HttpPut("updatestock")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GrossisteStock))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult UpdateStock(int beerId, int wholesalerId, int amount)
        {
            var allStocks = _stockRepo.GetAll();
            if (!allStocks.Any(st => st.BiereId == beerId && st.GrossisteId == wholesalerId))
                return BadRequest("Stock hasn't been updated, please check out the beer and the wholesaler.");
            var stock = allStocks.Single(st => st.BiereId == beerId && st.GrossisteId == wholesalerId);
            stock.Quantite = amount;
            _stockRepo.Update(stock);
            return Ok(stock);
        }

        [HttpDelete("deletebeerfrombrewery")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Biere))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult DeleteBeerFromBrewery(int id)
        {
            if (!_beerRepo.GetAll().Any(b => b.Id == id))
                return BadRequest("Beer hasn't been deleted, please change your id.");
            _beerRepo.Remove(id);
            return Ok();
        }

        /// <summary>
        /// Return a summary and a price for an order of several beers.
        /// </summary>
        /// <param name="fullOrder">Key : beer id, Value : quantity of beer in the order</param>
        /// <returns></returns>
        [HttpGet("beerdelivery={wholesalerId}/")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<string> BeerDelivery([FromQuery] Dictionary<int, int> fullOrder, int wholesalerId)
        {
            if (fullOrder.Count == 0)
                return BadRequest("Please complete the order");
            var wholesaler = _wholesalerRepo.Get(wholesalerId);
            if (wholesaler == null)
                return BadRequest("Please change the wholesaler id");

            _ = _beerRepo.GetAll();
            _ = _stockRepo.GetAll();
            foreach (var beerOrder in fullOrder)
            {
                if (!wholesaler.Stocks.Any(st => st.BiereId == beerOrder.Key))
                    return BadRequest($"Wholesaler {wholesaler.Nom} doesn't have beer {beerOrder.Key} in stock.");
                var stock = wholesaler.Stocks.SingleOrDefault(st => st.BiereId == beerOrder.Key);
                if (stock.Quantite < beerOrder.Value)
                    return BadRequest($"Wholesaler {wholesaler.Nom} doesn't have enough beer {beerOrder.Key} in stock {stock.Quantite}.");
            }

            var totalOfBeers = 0;
            var price = 0.0m;
            var res = string.Empty;
            foreach (var beerOrder in fullOrder)
            {
                var stock = wholesaler.Stocks.SingleOrDefault(st => st.BiereId == beerOrder.Key);
                totalOfBeers += beerOrder.Value;
                price += beerOrder.Value * stock.Biere.Prix;
                res += $"{beerOrder.Value} {stock.Biere.Nom} - {beerOrder.Value * stock.Biere.Prix}€\n";
            }

            if (totalOfBeers >= 20)
            { 
                price *= 0.8m;
                res += $"20% reduction\n";
            }
            else if (totalOfBeers >= 10) 
            { 
                price *= 0.9m;
                res += $"10% reduction\n";
            }
            res += $"TOTAL TO PAY: {price:0.00}€";
            return Ok(res);
        }
    }
}
