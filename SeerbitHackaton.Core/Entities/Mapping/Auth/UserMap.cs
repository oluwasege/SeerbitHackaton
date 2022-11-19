using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Timing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Entities.Mapping
{
    public class UserMap : IEntityTypeConfiguration<User>
    {
        public PasswordHasher<User> Hasher { get; set; } = new PasswordHasher<User>();

        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable(nameof(User));

            builder.Property(b => b.FirstName).HasMaxLength(150);
            builder.Property(b => b.LastName).HasMaxLength(150);
            builder.Property(b => b.MiddleName).HasMaxLength(150);

            SetupAdmin(builder);
        }

        private void SetupAdmin(EntityTypeBuilder<User> builder)
        {
            var user1 = new User
            {
                Id = 1,
                CreationTime = Clock.Now,
                FirstName = "Super Admin",
                LastName = "User",
                LastLoginDate = Clock.Now,
                Email = "root@myschooltrack.com",
                EmailConfirmed = true,
                NormalizedEmail = "root@myschooltrack.com".ToUpper(),
                UserName = "root@myschooltrack.com",
                NormalizedUserName = "root@myschooltrack.com".ToUpper(),
                PasswordHash = Hasher.HashPassword(null, "micr0s0ft_"),
                SecurityStamp = "99ae0c45-d682-4542-9ba7-1281e471916b",
                IsFirstTimeLogin = false
            };

            var users = new List<User>() { user1 };

            builder.HasData(users);
        }
    }
}
