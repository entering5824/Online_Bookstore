using Online_Bookstore.Models;
using System.Data.Entity;

namespace Online_Bookstore
{
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext() : base("DefaultConnection")
    {
    }

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
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<BookCategory>()
                    .HasMany(c => c.Books)
                    .WithOptional(b => b.Category)
                    .HasForeignKey(b => b.CategoryId);
    }
}}
