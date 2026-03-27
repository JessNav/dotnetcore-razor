using System;
using System.Collections.Generic;

namespace Razor.Web.Models;

public partial class Project
{
    public int Id { get; set; }

    public int? CondoId { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public bool? IsApproved { get; set; }

    public int? FundingAmt { get; set; }

    public int? Phase { get; set; }

    public virtual Condo? Condo { get; set; }
}
