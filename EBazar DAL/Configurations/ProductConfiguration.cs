using EBazar_DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBazar_DAL.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(x => x.Name)
                .HasColumnType("nvarchar(100)");
            builder.Property(x => x.Description)
                .HasColumnType("nvarchar(1000)");
            builder.Property(x => x.Price)
                 .HasColumnType("float");
        }
    }
}
