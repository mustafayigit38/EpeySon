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

    public virtual DbSet<Phone> Phones { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlite("Data Source=.\\data\\epeydb.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Phone>(entity =>
        {
            entity.Property(e => e.PhoneBatterySpecsBatteryCapacity).HasColumnName("PhoneBatterySpecs_BatteryCapacity");
            entity.Property(e => e.PhoneBatterySpecsChargingSpeed).HasColumnName("PhoneBatterySpecs_ChargingSpeed");
            entity.Property(e => e.PhoneBatterySpecsFastCharging).HasColumnName("PhoneBatterySpecs_FastCharging");
            entity.Property(e => e.PhoneCameraSpecsCameraFps).HasColumnName("PhoneCameraSpecs_CameraFps");
            entity.Property(e => e.PhoneCameraSpecsCameraResolution).HasColumnName("PhoneCameraSpecs_CameraResolution");
            entity.Property(e => e.PhoneCameraSpecsCameraZoom).HasColumnName("PhoneCameraSpecs_CameraZoom");
            entity.Property(e => e.PhoneScreenSpecsScreenFeature).HasColumnName("PhoneScreenSpecs_ScreenFeature");
            entity.Property(e => e.PhoneScreenSpecsScreenRefreshRate).HasColumnName("PhoneScreenSpecs_ScreenRefreshRate");
            entity.Property(e => e.PhoneScreenSpecsScreenResolution).HasColumnName("PhoneScreenSpecs_ScreenResolution");
            entity.Property(e => e.PhoneScreenSpecsScreenSize).HasColumnName("PhoneScreenSpecs_ScreenSize");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
