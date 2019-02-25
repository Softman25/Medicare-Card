using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ThisisaTest.Database
{
    public class SqliteDbContext : DbContext
    {
        public DbSet<Invoice> Invoice { get; set; }    
    
        protected override void OnConfiguring(DbContextOptionsBuilder Options)
        {
            //string DbLocation = Assembly.GetEntryAssembly().Location.Replace(@"\usr\Discord\PUBLISH", @"\Database");
            Options.UseSqlite($"Data Source=/usr/Discord/PUBLISH/Database/Database.sqlite");
        }
    }
}
