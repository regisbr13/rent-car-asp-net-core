using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentCar.Models;

namespace RentCar.Data.Map
{
    public class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Cpf).IsRequired();
            builder.HasIndex(x => x.Cpf).IsUnique();

            builder.HasMany(x => x.Addresses).WithOne(x => x.User).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(x => x.Rents).WithOne(x => x.User).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.Account).WithOne(x => x.User).OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("Usuarios");
        }
    }
}
