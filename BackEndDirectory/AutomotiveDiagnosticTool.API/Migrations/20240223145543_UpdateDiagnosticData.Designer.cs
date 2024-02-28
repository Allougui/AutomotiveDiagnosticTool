﻿// <auto-generated />
using System;
using AutomotiveDiagnosticTool.API;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AutomotiveDiagnosticTool.API.Migrations
{
    [DbContext(typeof(AppDBContext))]
    [Migration("20240223145543_UpdateDiagnosticData")]
    partial class UpdateDiagnosticData
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.0");

            modelBuilder.Entity("AutomotiveDiagnosticTool.API.DiagnosticData", b =>
                {
                    b.Property<int>("DiagnosticDataId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("BatteryStatus")
                        .HasColumnType("TEXT");

                    b.Property<string>("Details")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("DiagnosticType")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("EngineHealth")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("TEXT");

                    b.Property<string>("TirePressure")
                        .HasColumnType("TEXT");

                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("DiagnosticDataId");

                    b.HasIndex("UserId");

                    b.ToTable("DiagnosticData", (string)null);
                });

            modelBuilder.Entity("AutomotiveDiagnosticTool.API.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("UserType")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("UserId");

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("AutomotiveDiagnosticTool.API.DiagnosticData", b =>
                {
                    b.HasOne("AutomotiveDiagnosticTool.API.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });
#pragma warning restore 612, 618
        }
    }
}