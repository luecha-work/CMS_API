using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Entities;

public partial class CmsDbContext : DbContext
{
    public CmsDbContext()
    {
    }

    public CmsDbContext(DbContextOptions<CmsDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AxonscmsSession> AxonscmsSessions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Database=cms_db;Username=root;Password=123456");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("uuid-ossp");

        modelBuilder.Entity<AxonscmsSession>(entity =>
        {
            entity.HasKey(e => e.SessionId).HasName("cms_session_pk");

            entity.ToTable("axonscms_session");

            entity.Property(e => e.SessionId)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("session_id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.Browser)
                .HasMaxLength(50)
                .HasColumnName("browser");
            entity.Property(e => e.CreateAt).HasColumnName("create_at");
            entity.Property(e => e.CreateBy)
                .HasColumnType("character varying")
                .HasColumnName("create_by");
            entity.Property(e => e.ExpirationTime).HasColumnName("expiration_time");
            entity.Property(e => e.IssuedTime).HasColumnName("issued_time");
            entity.Property(e => e.LoginAt).HasColumnName("login_at");
            entity.Property(e => e.LoginIp)
                .HasMaxLength(50)
                .HasColumnName("login_ip");
            entity.Property(e => e.Os)
                .HasMaxLength(50)
                .HasColumnName("os");
            entity.Property(e => e.Platform)
                .HasMaxLength(50)
                .HasColumnName("platform");
            entity.Property(e => e.SessionStatus)
                .HasDefaultValueSql("'A'::bpchar")
                .HasComment("B (Blocked): Session ยังไม่ได้ใช้งาน\r\nA (Active): Session กำลังใช้งานอยู่\r\nE (Expired): Session หมดอายุแล้ว")
                .HasColumnType("character varying")
                .HasColumnName("session_status");
            entity.Property(e => e.UpdateAt).HasColumnName("update_at");
            entity.Property(e => e.UpdateBy)
                .HasColumnType("character varying")
                .HasColumnName("update_by");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
