using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Configuration
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasData(
                new Customer {
                    Id = new Guid("f9d4c053-49b6-410c-bc78-2d54a9991870"),
                    Name = "Sachin Woolley",
                    Age = 28,
                    ProductId = new Guid("abd4c053-49b6-410c-bc78-2d54a9991870")
                }
            );

        }
    }
}
