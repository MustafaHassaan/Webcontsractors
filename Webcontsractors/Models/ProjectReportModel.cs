namespace Webcontsractors.Models
{
    public class ProjectReportModel
    {
        //public int? ProjectId { get; set; }
        //public int? PartnerId { get; set; }
        public string? ProjectName { get; set; }
        public string? LastTransactionDate { get; set; }
        public double? CreditorSum { get; set; }
        public double? DebitorSum { get; set; }
        public double? Balance { get; set; }
        public string? PartnerName { get; set; }
        public string? Note { get; set; }

    }

}
