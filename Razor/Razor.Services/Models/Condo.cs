using System;
using System.Collections.Generic;

namespace Razor.Services.Models;

public partial class CondoDto
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string City { get; set; } = null!;

    public string State { get; set; } = null!;

    public int? Zip { get; set; }

    public int? NoOfUnits { get; set; }

    public int? YearBuilt { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public virtual ICollection<CommentDto> Comments { get; set; } = new List<CommentDto>();

    public virtual ICollection<ProjectDto> Projects { get; set; } = new List<ProjectDto>();
}
