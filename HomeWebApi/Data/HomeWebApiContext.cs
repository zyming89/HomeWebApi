#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HomeWebApi.Models;

namespace HomeWebApi.Data
{
    public class HomeWebApiContext : DbContext
    {
        public HomeWebApiContext (DbContextOptions<HomeWebApiContext> options)
            : base(options)
        {
        }

        public DbSet<User> User { get; set; }

        public DbSet<Menu> Menu { get; set; }

        public DbSet<HomeWebApi.Models.SubMenu> SubMenu { get; set; }

    }
}
