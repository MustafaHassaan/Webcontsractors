namespace Webcontsractors.Models
{
    public class TransactionViewModel
    {
        public string Projectname { get; set; }
        public string Partnername { get; set; }
        public decimal? Creditor { get; set; }
        public decimal? Debitor { get; set; }
        public decimal Vatamount { get; set; }
        public string Tdate { get; set; }
        public string Detailes { get; set; }
        public string? Note { get; set; }
        public decimal? Balance { get; set; }
        public decimal? Remaining { get; set; }
    }
}
