

using System.ComponentModel.DataAnnotations.Schema;

namespace Webcontsractors.Models
{
    public class Transactionmodel
    {
        public int Id { get; set; }

        public decimal? Creditor { get; set; }

        public decimal? Debitor { get; set; }

        public string? Tdate { get; set; }

        public string? Detailes { get; set; }
        public string? Projectname { get; set; }
        public string? Prtname { get; set; }
        public string? Note { get; set; }

        public decimal Vatamount { get; set; }
        [NotMapped]
        public decimal? Balance { get; set; }

        public int? Proid { get; set; }
        public int? Prtid { get; set; }
    }
}
