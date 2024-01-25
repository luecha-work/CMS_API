using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Configuration
{
    public class RoleConfiguration : IEntityTypeConfiguration<Roles>
    {
        public void Configure(EntityTypeBuilder<Roles> builder)
        {
            builder.HasData(
                new Roles
                {
                    RoleCode = "R01",
                    Name = "Administrator",
                    NormalizedName = "ADMINISTRATOR",
                    Create_At = DateTimeOffset.Now,
                    Update_At = DateTimeOffset.Now,
                    Create_By = "Configure",
                    Update_By = ""
                },
                new Roles
                {
                    RoleCode = "R02",
                    Name = "User",
                    NormalizedName = "USER",
                    Create_At = DateTimeOffset.Now,
                    Update_At = DateTimeOffset.Now,
                    Create_By = "Configure",
                    Update_By = ""
                }
            );
        }
    }
}
