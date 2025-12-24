using Microsoft.EntityFrameworkCore;
using ModelLayer.Entity;



namespace DataLogicLayer.Context
{
    public class FundooContext : DbContext
    {
        public FundooContext(DbContextOptions<FundooContext> options)
            : base(options)
        {
        }

        public DbSet<Users> Users { get; set; }
        public DbSet<Notes> Notes { get; set; }

    }
}
