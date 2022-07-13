﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using WITNetCoreProject.Models.Entities;

namespace WITNetCoreProject.Migrations
{
    [DbContext(typeof(RepositoryContext))]
    [Migration("20220609085132_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.25")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("WITNetCoreProject.Models.Entities.RefreshTokens", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasMaxLength(2147483647)
                        .IsUnicode(false);

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("TB_Refresh_Tokens");
                });

            modelBuilder.Entity("WITNetCoreProject.Models.Entities.Users", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasDefaultValueSql("now()");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("character varying(64)")
                        .HasDefaultValueSql("('SYSTEM')")
                        .HasMaxLength(64)
                        .IsUnicode(false);

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasColumnType("character varying(64)")
                        .HasMaxLength(64)
                        .IsUnicode(false);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("character varying(64)")
                        .HasMaxLength(64)
                        .IsUnicode(false);

                    b.Property<bool?>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("character varying(15)")
                        .HasMaxLength(15)
                        .IsUnicode(false);

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("character varying(64)")
                        .HasMaxLength(64)
                        .IsUnicode(false);

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasDefaultValueSql("now()");

                    b.Property<string>("UpdatedBy")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("character varying(64)")
                        .HasDefaultValueSql("('SYSTEM')")
                        .HasMaxLength(64)
                        .IsUnicode(false);

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("character varying(15)")
                        .HasMaxLength(15)
                        .IsUnicode(false);

                    b.HasKey("UserId");

                    b.ToTable("TB_Users");
                });

            modelBuilder.Entity("WITNetCoreProject.Models.Entities.RefreshTokens", b =>
                {
                    b.HasOne("WITNetCoreProject.Models.Entities.Users", "User")
                        .WithMany("RefreshTokens")
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK_RefreshTokens_Users")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
