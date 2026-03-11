using System;
using System.Collections.Generic;

namespace Prj_Swd392.Models;

public partial class Feedback
{
    public int FeedbackId { get; set; }

    public int? UserId { get; set; }

    public int? ProductId { get; set; }

    public string? Content { get; set; }

    public int? Rating { get; set; }

    public bool? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Product? Product { get; set; }

    public virtual User? User { get; set; }
}
