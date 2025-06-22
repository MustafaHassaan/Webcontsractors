using Webcontsractors.Domain.Models;

namespace Webcontsractors.Models
{
    public class PARepviewmodel
    {
        public int Proid { get; set; } // معرف المشروع
        public string ProjectName { get; set; } // اسم المشروع
        public string PartnerName { get; set; } // اسم الشريك
        public decimal OpeningBalance { get; set; } // الرصيد الافتتاحي
        public List<TransactionViewModel> Transactions { get; set; } = new List<TransactionViewModel>(); // قائمة الحركات
        public DateOnly LastTransactionDate { get; internal set; }
    }
}
