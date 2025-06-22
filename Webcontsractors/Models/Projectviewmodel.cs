namespace Webcontsractors.Models
{
    public class Projectviewmodel
    {
       public string Projectname { get; set; } = null!;

        public decimal? Amount { get; set; }

        public decimal? Amountvat { get; set; }
        public decimal? Opningbalance { get; set; }

        public string? Note { get; set; }

        public int? Prtid { get; set; }
    }
}
