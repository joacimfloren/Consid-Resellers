using ConsidResellers.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsidResellers.Data_Layer
{
    public class ConsidContext : DbContext
    {
        public ConsidContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("newid()");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.OrganizationNumber)
                    .IsRequired();
            });

            modelBuilder.Entity<Store>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("newid()");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(512);

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(512);

                entity.Property(e => e.Zip)
                    .IsRequired()
                    .HasMaxLength(16);

                entity.Property(e => e.Country)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Latitude).HasMaxLength(15);

                entity.Property(e => e.Longitude).HasMaxLength(15);   

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.Stores)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Companies_Stores");
            });
        }

        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<Store> Stores { get; set; }
    }
}
