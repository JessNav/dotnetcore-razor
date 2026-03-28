using System;
using System.Collections.Generic;

namespace Razor.Services.Models;

public partial class ProjectDto
{
    public int Id { get; set; }

    public int? CondoId { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public bool? IsApproved { get; set; }

    public int? FundingAmt { get; set; }

    public int? Phase { get; set; }

    public virtual CondoDto? Condo { get; set; }
}
