﻿// <auto-generated />
using System;
using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Entities.Migrations
{
    [DbContext(typeof(CMSDevDbContext))]
    [Migration("20240124124323_AddSessionTokenActiveMigration")]
    partial class AddSessionTokenActiveMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Entities.Account", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<bool>("Active")
                        .HasColumnType("boolean");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean");

                    b.Property<bool>("UserLocal")
                        .HasColumnType("boolean");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("Accounts", (string)null);
                });

            modelBuilder.Entity("Entities.AxonscmsSession", b =>
                {
                    b.Property<Guid>("SessionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("session_id")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<string>("AccountId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("account_id");

                    b.Property<string>("Browser")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("browser");

                    b.Property<DateTime?>("CreateAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("create_at");

                    b.Property<string>("CreateBy")
                        .HasColumnType("character varying")
                        .HasColumnName("create_by");

                    b.Property<DateTime>("ExpirationTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("expiration_time");

                    b.Property<DateTime>("IssuedTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("issued_time");

                    b.Property<DateTime>("LoginAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("login_at");

                    b.Property<string>("LoginIp")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("login_ip");

                    b.Property<string>("Os")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("os");

                    b.Property<string>("Platform")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("platform");

                    b.Property<string>("SessionStatus")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("character varying")
                        .HasColumnName("session_status")
                        .HasDefaultValueSql("'A'::bpchar")
                        .HasComment("B (Blocked): Session ยังไม่ได้ใช้งาน\r\nA (Active): Session กำลังใช้งานอยู่\r\nE (Expired): Session หมดอายุแล้ว");

                    b.Property<string>("Token")
                        .HasColumnType("character varying")
                        .HasColumnName("token");

                    b.Property<DateTime?>("UpdateAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("update_at");

                    b.Property<string>("UpdateBy")
                        .HasColumnType("character varying")
                        .HasColumnName("update_by");

                    b.HasKey("SessionId")
                        .HasName("cms_session_pk");

                    b.ToTable("axonscms_session", (string)null);
                });

            modelBuilder.Entity("Entities.BlockBruteforce", b =>
                {
                    b.Property<Guid>("BlockForceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("block_force_id")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<int>("Count")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(0)
                        .HasColumnName("count");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("created_at");

                    b.Property<DateTime?>("LockedTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("locked_time");

                    b.Property<string>("Status")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("character varying")
                        .HasColumnName("status")
                        .HasDefaultValueSql("'A'::character varying")
                        .HasComment("L (Locked): ถูกล็อก\r\nU (UnLock): ไม่ล็อก");

                    b.Property<DateTime?>("UnLockTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("un_lock_time");

                    b.Property<DateTime?>("UpdateAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("update_at");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("character varying")
                        .HasColumnName("username");

                    b.HasKey("BlockForceId")
                        .HasName("block_bruteforce_pk");

                    b.ToTable("block_bruteforce", (string)null);
                });

            modelBuilder.Entity("Entities.Roles", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("Create_At")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Create_By")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("RoleCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("Update_At")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Update_By")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("Roles", (string)null);

                    b.HasData(
                        new
                        {
                            Id = "e946f4cc-fca1-439c-8f67-55584676103d",
                            Create_At = new DateTimeOffset(new DateTime(2024, 1, 24, 19, 43, 23, 236, DateTimeKind.Unspecified).AddTicks(8231), new TimeSpan(0, 7, 0, 0, 0)),
                            Create_By = "Configure",
                            Name = "Administrator",
                            NormalizedName = "ADMINISTRATOR",
                            RoleCode = "R01",
                            Update_At = new DateTimeOffset(new DateTime(2024, 1, 24, 19, 43, 23, 236, DateTimeKind.Unspecified).AddTicks(8262), new TimeSpan(0, 7, 0, 0, 0)),
                            Update_By = ""
                        },
                        new
                        {
                            Id = "053c95d0-2749-4e76-8391-80859e5b56b1",
                            Create_At = new DateTimeOffset(new DateTime(2024, 1, 24, 19, 43, 23, 236, DateTimeKind.Unspecified).AddTicks(8267), new TimeSpan(0, 7, 0, 0, 0)),
                            Create_By = "Configure",
                            Name = "User",
                            NormalizedName = "USER",
                            RoleCode = "R02",
                            Update_At = new DateTimeOffset(new DateTime(2024, 1, 24, 19, 43, 23, 236, DateTimeKind.Unspecified).AddTicks(8268), new TimeSpan(0, 7, 0, 0, 0)),
                            Update_By = ""
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("RoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("text");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("UserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .HasColumnType("text");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("UserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("UserTokens", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Entities.Roles", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Entities.Account", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Entities.Account", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Entities.Roles", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Entities.Account", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Entities.Account", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
