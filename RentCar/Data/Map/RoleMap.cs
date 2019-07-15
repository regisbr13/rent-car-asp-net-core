using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentCar.Models;

namespace RentCar.Data.Map
{
    public class RoleMap : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.Property(x => x.Description).IsRequired().HasMaxLength(100);

            builder.ToTable("Niveis_Acesso");
        }
    }
}
