using Microsoft.EntityFrameworkCore.Metadata.Builders;

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
           
            var superAdmin = new User
            {
                Id = 1,
                CreationTime = Clock.Now,
                FirstName = "Super Admin",
                LastName = "User",
                LastLoginDate = Clock.Now,
                Email = "superadmin@seerbit.com",
                EmailConfirmed = true,
                NormalizedEmail = "superadmin@seerbit.com".ToUpper(),
                UserName = "superadmin@seerbit.com",
                NormalizedUserName = "superadmin@seerbit.com".ToUpper(),
                PasswordHash = Hasher.HashPassword(null, "micr0s0ft_"),
                SecurityStamp = "99ae0c45-d682-4542-9ba7-1281e471916b",
                IsFirstTimeLogin = false,
                DateOfBirth = new DateTime(1990, 2, 18, 0, 0, 0),
                IsSuperAdmin = true,
                UserStatus = UserStatus.Activated,
                UserType = UserType.SuperAdmin
            };
            var employee = new User
            {
                Id = 4,
                CreationTime = Clock.Now,
                FirstName = "Employee",
                LastName = "Employee",
                LastLoginDate = Clock.Now,
                Email = "employee@seerbit.com",
                EmailConfirmed = true,
                NormalizedEmail = "employee@seerbit.com".ToUpper(),
                UserName = "employee@seerbit.com",
                NormalizedUserName = "employee@seerbit.com".ToUpper(),
                PasswordHash = Hasher.HashPassword(null, "micr0s0ft_"),
                SecurityStamp = "99ae0c45-d682-4542-9ba7-1281e471916b",
                IsFirstTimeLogin = false,
                DateOfBirth = new DateTime(1999, 2, 18, 0, 0, 0),
                IsSuperAdmin = true,
                UserStatus = UserStatus.Activated,
                UserType = UserType.Employee
            };
            var employee2 = new User
            {
                Id = 3,
                CreationTime = Clock.Now,
                FirstName = "Employee",
                LastName = "Employee",
                LastLoginDate = Clock.Now,
                Email = "employee2@seerbit.com",
                EmailConfirmed = true,
                NormalizedEmail = "employee2@seerbit.com".ToUpper(),
                UserName = "employee2@seerbit.com",
                NormalizedUserName = "employee2@seerbit.com".ToUpper(),
                PasswordHash = Hasher.HashPassword(null, "micr0s0ft_"),
                SecurityStamp = "99ae0c45-d682-4542-9ba7-1281e471916b",
                IsFirstTimeLogin = false,
                DateOfBirth = new DateTime(1999, 2, 18, 0, 0, 0),
                IsSuperAdmin = true,
                UserStatus = UserStatus.Activated,
                UserType = UserType.Employee
            };
            var companyAdmin = new User
            {
                Id = 2,
                CreationTime = Clock.Now,
                FirstName = "Company",
                LastName = "Admin",
                LastLoginDate = Clock.Now,
                Email = "companyadmin@seerbit.com",
                EmailConfirmed = true,
                NormalizedEmail = "companyadmin@seerbit.com".ToUpper(),
                UserName = "superadmin@seerbit.com",
                NormalizedUserName = "companyadmin@seerbit.com".ToUpper(),
                PasswordHash = Hasher.HashPassword(null, "micr0s0ft_"),
                SecurityStamp = "99ae0c45-d682-4542-9ba7-1281e471916b",
                IsFirstTimeLogin = false,
                DateOfBirth = new DateTime(1997, 2, 18, 0, 0, 0),
                IsSuperAdmin = true,
                UserStatus = UserStatus.Activated,
                UserType = UserType.CompanyAdmin
            };

            var users = new List<User>() { companyAdmin,superAdmin,employee,employee2 };

            builder.HasData(users);
        }
    }
}
