using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Online_Bookstore.Models
{
    // Đây là user mặc định kế thừa từ IdentityUser
    public class ApplicationUser : IdentityUser
    {
        // Nếu muốn thêm thông tin người dùng thì khai báo ở đây, ví dụ:
        // public string FullName { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);

            // Add thêm claim nếu cần
            return userIdentity;
        }
    }

    // DbContext dành cho Identity
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        // Nếu bạn có bảng khác thì thêm DbSet ở đây
        // public DbSet<Book> Books { get; set; }
    }
}
