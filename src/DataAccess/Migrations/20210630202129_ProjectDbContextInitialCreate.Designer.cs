﻿// <auto-generated />
using System;
using DataAccess.Concrete.EntityFramework.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DataAccess.Migrations
{
    [DbContext(typeof(ProjectDbContext))]
    [Migration("20210630202129_ProjectDbContextInitialCreate")]
    partial class ProjectDbContextInitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.7")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Entities.Concrete.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CategoryName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            Id = new Guid("8d6af2ba-6234-4b2a-9448-1e299e2c4859"),
                            CategoryName = "Phone",
                            CreatedBy = "Admin",
                            CreatedDate = new DateTime(2021, 6, 30, 23, 21, 28, 357, DateTimeKind.Local).AddTicks(2472),
                            Name = "Iphone 4",
                            Price = 400m
                        },
                        new
                        {
                            Id = new Guid("82d9cb46-71c6-4c3f-982b-71983a0c740b"),
                            CategoryName = "Phone",
                            CreatedBy = "Admin",
                            CreatedDate = new DateTime(2021, 6, 30, 23, 21, 28, 358, DateTimeKind.Local).AddTicks(4962),
                            Name = "Iphone 5",
                            Price = 400m
                        },
                        new
                        {
                            Id = new Guid("34376e2c-9844-4d1d-9bbe-95d01353de9f"),
                            CategoryName = "Phone",
                            CreatedBy = "Admin",
                            CreatedDate = new DateTime(2021, 6, 30, 23, 21, 28, 358, DateTimeKind.Local).AddTicks(4987),
                            Name = "Iphone 6",
                            Price = 400m
                        },
                        new
                        {
                            Id = new Guid("254a0faf-281b-4aad-b7ac-d1e9d34d3df2"),
                            CategoryName = "Phone",
                            CreatedBy = "Admin",
                            CreatedDate = new DateTime(2021, 6, 30, 23, 21, 28, 358, DateTimeKind.Local).AddTicks(4991),
                            Name = "Iphone 7",
                            Price = 400m
                        },
                        new
                        {
                            Id = new Guid("2098ad11-2c36-447a-b94c-01603d3f1b33"),
                            CategoryName = "Phone",
                            CreatedBy = "Admin",
                            CreatedDate = new DateTime(2021, 6, 30, 23, 21, 28, 358, DateTimeKind.Local).AddTicks(4994),
                            Name = "Iphone 8",
                            Price = 400m
                        },
                        new
                        {
                            Id = new Guid("9f890060-76d6-4c69-a088-7655cbcf3ba7"),
                            CategoryName = "Phone",
                            CreatedBy = "Admin",
                            CreatedDate = new DateTime(2021, 6, 30, 23, 21, 28, 358, DateTimeKind.Local).AddTicks(4998),
                            Name = "Iphone 10",
                            Price = 400m
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
