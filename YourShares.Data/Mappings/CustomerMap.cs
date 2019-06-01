using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YourShares.Domain.Models;

namespace YourShares.Data.Mappings
{
    public class CustomerMap : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.Property(c => c.Id)
                .HasColumnName("Id");

            builder.Property(c => c.Address)
                .HasColumnType("varchar(255)")
                .HasMaxLength(255);
//
//            builder.Property(c => c.CompanyCode)
//                .HasColumnType("varchar(255)")
//                .HasMaxLength(255);
        }
    }
}