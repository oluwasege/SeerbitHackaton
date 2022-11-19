global using Microsoft.AspNetCore.Identity;

namespace SeerbitHackaton.Core.Entities
{
    public class User : IdentityUser<long>, IHasCreationTime, IHasDeletionTime, ISoftDelete, IHasModificationTime
    {
        public string FirstName { get; set; }
        public string Unit { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public bool IsFirstTimeLogin { get; set; } = true;
        public DateTime CreationTime { get; set; }
        public DateTime? DeletionTime { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public UserType UserType { get; set; }
        public UserStatus UserStatus { get; set; }

        [NotMapped]
        public string FullName
        {
            get
            {
                return $"{LastName} {FirstName}";
            }
        }
    }

    public class UserClaim : IdentityUserClaim<long> { }

    public class UserRole : IdentityUserRole<long> { }

    public class UserLogin : IdentityUserLogin<long>
    {
        public int Id { get; set; }
    }

    public class RoleClaim : IdentityRoleClaim<long> { }

    public class UserToken : IdentityUserToken<long> { }

    public static class UserExtensions
    {
        //public static bool IsDefaultAccount(this User user)
        //{
        //    return CoreConstants.DefaultAccount == user.UserName;
        //}

        public static bool IsNull(this User user)
        {
            return user == null;
        }

        //public static bool IsConfirmed(this User user)
        //{
        //    return user.EmailConfirmed || user.PhoneNumberConfirmed;
        //}

        //public static bool AccountLocked(this User user)
        //{
        //    return user.LockoutEnabled == true;
        //}

        //public static bool HasNoPassword(this User user)
        //{
        //    return string.IsNullOrWhiteSpace(user.PasswordHash);
        //}
    }
}
