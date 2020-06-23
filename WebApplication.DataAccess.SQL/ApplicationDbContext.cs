using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using Microsoft.EntityFrameworkCore;
using WebApplication.DataAccess.SQL.DataModels;

namespace WebApplication.DataAccess.SQL
{
    public class ApplicationDbContext : DbContext
    {
        public readonly string _ConnectionString;
        public ApplicationDbContext( DbContextOptions<ApplicationDbContext> Options): base(Options) { }
        public ApplicationDbContext(string ConnectionString ) {
            _ConnectionString = ConnectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            
                optionsBuilder.UseSqlServer(_ConnectionString);
            }
        }

        public DbSet<Utente> Utente { get; set; }
        public DbSet<Messaggio> Messaggio { get; set; }
        public DbSet<UtenteLikeMessaggio> UtenteLikeMessaggio { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<Utente>().ToTable("Utente");
            modelBuilder.Entity<Messaggio>().ToTable("Messaggio");
            modelBuilder.Entity<UtenteLikeMessaggio>().HasKey(c => new { c.Email, c.IDMessaggio });
         
        }

    }
}
