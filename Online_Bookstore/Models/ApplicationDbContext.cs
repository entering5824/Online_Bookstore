using Online_Bookstore.Models;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace Online_Bookstore { 

        public class ApplicationDbContext : DbContext
        {
            public ApplicationDbContext() : base("DefaultConnection")
            {
                // Disable lazy loading to avoid N+1 queries and deadlocks
                this.Configuration.LazyLoadingEnabled = false;
                
                // Disable proxy creation to avoid performance issues
                this.Configuration.ProxyCreationEnabled = false;
                
                // Set command timeout to 5 seconds for faster error detection
                this.Database.CommandTimeout = 5;
                
                // Disable database initialization to work with existing database
                Database.SetInitializer<ApplicationDbContext>(null);
                
                // Enable logging for debugging
                #if DEBUG
                this.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
                #endif
            }

            public static ApplicationDbContext Create()
            {
                return new ApplicationDbContext();
            }

            // DbSets for all entities
            public DbSet<Book> Books { get; set; }
            public DbSet<BookCategory> BookCategories { get; set; }
            public DbSet<User> Users { get; set; }
            public DbSet<ActivityLog> ActivityLogs { get; set; }
            public DbSet<BorrowRecord> BorrowRecords { get; set; }
            public DbSet<DigitalResource> DigitalResources { get; set; }
            public DbSet<Notification> Notifications { get; set; }
            public DbSet<Reservation> Reservations { get; set; }
            public DbSet<SystemSetting> SystemSettings { get; set; }

            protected override void OnModelCreating(DbModelBuilder modelBuilder)
            {
                // Remove pluralizing convention to prevent EF from changing table names
                modelBuilder.Conventions.Remove<System.Data.Entity.ModelConfiguration.Conventions.PluralizingTableNameConvention>();
                
                base.OnModelCreating(modelBuilder);

                // Configure BookCategory relationship
                modelBuilder.Entity<BookCategory>()
                            .HasMany(c => c.Books)
                            .WithOptional(b => b.Category)
                            .HasForeignKey(b => b.CategoryId);

                // Note: Navigation properties are not defined in current models
                // Relationships will be handled through foreign keys only

                // Note: EF6 doesn't support HasIndex() - indexes should be created via migrations or SQL scripts
                // If you need unique constraints, use ColumnAnnotation or create them manually in database
            }

            // Override SaveChanges to add audit fields
            public override int SaveChanges()
            {
                try
                {
                    return base.SaveChanges();
                }
                catch (DbUpdateException ex)
                {
                    // Log the exception or handle it appropriately
                    System.Diagnostics.Debug.WriteLine($"Database update error: {ex.Message}");
                    throw;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"General database error: {ex.Message}");
                    throw;
                }
            }

            // Method to test database connection
            public bool TestConnection()
            {
                try
                {
                    return this.Database.Exists();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Connection test failed: {ex.Message}");
                    return false;
                }
            }
        }
    }

