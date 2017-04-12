using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using ConsidResellers.Data_Layer;

namespace ConsidResellers.Migrations
{
    [DbContext(typeof(ConsidContext))]
    [Migration("20170224212739_migration")]
    partial class migration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ConsidResellers.Models.Company", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("newid()");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 255);

                    b.Property<string>("Notes");

                    b.Property<int>("OrganizationNumber");

                    b.HasKey("Id");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("ConsidResellers.Models.Store", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("newid()");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 512);

                    b.Property<string>("City")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 512);

                    b.Property<Guid>("CompanyId");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 50);

                    b.Property<string>("Latitude")
                        .HasAnnotation("MaxLength", 15);

                    b.Property<string>("Longitude")
                        .HasAnnotation("MaxLength", 15);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 100);

                    b.Property<string>("Zip")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 16);

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.ToTable("Stores");
                });

            modelBuilder.Entity("ConsidResellers.Models.Store", b =>
                {
                    b.HasOne("ConsidResellers.Models.Company", "Company")
                        .WithMany("Stores")
                        .HasForeignKey("CompanyId")
                        .HasConstraintName("FK_Companies_Stores");
                });
        }
    }
}
