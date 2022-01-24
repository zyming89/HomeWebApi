using System;
using System.Collections.Generic;

namespace HomeWebApi.Models
{
    public partial class User
    {
        public string? Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
    }
}
