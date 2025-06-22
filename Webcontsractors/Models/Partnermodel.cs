namespace Webcontsractors.Models
{
    public class Partnermodel
    {
        public int Id { get; set; }

        public string Partnername { get; set; } = null!;

        public string? Description { get; set; }
        public decimal? Amount { get; set; }
        public int Percentage { get; set; }
    }
}
