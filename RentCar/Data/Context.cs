using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RentCar.Data.Map;
using RentCar.Models;

namespace RentCar.Data
{
    public class Context : IdentityDbContext
    {
        public DbSet<User> User { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Rent> Rents { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Address> Addresses { get; set; }

        public Context(DbContextOptions<Context> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new UserMap());
            modelBuilder.ApplyConfiguration(new RoleMap());
            modelBuilder.ApplyConfiguration(new CarMap());
            modelBuilder.ApplyConfiguration(new RentMap());
            modelBuilder.ApplyConfiguration(new AccountMap());
            modelBuilder.ApplyConfiguration(new AddressMap());
        }

    }
}
