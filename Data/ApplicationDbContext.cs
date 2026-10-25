using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SSD_LAB1.Models;

namespace SSD_LAB1.Data
{
    public class ApplicationDbContext :
        IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Company> Companies { get; set; }

        public DbSet<CompanyContact> CompanyContacts { get; set; }

        public DbSet<CompanyContract> CompanyContracts { get; set; }
        public DbSet<Message> Messages { get; set; }
    }
}