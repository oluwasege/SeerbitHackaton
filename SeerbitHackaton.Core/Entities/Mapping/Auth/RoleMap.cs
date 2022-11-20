using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Entities.Mapping
{
    public class RoleMap : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable(nameof(Role));
            SetupRoles(builder);
        }

        private void SetupRoles(EntityTypeBuilder<Role> builder)
        {
            var adminRole = new Role
            {
                Id = 1,
                Name = AppRoles.SuperAdmin
            };
            var companyAdminRole = new Role
            {
                Id = 2,
                Name = AppRoles.CompanyAdmin
            };
            var employeRole = new Role
            {
                Id = 3,
                Name = AppRoles.Employee
            };

            var roles=new List<Role> { adminRole, companyAdminRole,employeRole };
            builder.HasData(roles);

        }
    }
}
