using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace LoggingSystem.API.Data
{
    public class AuthDbContext : IdentityDbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) 
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            var roleId = "4cec5e78-44f8-471e-b826-2680b2789a21";

            // Adding Just one Role as Admin in Registration And i can add more roles
            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = roleId,
                    ConcurrencyStamp = roleId,
                    Name = "Admin",
                    NormalizedName = "Admin".ToUpper()
                }
            };

            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}
