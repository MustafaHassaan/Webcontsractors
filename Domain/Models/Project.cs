using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Webcontsractors.Domain.Models;

public partial class Project
{
    public int Id { get; set; }

    public string Projectname { get; set; } = null!;

    public decimal? Amount { get; set; }

    public decimal? Amountvat { get; set; }
    public decimal? Opningbalance { get; set; }

    public DateOnly? Tdate { get; set; }
    public string? Note { get; set; }
    public string? Status { get; set; }

    public int? Prtid { get; set; }

    public virtual Partner? Prt { get; set; }

    [NotMapped]
    public virtual TblTransaction? Trn { get; set; }
}
