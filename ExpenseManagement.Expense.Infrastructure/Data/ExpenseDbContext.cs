using ExpenseManagement.BuildingBlocks.Common.Models;
using ExpenseManagement.Expense.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;


namespace ExpenseManagement.Expense.Infrastructure.Data
{
    public class ExpenseDbContext : DbContext
    {
        public ExpenseDbContext(DbContextOptions<ExpenseDbContext> options) : base(options) { }

        public DbSet<Domain.Entities.Expense> Expenses { get; set; }
        public DbSet<ExpenseItem> ExpenseItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Receipt> Receipts { get; set; }
        public DbSet<Department> Departments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Expense Configuration
            modelBuilder.Entity<Domain.Entities.Expense>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.UserId);
                entity.HasIndex(e => e.Status);
                entity.HasIndex(e => e.ExpenseDate);

                entity.Property(e => e.Title).HasMaxLength(200).IsRequired();
                entity.Property(e => e.Description).HasMaxLength(1000);
                entity.Property(e => e.TotalAmount).HasPrecision(18, 2);
                entity.Property(e => e.Currency).HasMaxLength(3);

                entity.HasOne(e => e.Department)
                    .WithMany(d => d.Expenses)
                    .HasForeignKey(e => e.DepartmentId)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasQueryFilter(e => !e.IsDeleted);
            });

            // ExpenseItem Configuration
            modelBuilder.Entity<ExpenseItem>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Description).HasMaxLength(500).IsRequired();
                entity.Property(e => e.Amount).HasPrecision(18, 2);
                entity.Property(e => e.UnitPrice).HasPrecision(18, 2);

                entity.HasOne(ei => ei.Expense)
                    .WithMany(e => e.Items)
                    .HasForeignKey(ei => ei.ExpenseId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(ei => ei.Category)
                    .WithMany(c => c.ExpenseItems)
                    .HasForeignKey(ei => ei.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Category Configuration
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Code).IsUnique();
                entity.HasIndex(e => e.Name);

                entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Code).HasMaxLength(20).IsRequired();
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.MaxAmountPerExpense).HasPrecision(18, 2);

                entity.HasOne(c => c.ParentCategory)
                    .WithMany(c => c.SubCategories)
                    .HasForeignKey(c => c.ParentCategoryId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasQueryFilter(e => !e.IsDeleted);
            });

            // Receipt Configuration
            modelBuilder.Entity<Receipt>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.FileName).HasMaxLength(255).IsRequired();
                entity.Property(e => e.FilePath).HasMaxLength(500).IsRequired();
                entity.Property(e => e.ContentType).HasMaxLength(100);

                entity.HasOne(r => r.Expense)
                    .WithMany(e => e.Receipts)
                    .HasForeignKey(r => r.ExpenseId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Department Configuration
            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Code).IsUnique();
                entity.HasIndex(e => e.Name);

                entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Code).HasMaxLength(20).IsRequired();
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.MonthlyBudget).HasPrecision(18, 2);

                entity.HasQueryFilter(e => !e.IsDeleted);
            });

            // Seed Data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Departments
            var itDeptId = Guid.Parse("D1111111-1111-1111-1111-111111111111");
            var hrDeptId = Guid.Parse("D2222222-2222-2222-2222-222222222222");
            var salesDeptId = Guid.Parse("D3333333-3333-3333-3333-333333333333");

            modelBuilder.Entity<Department>().HasData(
                new Department { Id = itDeptId, Name = "Information Technology", Code = "IT", Description = "IT Department", IsActive = true, CreatedAt = DateTime.UtcNow, CreatedBy = "System" },
                new Department { Id = hrDeptId, Name = "Human Resources", Code = "HR", Description = "HR Department", IsActive = true, CreatedAt = DateTime.UtcNow, CreatedBy = "System" },
                new Department { Id = salesDeptId, Name = "Sales", Code = "SALES", Description = "Sales Department", IsActive = true, CreatedAt = DateTime.UtcNow, CreatedBy = "System" }
            );

            // Seed Categories
            var categories = new[]
            {
            new Category { Id = Guid.Parse("C1111111-1111-1111-1111-111111111111"), Name = "Travel", Code = "TRV", Description = "Travel expenses", IsActive = true, RequiresReceipt = true, CreatedAt = DateTime.UtcNow, CreatedBy = "System" },
            new Category { Id = Guid.Parse("C2222222-2222-2222-2222-222222222222"), Name = "Meals & Entertainment", Code = "M&E", Description = "Meals and entertainment", IsActive = true, RequiresReceipt = true, MaxAmountPerExpense = 150, CreatedAt = DateTime.UtcNow, CreatedBy = "System" },
            new Category { Id = Guid.Parse("C3333333-3333-3333-3333-333333333333"), Name = "Office Supplies", Code = "OFF", Description = "Office supplies", IsActive = true, RequiresReceipt = true, CreatedAt = DateTime.UtcNow, CreatedBy = "System" },
            new Category { Id = Guid.Parse("C4444444-4444-4444-4444-444444444444"), Name = "Transportation", Code = "TRN", Description = "Transportation", IsActive = true, RequiresReceipt = false, CreatedAt = DateTime.UtcNow, CreatedBy = "System" },
            new Category { Id = Guid.Parse("C5555555-5555-5555-5555-555555555555"), Name = "Accommodation", Code = "ACC", Description = "Hotel and lodging", IsActive = true, RequiresReceipt = true, CreatedAt = DateTime.UtcNow, CreatedBy = "System" },
            new Category { Id = Guid.Parse("C6666666-6666-6666-6666-666666666666"), Name = "Training", Code = "TRG", Description = "Training and education", IsActive = true, RequiresReceipt = true, CreatedAt = DateTime.UtcNow, CreatedBy = "System" },
            new Category { Id = Guid.Parse("C7777777-7777-7777-7777-777777777777"), Name = "Communication", Code = "COM", Description = "Phone and internet", IsActive = true, RequiresReceipt = true, CreatedAt = DateTime.UtcNow, CreatedBy = "System" },
            new Category { Id = Guid.Parse("C8888888-8888-8888-8888-888888888888"), Name = "Miscellaneous", Code = "MISC", Description = "Other expenses", IsActive = true, RequiresReceipt = false, CreatedAt = DateTime.UtcNow, CreatedBy = "System" }
        };

            modelBuilder.Entity<Category>().HasData(categories);
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
