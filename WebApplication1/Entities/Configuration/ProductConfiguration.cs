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
                    Id = new Guid("aad4c053-49b6-410c-bc78-2d54a9991870"),
                    Name = "cool name",
                    Price = 1773,
                    CompanyId = new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870")
                },
                new Product {
                    Id = new Guid("abd4c053-49b6-410c-bc78-2d54a9991870"),
                    Name = "untitled",
                    Price = 300,
                    CompanyId = new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870")
                }
            );
        }
    }
}
