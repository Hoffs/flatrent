﻿// <auto-generated />
using System;
using FlatRent.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace FlatRent.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20181225163711_AddDefaultIdGeneration")]
    partial class AddDefaultIdGeneration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.2.0-rtm-35687")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("FlatRent.Entities.Address", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<string>("Country");

                    b.Property<Guid>("FlatId");

                    b.Property<string>("FlatNumber");

                    b.Property<string>("HouseNumber");

                    b.Property<string>("PostCode");

                    b.Property<string>("Street");

                    b.HasKey("Id");

                    b.HasIndex("FlatId")
                        .IsUnique();

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("FlatRent.Entities.ClientInformation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<string>("Description");

                    b.HasKey("Id");

                    b.ToTable("ClientInformations");
                });

            modelBuilder.Entity("FlatRent.Entities.EmployeeInformation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<string>("Department")
                        .HasMaxLength(64);

                    b.Property<string>("Position")
                        .HasMaxLength(64);

                    b.HasKey("Id");

                    b.ToTable("EmployeeInformations");
                });

            modelBuilder.Entity("FlatRent.Entities.Fault", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<Guid>("ClientInformationId");

                    b.Property<DateTime>("CreationDate");

                    b.Property<string>("Description");

                    b.Property<Guid>("EmployeeInformationId");

                    b.Property<Guid>("FlatId");

                    b.Property<float>("Price");

                    b.Property<bool>("Repaired");

                    b.HasKey("Id");

                    b.HasIndex("ClientInformationId");

                    b.HasIndex("EmployeeInformationId");

                    b.HasIndex("FlatId");

                    b.ToTable("Faults");
                });

            modelBuilder.Entity("FlatRent.Entities.Flat", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<Guid>("AddressId");

                    b.Property<float>("Area");

                    b.Property<string>("Description");

                    b.Property<int>("Floor");

                    b.Property<string>("Name");

                    b.Property<Guid>("OwnerId");

                    b.Property<Guid?>("OwnerId1");

                    b.Property<float>("Price");

                    b.Property<int>("RoomCount");

                    b.Property<int>("YearOfConstruction");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.HasIndex("OwnerId1");

                    b.ToTable("Flats");
                });

            modelBuilder.Entity("FlatRent.Entities.Invoice", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<float>("AmountPaid");

                    b.Property<float>("AmountToPay");

                    b.Property<DateTime>("CreationDate");

                    b.Property<DateTime>("PaidDate");

                    b.Property<Guid>("RentAgreementId");

                    b.HasKey("Id");

                    b.HasIndex("RentAgreementId");

                    b.ToTable("Invoices");
                });

            modelBuilder.Entity("FlatRent.Entities.Owner", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<string>("Account");

                    b.Property<string>("Email");

                    b.Property<string>("Name");

                    b.Property<string>("PhoneNumber");

                    b.HasKey("Id");

                    b.ToTable("Owners");
                });

            modelBuilder.Entity("FlatRent.Entities.Photo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<Guid>("FlatId");

                    b.Property<byte[]>("PhotoBytes");

                    b.HasKey("Id");

                    b.HasIndex("FlatId");

                    b.ToTable("Photos");
                });

            modelBuilder.Entity("FlatRent.Entities.RentAgreement", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<Guid>("ClientInformationId");

                    b.Property<string>("Comments");

                    b.Property<Guid?>("FlatId");

                    b.Property<DateTime>("From");

                    b.Property<DateTime>("To");

                    b.Property<bool>("Verified");

                    b.HasKey("Id");

                    b.HasIndex("ClientInformationId");

                    b.HasIndex("FlatId");

                    b.ToTable("RentAgreements");
                });

            modelBuilder.Entity("FlatRent.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<Guid>("ClientInformationId");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<Guid>("EmployeeInformationId");

                    b.Property<string>("FirstName")
                        .HasMaxLength(50);

                    b.Property<string>("LastName")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<string>("Password")
                        .HasMaxLength(64);

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(50);

                    b.Property<Guid>("TypeId");

                    b.HasKey("Id");

                    b.HasIndex("ClientInformationId");

                    b.HasIndex("EmployeeInformationId");

                    b.HasIndex("TypeId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("FlatRent.Entities.UserType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("UserTypes");

                    b.HasData(
                        new
                        {
                            Id = new Guid("ed42ea4b-9900-4477-af32-0336ca61eab1"),
                            Name = "Client"
                        },
                        new
                        {
                            Id = new Guid("268c6597-15cb-4ab1-9d39-8a7d7c85b3d1"),
                            Name = "Employee"
                        },
                        new
                        {
                            Id = new Guid("ee3d96b6-4243-4235-8231-9a9fced615fe"),
                            Name = "Administrator"
                        });
                });

            modelBuilder.Entity("FlatRent.Entities.Address", b =>
                {
                    b.HasOne("FlatRent.Entities.Flat", "Flat")
                        .WithOne("Address")
                        .HasForeignKey("FlatRent.Entities.Address", "FlatId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FlatRent.Entities.Fault", b =>
                {
                    b.HasOne("FlatRent.Entities.ClientInformation", "ClientInformation")
                        .WithMany()
                        .HasForeignKey("ClientInformationId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("FlatRent.Entities.EmployeeInformation", "EmployeeInformation")
                        .WithMany()
                        .HasForeignKey("EmployeeInformationId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("FlatRent.Entities.Flat", "Flat")
                        .WithMany("Faults")
                        .HasForeignKey("FlatId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FlatRent.Entities.Flat", b =>
                {
                    b.HasOne("FlatRent.Entities.User", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("FlatRent.Entities.Owner")
                        .WithMany("Flats")
                        .HasForeignKey("OwnerId1");
                });

            modelBuilder.Entity("FlatRent.Entities.Invoice", b =>
                {
                    b.HasOne("FlatRent.Entities.RentAgreement", "RentAgreement")
                        .WithMany()
                        .HasForeignKey("RentAgreementId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FlatRent.Entities.Photo", b =>
                {
                    b.HasOne("FlatRent.Entities.Flat", "Flat")
                        .WithMany("Photos")
                        .HasForeignKey("FlatId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FlatRent.Entities.RentAgreement", b =>
                {
                    b.HasOne("FlatRent.Entities.ClientInformation", "ClientInformation")
                        .WithMany("Agreements")
                        .HasForeignKey("ClientInformationId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("FlatRent.Entities.Flat")
                        .WithMany("Agreements")
                        .HasForeignKey("FlatId");
                });

            modelBuilder.Entity("FlatRent.Entities.User", b =>
                {
                    b.HasOne("FlatRent.Entities.ClientInformation", "ClientInformation")
                        .WithMany()
                        .HasForeignKey("ClientInformationId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("FlatRent.Entities.EmployeeInformation", "EmployeeInformation")
                        .WithMany()
                        .HasForeignKey("EmployeeInformationId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("FlatRent.Entities.UserType", "Type")
                        .WithMany("Users")
                        .HasForeignKey("TypeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
