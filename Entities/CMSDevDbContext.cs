using Entities.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Entities
{
    public class CMSDevDbContext : IdentityDbContext<Account, Roles, string> // ปรับ IdentityDbContext ให้ใช้ Account และ Roles
    {
        public CMSDevDbContext(DbContextOptions<CMSDevDbContext> options)
            : base(options) { }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public virtual DbSet<BlockBruteforce> BlockBruteforces { get; set; }
        public virtual DbSet<AxonscmsSession> AxonscmsSessions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //TODO: First create data to database
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new RoleConfiguration());

            // สามารถให้รูปแบบที่ต้องการสำหรับตาราง Identity ได้
            modelBuilder.Entity<Account>().ToTable("Accounts");
            modelBuilder.Entity<Roles>().ToTable("Roles");
            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
            modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
            modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
            modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
            modelBuilder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");

            modelBuilder.Entity<BlockBruteforce>(entity =>
            {
                entity.HasKey(e => e.BlockForceId).HasName("block_bruteforce_pk");

                entity.ToTable("block_bruteforce");

                entity
                    .Property(e => e.BlockForceId)
                    .HasDefaultValueSql("uuid_generate_v4()")
                    .HasColumnName("block_force_id");
                entity.Property(e => e.Count).HasDefaultValue(0).HasColumnName("count");
                entity
                    .Property(e => e.CreatedAt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("created_at");
                entity.Property(e => e.LockedTime).HasColumnName("locked_time");
                entity
                    .Property(e => e.Status)
                    .HasDefaultValueSql("'A'::character varying")
                    .HasComment("L (Locked): ถูกล็อก\r\nU (UnLock): ไม่ล็อก")
                    .HasColumnType("character varying")
                    .HasColumnName("status");
                entity.Property(e => e.UnLockTime).HasColumnName("un_lock_time");
                entity
                    .Property(e => e.UpdateAt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("update_at");
                entity
                    .Property(e => e.Username)
                    .HasColumnType("character varying")
                    .HasColumnName("username");
            });

            modelBuilder.Entity<AxonscmsSession>(entity =>
            {
                entity.HasKey(e => e.SessionId).HasName("cms_session_pk");

                entity.ToTable("axonscms_session");

                entity
                    .Property(e => e.SessionId)
                    .HasDefaultValueSql("uuid_generate_v4()")
                    .HasColumnName("session_id");
                entity.Property(e => e.AccountId).HasColumnName("account_id");
                entity
                    .Property(e => e.Token)
                    .HasColumnType("character varying")
                    .HasColumnName("token");
                entity.Property(e => e.Browser).HasMaxLength(50).HasColumnName("browser");
                entity.Property(e => e.CreateAt).HasColumnName("create_at");
                entity
                    .Property(e => e.CreateBy)
                    .HasColumnType("character varying")
                    .HasColumnName("create_by");
                entity.Property(e => e.ExpirationTime).HasColumnName("expiration_time");
                entity.Property(e => e.IssuedTime).HasColumnName("issued_time");
                entity.Property(e => e.LoginAt).HasColumnName("login_at");
                entity.Property(e => e.LoginIp).HasMaxLength(50).HasColumnName("login_ip");
                entity.Property(e => e.Os).HasMaxLength(50).HasColumnName("os");
                entity.Property(e => e.Platform).HasMaxLength(50).HasColumnName("platform");
                entity
                    .Property(e => e.SessionStatus)
                    .HasDefaultValueSql("'A'::bpchar")
                    .HasComment(
                        "B (Blocked): Session ยังไม่ได้ใช้งาน\r\nA (Active): Session กำลังใช้งานอยู่\r\nE (Expired): Session หมดอายุแล้ว"
                    )
                    .HasColumnType("character varying")
                    .HasColumnName("session_status");
                entity.Property(e => e.UpdateAt).HasColumnName("update_at");
                entity
                    .Property(e => e.UpdateBy)
                    .HasColumnType("character varying")
                    .HasColumnName("update_by");
            });
        }
    }

    public class CMSDevDbContextFactory : IDesignTimeDbContextFactory<CMSDevDbContext>
    {
        public CMSDevDbContext CreateDbContext(string[] args)
        {
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<CMSDevDbContext>();
            var conn = config.GetConnectionString("DefaultConnection");
            optionsBuilder.UseNpgsql(conn);
            return new CMSDevDbContext(optionsBuilder.Options);
        }
    }
}
