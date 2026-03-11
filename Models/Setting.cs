using System;
using System.Collections.Generic;

namespace Prj_Swd392.Models;

public partial class Setting
{
    public int SettingId { get; set; }

    public string? SettingKey { get; set; }

    public string? SettingValue { get; set; }

    public bool? Status { get; set; }
}
