using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models;

public partial class EpeyContext : DbContext
{
    public EpeyContext()
    {
    }

    public EpeyContext(DbContextOptions<EpeyContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Brand> Brands { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Phone> Phones { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlite("Data Source=.\\data\\epeydb.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Comment>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");

            entity.HasOne(d => d.Product).WithMany(p => p.Comments)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Phone>(entity =>
        {
            entity.Property(e => e.PhoneBatterySpecsBatteryCapacity).HasColumnName("PhoneBatterySpecs_BatteryCapacity");
            entity.Property(e => e.PhoneBatterySpecsChargingSpeed).HasColumnName("PhoneBatterySpecs_ChargingSpeed");
            entity.Property(e => e.PhoneBatterySpecsFastCharging).HasColumnName("PhoneBatterySpecs_FastCharging");
            entity.Property(e => e.PhoneCameraSpecsCameraFps).HasColumnName("PhoneCameraSpecs_CameraFps");
            entity.Property(e => e.PhoneCameraSpecsCameraResolution).HasColumnName("PhoneCameraSpecs_CameraResolution");
            entity.Property(e => e.PhoneCameraSpecsCameraZoom).HasColumnName("PhoneCameraSpecs_CameraZoom");
            entity.Property(e => e.PhoneScreenSpecsScreenRefreshRate).HasColumnName("PhoneScreenSpecs_ScreenRefreshRate");
            entity.Property(e => e.PhoneScreenSpecsScreenResolution).HasColumnName("PhoneScreenSpecs_ScreenResolution");
            entity.Property(e => e.PhoneScreenSpecsScreenSize).HasColumnName("PhoneScreenSpecs_ScreenSize");

            entity.HasOne(d => d.Brand).WithMany(p => p.Phones)
                .HasForeignKey(d => d.BrandId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Category).WithMany(p => p.Phones)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
