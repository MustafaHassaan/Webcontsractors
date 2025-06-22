using Webcontsractors.Domain.Models;

namespace Webcontsractors.Models
{
    public class PTRepviewmodel
    {
        public Project Project { get; set; }
        public List<TblTransaction> Transactions { get; set; }
        public decimal? OpeningBalance { get; set; }  // الرصيد
        public decimal TotalDebitor { get; set; }  // مجموع المدين
        public decimal TotalVatamount { get; set; } // مجموع الضريبة
    }
}
