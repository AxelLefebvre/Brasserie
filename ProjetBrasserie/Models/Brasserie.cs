using System.Collections.Generic;

namespace ProjetBrasserie.Models
{
    public class Brasserie
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public List<Biere> Bieres { get; set; } = new List<Biere>();
    }
}
