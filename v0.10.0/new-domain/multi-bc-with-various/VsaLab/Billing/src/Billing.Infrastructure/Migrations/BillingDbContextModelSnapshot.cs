﻿// <auto-generated />
using System;
using Billing.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Billing.Infrastructure.Migrations
{
    [DbContext(typeof(BillingDbContext))]
    partial class BillingDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.6")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("Billing.Core.Entities.Patient", b =>
                {
                    b.Property<Guid>("PatientId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset?>("Dob")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Ethnicity")
                        .HasColumnType("text");

                    b.Property<string>("ExternalId")
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .HasColumnType("text");

                    b.Property<string>("Gender")
                        .HasColumnType("text");

                    b.Property<string>("InternalId")
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .HasColumnType("text");

                    b.Property<string>("Race")
                        .HasColumnType("text");

                    b.Property<string>("Sex")
                        .HasColumnType("text");

                    b.HasKey("PatientId");

                    b.ToTable("Patient");
                });
#pragma warning restore 612, 618
        }
    }
}