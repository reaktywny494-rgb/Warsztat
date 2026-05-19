namespace Warsztat.Models
{
    public class Narzedzie
    {
        public int ID { get; set; }

        public string Nazwa { get; set; } = string.Empty;

        public string? Opis { get; set; }

        public int? Mebel_ID { get; set; }

        public Mebel? Mebel { get; set; }
    }
}
