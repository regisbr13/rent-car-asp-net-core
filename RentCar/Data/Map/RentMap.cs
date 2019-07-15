using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentCar.Models;

namespace RentCar.Data.Map
{
    public class RentMap : IEntityTypeConfiguration<Rent>
    {
        public void Configure(EntityTypeBuilder<Rent> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Start).IsRequired().HasMaxLength(15);
            builder.Property(x => x.End).IsRequired().HasMaxLength(15);
            builder.Property(x => x.Value).IsRequired();

            builder.HasOne(x => x.User).WithMany(x => x.Rents).HasForeignKey(x => x.UserId);
            builder.HasOne(x => x.Car).WithMany(x => x.Rents).HasForeignKey(x => x.CarId);

            builder.ToTable("Alugueis");
        }
    }
}
