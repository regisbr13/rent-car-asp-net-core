using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentCar.Models;

namespace RentCar.Data.Map
{
    public class CarMap : IEntityTypeConfiguration<Car>
    {
        public void Configure(EntityTypeBuilder<Car> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Brand).IsRequired().HasMaxLength(20);
            builder.Property(x => x.Model).IsRequired().HasMaxLength(20);
            builder.Property(x => x.ImgPath).IsRequired();
            builder.Property(x => x.DailyPrice).IsRequired();

            builder.HasMany(x => x.Rents).WithOne(x => x.Car).HasForeignKey(x => x.CarId).OnDelete(DeleteBehavior.Restrict);
            
            builder.ToTable("Carros");
        }
    }
}
