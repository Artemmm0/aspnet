using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Configuration
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasData(
                new Product {
                    Id = new Guid("aaabbbcccc"),
                    Name = "some cool product name",
                    Price = 1773,
                    CompanyId = new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870")
                },
                new Product {
                    Id = new Guid("ffffffffff"),
                    Name = "untitled",
                    Price = 300,
                    CompanyId = new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870")
                }
            );
        }
    }
}
