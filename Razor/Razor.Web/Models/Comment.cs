using System;
using System.Collections.Generic;

namespace Razor.Web.Models;

public partial class Comment
{
    public int Id { get; set; }

    public int? CondoId { get; set; }

    public string Text { get; set; } = null!;

    public string Username { get; set; } = null!;

    public virtual Condo? Condo { get; set; }
}
