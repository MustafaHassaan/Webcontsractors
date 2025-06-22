using System.ComponentModel.DataAnnotations.Schema;
using System.Transactions;
using Webcontsractors.Domain.Models;

namespace Webcontsractors.Models
{
    public class Projectmodel
    {
        public int Id { get; set; }

        [NotMapped]
        public int? ProId { get; set; }

        public string? Projectname { get; set; } = null!;

        public decimal? Amount { get; set; }

        public decimal? Amountvat { get; set; }
        public decimal? Opningbalance { get; set; }

        [NotMapped]
        public decimal? Balance { get; set; }

        [NotMapped]
        public double Profits { get; set; }

        public string? Partnername { get; set; }
        public string? Note { get; set; }
        public string? Status { get; set; }
        public DateOnly? Tdate { get; set; }
        public int? Prtid { get; set; }

        public virtual Partner? Prt { get; set; }

        [NotMapped]
        public virtual ICollection<TblTransaction> Transactions { get; set; } = new List<TblTransaction>();
    }
}
