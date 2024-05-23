using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models;

public partial class EpeydbContext : DbContext
{
    public EpeydbContext()
    {
    }

    public EpeydbContext(DbContextOptions<EpeydbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Kategori> Kategoriler { get; set; }

    public virtual DbSet<KategoriOzellik> KategoriOzellikler { get; set; }

    public virtual DbSet<KategoriÖzellikÜrünDeğer> KategoriÖzellikÜrünDeğerler { get; set; }

    public virtual DbSet<Kullanıcı> Kullanıcılar { get; set; }

    public virtual DbSet<Marka> Markalar { get; set; }

    public virtual DbSet<Ürün> Ürünler { get; set; }
    

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlite("Data Source = .\\Data\\epeydb.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Kategori>(entity =>
        {
            entity.ToTable("Kategori");
        });

        modelBuilder.Entity<KategoriOzellik>(entity =>
        {
            entity.ToTable("KategoriOzellik");

            entity.HasOne(d => d.Kategori).WithMany(p => p.KategoriOzelliks)
                .HasForeignKey(d => d.KategoriId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<KategoriÖzellikÜrünDeğer>(entity =>
        {
            entity.ToTable("KategoriÖzellikÜrünDeğer");

            entity.HasOne(d => d.KategoriÖzellik).WithMany(p => p.KategoriÖzellikÜrünDeğers)
                .HasForeignKey(d => d.KategoriÖzellikId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Ürün).WithMany(p => p.KategoriÖzellikÜrünDeğers)
                .HasForeignKey(d => d.ÜrünId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Kullanıcı>(entity =>
        {
            entity.ToTable("Kullanıcı");

            entity.HasIndex(e => e.EPosta, "IX_Kullanıcı_E-posta").IsUnique();

            entity.Property(e => e.EPosta).HasColumnName("E-posta");
        });

        modelBuilder.Entity<Marka>(entity =>
        {
            entity.ToTable("Marka");
        });

        modelBuilder.Entity<Ürün>(entity =>
        {
            entity.ToTable("Ürün");

            entity.HasOne(d => d.Kategori).WithMany(p => p.Ürüns)
                .HasForeignKey(d => d.KategoriId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Marka).WithMany(p => p.Ürüns)
                .HasForeignKey(d => d.MarkaId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
