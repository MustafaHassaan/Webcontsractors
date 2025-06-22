using System;
using System.Collections.Generic;

namespace Webcontsractors.Domain.Models;

public partial class Partner
{
    public int Id { get; set; }

    public string Partnername { get; set; } = null!;

    public string? Description { get; set; }
    public decimal? Amount { get; set; }
    public decimal? Percentage { get; set; }


    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();
}
