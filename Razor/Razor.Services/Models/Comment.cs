using System;
using System.Collections.Generic;

namespace Razor.Services.Models;

public partial class CommentDto
{
    public int Id { get; set; }

    public int? CondoId { get; set; }

    public string Text { get; set; } = null!;

    public string Username { get; set; } = null!;

    public virtual CondoDto? Condo { get; set; }
}
