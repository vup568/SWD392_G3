using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace OnlineShopWeb.Models;

public partial class Setting
{
    [Key]
    public int SettingId { get; set; }

    [StringLength(100)]
    public string? SettingKey { get; set; }

    [StringLength(255)]
    public string? SettingValue { get; set; }

    public bool? Status { get; set; }
}
