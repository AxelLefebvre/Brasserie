using System.Collections.Generic;

namespace ProjetBrasserie.Models
{
    public class Biere
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public decimal Degre { get; set; }
        public decimal Prix { get; set; }
        public int BrasserieId { get; set; }
        public Brasserie Brasserie { get; set; }
        public List<GrossisteStock> Stocks { get; set; } = new List<GrossisteStock>();
    }
}
