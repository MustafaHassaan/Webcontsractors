using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webcontsractors.Domain.Models;

namespace Webcontsractors.Models
{
    public class Paymentview
    {
        public int Id { get; set; }
        public decimal? Creditor { get; set; }
        public decimal? Debitor { get; set; }
        public DateOnly? Date { get; set; }
        public string? Note { get; set; }
        public int? Proid { get; set; }
        public int? Prtid { get; set; }
        public virtual Project? Pro { get; set; }
        public virtual Partner? Prt { get; set; }
    }
}
