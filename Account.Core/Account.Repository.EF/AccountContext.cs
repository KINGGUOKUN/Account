using Account.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Account.Repository.EF
{
    public class AccountContext : DbContext
    {
        public AccountContext(DbContextOptions<AccountContext> options)
            :base(options)
        {

        }

        public DbSet<Manifest> Manifests { get; set; }
        public DbSet<Daily> Dailys { get; set; }
        
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Manifest>().ToTable("Manifest");
        //    modelBuilder.Entity<Daily>().ToTable("Daily");
        //}
    }
}
