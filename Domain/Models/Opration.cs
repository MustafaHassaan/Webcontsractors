using System;
using System.Collections.Generic;

namespace Webcontsractors.Domain.Models;

public partial class Opration
{
    public int Id { get; set; }

    public string? Oprationname { get; set; }

    public string? Detailes { get; set; }

    public string? Tblname { get; set; }

    public DateOnly Date { get; set; }

    public string? Time { get; set; }

    public int Tblid { get; set; }

    public int Usrid { get; set; }

    public virtual User Usr { get; set; } = null!;
}
