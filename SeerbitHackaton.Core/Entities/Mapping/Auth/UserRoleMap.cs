using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Entities.Mapping
{
    public class UserRoleMap : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.ToTable(nameof(UserRole));
            SetupUserRole(builder);
        }

        private void SetupUserRole(EntityTypeBuilder<UserRole> builder)
        {
            var role1=new UserRole
            { RoleId= 1 , UserId=1};
            var role2 = new UserRole
            { RoleId = 2, UserId = 2 };
            var role3 = new UserRole
            { RoleId = 3, UserId = 3 };
            var role4 = new UserRole
            { RoleId = 3, UserId = 4 };
            var roles=new List<UserRole>() { role1,role2,role3,role4};
            builder.HasData(roles);
        }
    }
}
