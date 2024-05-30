﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebApplication1.Contexts;

#nullable disable

namespace WebApplication1.Migrations
{
    [DbContext(typeof(EpeyDbContext))]
    [Migration("20240529225712_mig_3")]
    partial class mig_3
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.4");

            modelBuilder.Entity("WebApplication1.Models.Brand", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Brands");
                });

            modelBuilder.Entity("WebApplication1.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("WebApplication1.Models.Phone", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Brand")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("ImagePath")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<float>("Price")
                        .HasColumnType("REAL");

                    b.Property<string>("Seller")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Phones");
                });

            modelBuilder.Entity("WebApplication1.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("WebApplication1.Models.Phone", b =>
                {
                    b.OwnsOne("WebApplication1.Models.PhoneSpecs.PhoneBattery.PhoneBatterySpecs", "PhoneBatterySpecs", b1 =>
                        {
                            b1.Property<int>("PhoneId")
                                .HasColumnType("INTEGER");

                            b1.Property<int>("BatteryCapacity")
                                .HasColumnType("INTEGER");

                            b1.Property<int>("ChargingSpeed")
                                .HasColumnType("INTEGER");

                            b1.Property<bool>("FastCharging")
                                .HasColumnType("INTEGER");

                            b1.HasKey("PhoneId");

                            b1.ToTable("Phones");

                            b1.WithOwner()
                                .HasForeignKey("PhoneId");
                        });

                    b.OwnsOne("WebApplication1.Models.PhoneSpecs.PhoneCamera.PhoneCameraSpecs", "PhoneCameraSpecs", b1 =>
                        {
                            b1.Property<int>("PhoneId")
                                .HasColumnType("INTEGER");

                            b1.Property<int>("CameraFps")
                                .HasColumnType("INTEGER");

                            b1.Property<int>("CameraResolution")
                                .HasColumnType("INTEGER");

                            b1.Property<int>("CameraZoom")
                                .HasColumnType("INTEGER");

                            b1.HasKey("PhoneId");

                            b1.ToTable("Phones");

                            b1.WithOwner()
                                .HasForeignKey("PhoneId");
                        });

                    b.OwnsOne("WebApplication1.Models.PhoneSpecs.PhoneScreen.PhoneScreenSpecs", "PhoneScreenSpecs", b1 =>
                        {
                            b1.Property<int>("PhoneId")
                                .HasColumnType("INTEGER");

                            b1.Property<int>("ScreenFeature")
                                .HasColumnType("INTEGER");

                            b1.Property<int>("ScreenRefreshRate")
                                .HasColumnType("INTEGER");

                            b1.Property<int>("ScreenResolution")
                                .HasColumnType("INTEGER");

                            b1.Property<float>("ScreenSize")
                                .HasColumnType("REAL");

                            b1.HasKey("PhoneId");

                            b1.ToTable("Phones");

                            b1.WithOwner()
                                .HasForeignKey("PhoneId");
                        });

                    b.Navigation("PhoneBatterySpecs")
                        .IsRequired();

                    b.Navigation("PhoneCameraSpecs")
                        .IsRequired();

                    b.Navigation("PhoneScreenSpecs")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
