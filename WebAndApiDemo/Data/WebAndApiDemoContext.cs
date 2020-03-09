using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebAndApiDemo.Models;

namespace WebAndApiDemo.Data
{
    public class WebAndApiDemoContext : DbContext
    {
        public WebAndApiDemoContext (DbContextOptions<WebAndApiDemoContext> options)
            : base(options)
        {
        }

        public DbSet<News> News { get; set; }

        public DbSet<NewsCategory> NewsCategory { get; set; }

        public DbSet<NewsCategoryAssignment> NewsCategoryAssignment { get; set; }
    }
}
