using ExpenseManagement.BuildingBlocks.Common.Models;
using ExpenseMaqnagement.Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ExpenseManagement.Identity.Infrastructure.Data
{
    public class IdentityDbContext : DbContext
    {
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
      
        public DbSet<UserClaim> UserClaims { get; set; }

        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User Configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasIndex(e => e.Username).IsUnique();
                entity.Property(e => e.Email).HasMaxLength(256).IsRequired();
                entity.Property(e => e.Username).HasMaxLength(100).IsRequired();
                entity.Property(e => e.FirstName).HasMaxLength(100).IsRequired();
                entity.Property(e => e.LastName).HasMaxLength(100).IsRequired();
                entity.HasQueryFilter(e => !e.IsDeleted);
            });

            // Role Configuration
            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Name).IsUnique();
                entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
                entity.HasQueryFilter(e => !e.IsDeleted);
            });

            // UserRole Configuration
            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => new { e.UserId, e.RoleId }).IsUnique();

                entity.HasOne(ur => ur.User)
                    .WithMany(u => u.UserRoles)
                    .HasForeignKey(ur => ur.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Permission Configuration
            modelBuilder.Entity<Permission>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Name).IsUnique();
                entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
            });

            // RolePermission Configuration
            modelBuilder.Entity<RolePermission>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => new { e.RoleId, e.PermissionId }).IsUnique();

                entity.HasOne(rp => rp.Role)
                    .WithMany(r => r.RolePermissions)
                    .HasForeignKey(rp => rp.RoleId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(rp => rp.Permission)
                    .WithMany(p => p.RolePermissions)
                    .HasForeignKey(rp => rp.PermissionId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.TokenHash)
                      .HasMaxLength(512)
                      .IsRequired();

                // Unique index for lookup during refresh
                entity.HasIndex(e => e.TokenHash)
                      .IsUnique();

                // Composite index for cleanup + per-user queries
                entity.HasIndex(e => new { e.UserId, e.ExpiresAt });

                entity.Property(e => e.ExpiresAt)
                      .IsRequired();

                entity.HasOne(rt => rt.User)
                      .WithMany(u => u.RefreshTokens)
                      .HasForeignKey(rt => rt.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });



            // UserClaim Configuration
            modelBuilder.Entity<UserClaim>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasOne(uc => uc.User)
                    .WithMany(u => u.UserClaims)
                    .HasForeignKey(uc => uc.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Seed Data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Roles
            var employeeRoleId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var managerRoleId = Guid.Parse("22222222-2222-2222-2222-222222222222");
            var financeRoleId = Guid.Parse("33333333-3333-3333-3333-333333333333");
            var adminRoleId = Guid.Parse("44444444-4444-4444-4444-444444444444");

            modelBuilder.Entity<Role>().HasData(
                new Role { Id = employeeRoleId, Name = "Employee", Description = "Regular employee", CreatedAt = DateTime.UtcNow, CreatedBy = "System" },
                new Role { Id = managerRoleId, Name = "Manager", Description = "Department manager", CreatedAt = DateTime.UtcNow, CreatedBy = "System" },
                new Role { Id = financeRoleId, Name = "Finance", Description = "Finance team member", CreatedAt = DateTime.UtcNow, CreatedBy = "System" },
                new Role { Id = adminRoleId, Name = "Admin", Description = "System administrator", CreatedAt = DateTime.UtcNow, CreatedBy = "System" }
            );

            // Seed Permissions
            var permissions = new[]
            {
            new Permission { Id = Guid.NewGuid(), Name = "expense.create", Description = "Create expenses", CreatedAt = DateTime.UtcNow, CreatedBy = "System" },
            new Permission { Id = Guid.NewGuid(), Name = "expense.read.own", Description = "Read own expenses", CreatedAt = DateTime.UtcNow, CreatedBy = "System" },
            new Permission { Id = Guid.NewGuid(), Name = "expense.read.all", Description = "Read all expenses", CreatedAt = DateTime.UtcNow, CreatedBy = "System" },
            new Permission { Id = Guid.NewGuid(), Name = "expense.update.own", Description = "Update own expenses", CreatedAt = DateTime.UtcNow, CreatedBy = "System" },
            new Permission { Id = Guid.NewGuid(), Name = "expense.delete.own", Description = "Delete own expenses", CreatedAt = DateTime.UtcNow, CreatedBy = "System" },
            new Permission { Id = Guid.NewGuid(), Name = "approval.process", Description = "Process approvals", CreatedAt = DateTime.UtcNow, CreatedBy = "System" },
            new Permission { Id = Guid.NewGuid(), Name = "reimbursement.process", Description = "Process reimbursements", CreatedAt = DateTime.UtcNow, CreatedBy = "System" },
            new Permission { Id = Guid.NewGuid(), Name = "reports.view", Description = "View reports", CreatedAt = DateTime.UtcNow, CreatedBy = "System" },
            new Permission { Id = Guid.NewGuid(), Name = "users.manage", Description = "Manage users", CreatedAt = DateTime.UtcNow, CreatedBy = "System" }
        };

            modelBuilder.Entity<Permission>().HasData(permissions);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateAuditFields();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateAuditFields()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is BaseEntity &&
                           (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entry in entries)
            {
                var entity = (BaseEntity)entry.Entity;

                if (entry.State == EntityState.Added)
                {
                    entity.CreatedAt = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entity.UpdatedAt = DateTime.UtcNow;
                }
            }
        }
    }
}
