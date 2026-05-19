namespace Warsztat.Models
{
    public class Mebel
    {
        public int ID { get; set; }

        public string Nazwa { get; set; } = string.Empty;

        public string? Opis { get; set; }

        public List<Narzedzie> Narzedzia { get; set; } = new();
    }
}
