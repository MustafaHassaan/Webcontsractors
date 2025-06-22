using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Webcontsractors.Domain.Models;

public partial class TblTransaction
{
    public int Id { get; set; }

    public decimal? Creditor { get; set; }

    public decimal? Debitor { get; set; }

    public DateOnly? Tdate { get; set; }

    public string? Detailes { get; set; }
    public string? Note { get; set; }

    public decimal? Vatamount { get; set; }

    public int? Proid { get; set; }
    public int? Prtid { get; set; }
    public virtual Project? Pro { get; set; }
    public virtual Partner? Prt { get; set; }
}
