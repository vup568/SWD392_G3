using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace OnlineShopWeb.Models;

public partial class Slider
{
    [Key]
    public int SliderId { get; set; }

    [StringLength(150)]
    public string? Title { get; set; }

    [StringLength(255)]
    public string? ImageUrl { get; set; }

    [StringLength(255)]
    public string? Link { get; set; }

    public bool? Status { get; set; }
}
