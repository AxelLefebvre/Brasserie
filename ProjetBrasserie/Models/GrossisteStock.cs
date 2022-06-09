namespace ProjetBrasserie.Models
{
    public class GrossisteStock
    {
        public int GrossisteId { get; set; }
        public Grossiste Grossiste { get; set; }
        public int BiereId { get; set; }
        public Biere Biere { get; set; }
        public int Quantite { get; set; }
    }
}
