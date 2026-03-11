using System;
using System.Collections.Generic;

namespace Prj_Swd392.Models;

public partial class Slider
{
    public int SliderId { get; set; }

    public string? Title { get; set; }

    public string? ImageUrl { get; set; }

    public string? Link { get; set; }

    public bool? Status { get; set; }
}
