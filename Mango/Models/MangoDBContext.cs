using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Mango.Models
{
    public partial class MangoDBContext : DbContext
    {
        public MangoDBContext()
        {
        }

        public MangoDBContext(DbContextOptions<MangoDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<MangoCount> MangoCount { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            /*
            Developement - Environment -- Abhilash

            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server = DESKTOP-UD9NFAQ\\SQLEXPRESS; Database = MangoDB ; Trusted_Connection = True");
            }
            */

            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server =db;Database=master;User=sa;Password=Your_password123;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {}
    }
}
