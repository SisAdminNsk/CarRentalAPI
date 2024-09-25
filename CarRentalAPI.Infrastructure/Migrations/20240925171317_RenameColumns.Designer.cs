﻿// <auto-generated />
using System;
using CarRentalAPI.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CarRentalAPI.Infrastructure.Migrations
{
    [DbContext(typeof(PostgresContext))]
    [Migration("20240925171317_RenameColumns")]
    partial class RenameColumns
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("CarRentalAPI.Core.Car", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<decimal>("BaseRentalPricePerHour")
                        .HasColumnType("numeric");

                    b.Property<string>("Brand")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("CarClass")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("CarImageURI")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Model")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Power")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Cars");
                });

            modelBuilder.Entity("CarRentalAPI.Core.CarOrder", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CarId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("CarsharingUserId")
                        .HasColumnType("uuid");

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<DateTime>("EndOfLease")
                        .HasColumnType("timestamp without time zone");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.Property<DateTime>("StartOfLease")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.HasIndex("CarId");

                    b.HasIndex("CarsharingUserId");

                    b.ToTable("CarOrders");
                });

            modelBuilder.Entity("CarRentalAPI.Core.CarsharingUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("Age")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("Patronymic")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("CarsharingUsers");
                });

            modelBuilder.Entity("CarRentalAPI.Core.ClosedCarOrder", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CarId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("CarOrderId")
                        .HasColumnType("uuid");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("Id");

                    b.HasIndex("CarId");

                    b.HasIndex("CarOrderId");

                    b.ToTable("ClosedCarOrders");
                });

            modelBuilder.Entity("CarRentalAPI.Core.OpenCarOrder", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CarId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("CarOrderId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("CarId")
                        .IsUnique();

                    b.HasIndex("CarOrderId");

                    b.ToTable("OpenCarOrders");
                });

            modelBuilder.Entity("CarRentalAPI.Core.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("UsersRoles");
                });

            modelBuilder.Entity("CarRentalAPI.Core.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("RoleUser", b =>
                {
                    b.Property<Guid>("RolesId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("UsersId")
                        .HasColumnType("uuid");

                    b.HasKey("RolesId", "UsersId");

                    b.HasIndex("UsersId");

                    b.ToTable("RoleUser");
                });

            modelBuilder.Entity("CarRentalAPI.Core.CarOrder", b =>
                {
                    b.HasOne("CarRentalAPI.Core.Car", "Car")
                        .WithMany("CarOrders")
                        .HasForeignKey("CarId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CarRentalAPI.Core.CarsharingUser", "CarsharingUser")
                        .WithMany("Orders")
                        .HasForeignKey("CarsharingUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Car");

                    b.Navigation("CarsharingUser");
                });

            modelBuilder.Entity("CarRentalAPI.Core.CarsharingUser", b =>
                {
                    b.HasOne("CarRentalAPI.Core.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("CarRentalAPI.Core.ClosedCarOrder", b =>
                {
                    b.HasOne("CarRentalAPI.Core.Car", null)
                        .WithMany("ClosedCarOrders")
                        .HasForeignKey("CarId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CarRentalAPI.Core.CarOrder", "CarOrder")
                        .WithMany()
                        .HasForeignKey("CarOrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CarOrder");
                });

            modelBuilder.Entity("CarRentalAPI.Core.OpenCarOrder", b =>
                {
                    b.HasOne("CarRentalAPI.Core.Car", "Car")
                        .WithOne("OpenCarOrder")
                        .HasForeignKey("CarRentalAPI.Core.OpenCarOrder", "CarId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CarRentalAPI.Core.CarOrder", "CarOrder")
                        .WithMany()
                        .HasForeignKey("CarOrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Car");

                    b.Navigation("CarOrder");
                });

            modelBuilder.Entity("RoleUser", b =>
                {
                    b.HasOne("CarRentalAPI.Core.Role", null)
                        .WithMany()
                        .HasForeignKey("RolesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CarRentalAPI.Core.User", null)
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CarRentalAPI.Core.Car", b =>
                {
                    b.Navigation("CarOrders");

                    b.Navigation("ClosedCarOrders");

                    b.Navigation("OpenCarOrder")
                        .IsRequired();
                });

            modelBuilder.Entity("CarRentalAPI.Core.CarsharingUser", b =>
                {
                    b.Navigation("Orders");
                });
#pragma warning restore 612, 618
        }
    }
}