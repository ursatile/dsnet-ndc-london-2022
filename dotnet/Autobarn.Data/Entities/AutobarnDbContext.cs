using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Autobarn.Data.Entities
{
    public partial class AutobarnDbContext : DbContext
    {
        public AutobarnDbContext()
        {
        }

        public AutobarnDbContext(DbContextOptions<AutobarnDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Manufacturer> Manufacturers { get; set; }
        public virtual DbSet<Model> Models { get; set; }
        public virtual DbSet<Vehicle> Vehicles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=localhost;Database=Autobarn;User=autobarn;Password=autobarn");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Manufacturer>(entity =>
            {
                entity.HasKey(e => e.Code)
                    .HasName("PK__Manufact__A25C5AA6788AD2B2");

                entity.Property(e => e.Code)
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(32)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Model>(entity =>
            {
                entity.HasKey(e => e.Code)
                    .HasName("PK__Models__A25C5AA61CD58896");

                entity.Property(e => e.Code)
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.ManufacturerCode)
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.HasOne(d => d.Manufacturer)
                    .WithMany(p => p.Models)
                    .HasForeignKey(d => d.ManufacturerCode)
                    .HasConstraintName("FK_Models_Manufacturers");
            });

            modelBuilder.Entity<Vehicle>(entity =>
            {
                entity.HasKey(e => e.Registration)
                    .HasName("PK__Vehicles__EFBECB9F660B8F07");

                entity.Property(e => e.Registration)
                    .HasMaxLength(16)
                    .IsUnicode(false);

                entity.Property(e => e.Color)
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.ModelCode)
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.HasOne(d => d.VehicleModel)
                    .WithMany(p => p.Vehicles)
                    .HasForeignKey(d => d.ModelCode)
                    .HasConstraintName("FK_Vehicles_Models");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
