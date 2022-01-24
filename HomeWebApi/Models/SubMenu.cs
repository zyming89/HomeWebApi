using System;
using System.Collections.Generic;

namespace HomeWebApi.Models
{
    public partial class SubMenu
    {
        public double? Id { get; set; }
        public double? Pid { get; set; }
        public string? Title { get; set; }
        public string? Url { get; set; }
    }
}
