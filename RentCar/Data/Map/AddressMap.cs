using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentCar.Models;

namespace RentCar.Data.Map
{
    public class AddressMap : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Street).IsRequired().HasMaxLength(30);
            builder.Property(x => x.State).IsRequired().HasMaxLength(20);
            builder.Property(x => x.District).IsRequired().HasMaxLength(30);

            builder.HasOne(x => x.User).WithMany(x => x.Addresses).HasForeignKey(x => x.UserId);

            builder.ToTable("Endereco");
        }
    }
}
