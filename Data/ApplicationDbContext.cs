using System;
using System.Collections.Generic;
using System.Text;
using control.personal.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace control.personal.Data
{
    public class ApplicationDbContext : IdentityDbContext<Usuario>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public virtual DbSet<Registro> Registro { get; set; }
        public virtual DbSet<ControlIngreso> ControlIngreso { get; set; }
        public virtual DbSet<Identificacion> Identificacion { get; set; }

        /*protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Usuario>().HasIndex(c => c.Cedula).IsUnique();
            
            base.OnModelCreating(builder);
        }*/
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
