using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using razorPagesEgitim.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace razorPagesEgitim.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<bakimTipi> bakimTipi { get; set; }
        public DbSet<ApplicationUser> applicationUser { get; set; }
        public DbSet<Makina> Makina { get; set; }

        public DbSet<BakimHizmetKart> BakimHizmetKart { get; set; }
        public DbSet<BakimHizmetiGenel> BakimHizmetiGenel { get; set; }
        public DbSet<BakimHizmetiDetay> BakimHizmetiDetay { get; set; }

    }
}
