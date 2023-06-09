﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Spinfluence.Data;

#nullable disable

namespace Spinfluence.Migrations
{
    [DbContext(typeof(SpinContext))]
    [Migration("20230512085112_ApprovedPracticeMigration")]
    partial class ApprovedPracticeMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Spinfluence.Data.Account", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("address")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("admin")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("logo")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("password")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("varchar(64)");

                    b.Property<string>("token")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("username")
                        .IsRequired()
                        .HasMaxLength(80)
                        .HasColumnType("varchar(80)");

                    b.HasKey("Id");

                    b.ToTable("Account", (string)null);
                });

            modelBuilder.Entity("Spinfluence.Data.Company", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("LogoImage")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("PosterImage")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Company", (string)null);
                });

            modelBuilder.Entity("Spinfluence.Data.CompanyEvent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("BeginDate")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("CompanyId")
                        .HasColumnType("int");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("Seats")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.ToTable("CompanyEvent", (string)null);
                });

            modelBuilder.Entity("Spinfluence.Data.Practice", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.Property<string>("Body")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("CompanyEventId")
                        .HasColumnType("int");

                    b.Property<string>("CoverLetter")
                        .HasColumnType("longtext");

                    b.Property<bool?>("IsApproved")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsCanceled")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Resume")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("CompanyEventId");

                    b.ToTable("Practice", (string)null);
                });

            modelBuilder.Entity("Spinfluence.Data.CompanyEvent", b =>
                {
                    b.HasOne("Spinfluence.Data.Company", "Company")
                        .WithMany("CompanyEvents")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Company");
                });

            modelBuilder.Entity("Spinfluence.Data.Practice", b =>
                {
                    b.HasOne("Spinfluence.Data.Account", "Account")
                        .WithMany("Practices")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Spinfluence.Data.CompanyEvent", "CompanyEvent")
                        .WithMany("Practices")
                        .HasForeignKey("CompanyEventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");

                    b.Navigation("CompanyEvent");
                });

            modelBuilder.Entity("Spinfluence.Data.Account", b =>
                {
                    b.Navigation("Practices");
                });

            modelBuilder.Entity("Spinfluence.Data.Company", b =>
                {
                    b.Navigation("CompanyEvents");
                });

            modelBuilder.Entity("Spinfluence.Data.CompanyEvent", b =>
                {
                    b.Navigation("Practices");
                });
#pragma warning restore 612, 618
        }
    }
}
