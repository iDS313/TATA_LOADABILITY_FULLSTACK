using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Loadability.DatabaseSeeder;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Loadability.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            Database.SetInitializer(new MasterDataSeeder());
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        public DbSet<Sku> Sku { get; set; }
        public DbSet<Cfa> Cfa { get; set; }
        public DbSet<PrDetails> PrDetails { get; set; }
        public DbSet<LoadPlan> LoadPlans { get; set; }

        public DbSet<DailyPlan> DailyPlan { get; set; }
        public DbSet<StockDetails> StockDetails { get; set; }
        public DbSet<Priority> Priority { get; set; }
        public DbSet<Truck> Trucks { get; set; }
        public DbSet<FileModel> FileModels { get; set; }
    }
}