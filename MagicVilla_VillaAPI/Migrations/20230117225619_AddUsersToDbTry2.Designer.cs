﻿// <auto-generated />
using System;
using MagicVilla_VillaAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MagicVillaVillaAPI.Migrations
{
    [DbContext(typeof(VillaDbContext))]
    [Migration("20230117225619_AddUsersToDbTry2")]
    partial class AddUsersToDbTry2
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("MagicVilla_VillaAPI.Models.LocalUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("LocalUsers");
                });

            modelBuilder.Entity("MagicVilla_VillaAPI.Models.Villa", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Amenity")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateUpdated")
                        .HasColumnType("datetime2");

                    b.Property<string>("Details")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Occupancy")
                        .HasColumnType("int");

                    b.Property<double>("Rate")
                        .HasColumnType("float");

                    b.Property<int>("Sqft")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Villas");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Amenity = "",
                            DateCreated = new DateTime(2023, 1, 17, 23, 56, 18, 921, DateTimeKind.Local).AddTicks(1161),
                            DateUpdated = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Details = "Fusce 11 tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                            ImageUrl = "https://dotnetmastery.com/bluevillaimages/villa3.jpg",
                            Name = "Royal Villa",
                            Occupancy = 4,
                            Rate = 200.0,
                            Sqft = 550
                        },
                        new
                        {
                            Id = 2,
                            Amenity = "",
                            DateCreated = new DateTime(2023, 1, 17, 23, 56, 18, 921, DateTimeKind.Local).AddTicks(1224),
                            DateUpdated = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Details = "Fusce 11 tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                            ImageUrl = "https://dotnetmastery.com/bluevillaimages/villa1.jpg",
                            Name = "Premium Pool Villa",
                            Occupancy = 4,
                            Rate = 300.0,
                            Sqft = 550
                        },
                        new
                        {
                            Id = 3,
                            Amenity = "",
                            DateCreated = new DateTime(2023, 1, 17, 23, 56, 18, 921, DateTimeKind.Local).AddTicks(1228),
                            DateUpdated = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Details = "Fusce 11 tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                            ImageUrl = "https://dotnetmastery.com/bluevillaimages/villa4.jpg",
                            Name = "Luxury Pool Villa",
                            Occupancy = 4,
                            Rate = 400.0,
                            Sqft = 750
                        },
                        new
                        {
                            Id = 4,
                            Amenity = "",
                            DateCreated = new DateTime(2023, 1, 17, 23, 56, 18, 921, DateTimeKind.Local).AddTicks(1230),
                            DateUpdated = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Details = "Fusce 11 tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                            ImageUrl = "https://dotnetmastery.com/bluevillaimages/villa5.jpg",
                            Name = "Diamond Villa",
                            Occupancy = 4,
                            Rate = 550.0,
                            Sqft = 900
                        },
                        new
                        {
                            Id = 5,
                            Amenity = "",
                            DateCreated = new DateTime(2023, 1, 17, 23, 56, 18, 921, DateTimeKind.Local).AddTicks(1233),
                            DateUpdated = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Details = "Fusce 11 tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                            ImageUrl = "https://dotnetmastery.com/bluevillaimages/villa2.jpg",
                            Name = "Diamond Pool Villa",
                            Occupancy = 4,
                            Rate = 600.0,
                            Sqft = 1100
                        });
                });

            modelBuilder.Entity("MagicVilla_VillaAPI.Models.VillaNumber", b =>
                {
                    b.Property<int>("VillaNo")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("SpecialDetails")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("VillaID")
                        .HasColumnType("int");

                    b.HasKey("VillaNo");

                    b.HasIndex("VillaID");

                    b.ToTable("VillaNumbers");
                });

            modelBuilder.Entity("MagicVilla_VillaAPI.Models.VillaNumber", b =>
                {
                    b.HasOne("MagicVilla_VillaAPI.Models.Villa", "Villa")
                        .WithMany()
                        .HasForeignKey("VillaID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Villa");
                });
#pragma warning restore 612, 618
        }
    }
}
