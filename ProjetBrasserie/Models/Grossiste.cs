using System.Collections.Generic;

namespace ProjetBrasserie.Models
{
    public class Grossiste
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public List<GrossisteStock> Stocks { get; set; } = new List<GrossisteStock>();
    }
}
