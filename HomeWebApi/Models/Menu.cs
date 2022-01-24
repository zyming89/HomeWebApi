using System;
using System.Collections.Generic;

namespace HomeWebApi.Models
{
    public partial class Menu
    {
        public double? Id { get; set; }
        public string? Title { get; set; }
        public string? Icon { get; set; }
        public int? IsOpen { get; set; }
        public string? SubMenus { get; set; }
    }
}
