using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentCar.Models;

namespace RentCar.Data.Map
{
    public class AccountMap : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Balance).IsRequired();

            builder.HasOne(x => x.User).WithOne(x => x.Account).HasForeignKey<Account>(x => x.UserId);

            builder.ToTable("Contas");
        }
    }
}
