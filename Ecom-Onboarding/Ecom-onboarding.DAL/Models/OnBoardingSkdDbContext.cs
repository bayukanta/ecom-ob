using Microsoft.EntityFrameworkCore;

namespace Ecom_Onboarding.DAL.Models
{
    public class OnBoardingSkdDbContext : DbContext
    {
        public OnBoardingSkdDbContext(DbContextOptions<OnBoardingSkdDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //builder.Entity<VisibilityTicket>().HasIndex(a => new { a.TicketTypeId, a.CallTypeId, a.SubCallTypeId }).IsUnique(true);
        }

        public DbSet<Game> Game { get; set; }
        public DbSet<Publisher> Publisher { get; set; }


    }
}

