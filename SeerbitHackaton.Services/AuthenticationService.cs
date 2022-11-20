global using SeerbitHackaton.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SeerbitHackaton.Core.DataAccess.EfCore.Context;
using SeerbitHackaton.Core.Entities;
using SeerbitHackaton.Core.Enums;
using SeerbitHackaton.Core.Utils;
using Shared.Pagination;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SeerbitHackaton.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;
        private static readonly Random random = new Random();
        private readonly RoleManager<Role> _roleManager;
        //private readonly IEmailService _emailService;
        public AuthenticationService(UserManager<User> userManager, IConfiguration configuration, ApplicationDbContext context, RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _configuration = configuration;
            _context = context;
            _roleManager = roleManager;
            //_emailService = emailService;
        }
        public async Task<ResultModel<LoginResponseVM>> LoginAsync(LoginVM model, DateTime currentDate)
        {
            var resultModel = new ResultModel<LoginResponseVM>();
            try
            {
                var user = await GetUser(model.EmailAddress);
                if (user == null)
                {
                    resultModel.AddError("Incorrect email or password");
                    return resultModel;
                }

                if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    var response = await _userManager.IsEmailConfirmedAsync(user);
                    if (user.UserStatus == UserStatus.Deactivated || response == false)
                    {
                        resultModel.AddError("You have to confirm your email and change your password");
                        return resultModel;
                    }
                    var userRoles = await _userManager.GetRolesAsync(user);
                    var (token, expiration) = CreateJwtTokenAsync(user, userRoles);

                    await UpdateUserLastLogin(user.Email, currentDate, false);

                    var data = new LoginResponseVM()
                    {
                        Token = token,
                        Expiration = expiration,
                        Roles = userRoles,
                    };

                    if (user.RefreshTokens.Any(a => a.IsActive))
                    {
                        var activeRefreshToken = user.RefreshTokens
                            .Where(a => a.IsActive == true)
                            .FirstOrDefault();
                        // authenticationModel.RefreshToken = activeRefreshToken.Token;
                        // authenticationModel.RefreshTokenExpiration = activeRefreshToken.Expires;
                    }
                    else
                    {
                        var refreshToken = CreateRefreshToken(currentDate);
                        //authenticationModel.RefreshToken = refreshToken.Token;
                        //authenticationModel.RefreshTokenExpiration = refreshToken.Expires;
                        user.RefreshTokens.Add(refreshToken);
                        _context.Update(user);
                        _context.SaveChanges();
                    }


                    var recipients = new List<string>
                    {
                        user.Email
                    };
                    string body = $"<h3>This is to notify you that you just logged in.</h3>";
                    //var status = await SendEmail(recipients, "Login", body);
                    //if (!status)
                    //{
                    //    resultModel.AddError("Unable to send mail");
                    //    return resultModel;
                    //}
                    resultModel.Data = data;
                    return resultModel;
                }

                resultModel.AddError("Incorrect email or password");
                resultModel.Message = "Incorrect email or password";
                return resultModel;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task<User> GetUser(string emailAddress)
            => await _userManager.FindByEmailAsync(emailAddress);
        private (string, DateTime) CreateJwtTokenAsync(User user, IList<string> userRoles)
        {
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

            IdentityOptions identityOptions = new();

            var userClaims = new List<Claim>()
            {
                new Claim(identityOptions.ClaimsIdentity.UserIdClaimType, user.Id.ToString()),
                new Claim(identityOptions.ClaimsIdentity.UserNameClaimType, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id.ToString()),
                new Claim("LastLoginDate", user.LastLoginDate == null ? "Not set" : user.LastLoginDate.ToString()),
                new Claim("FirstName", user.FirstName?.ToString()),
                new Claim("LastName", user.LastName?.ToString()),
              new Claim("IsSuperAdmin", user.IsSuperAdmin ? "True" : "False"),
                new Claim("IsCompanyAdmin", user.IsCompanyAdmin ? "True" : "False"),
                new Claim("IsEmployee", user.IsEmployee ? "True" : "False")
            };

            foreach (var userRole in userRoles)
            {
                userClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var signKey = new SymmetricSecurityKey(key);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _configuration.GetSection("JWT:Issuer").Value,
                audience: _configuration.GetSection("JWT:Audience").Value,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(_configuration.GetSection("JWT:DurationInMinutes").Value)),
                claims: userClaims, signingCredentials: new SigningCredentials(signKey, SecurityAlgorithms.HmacSha256));

            return (new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken), jwtSecurityToken.ValidTo);
        }
        public async Task<ResultModel<bool>> ChangePasswordAsync(ChangePasswordVM model, DateTime currentDate)
        {
            var resultModel = new ResultModel<bool>();

            try
            {
                var user = await GetUser(model.EmailAddress);
                if (user == null)
                {
                    resultModel.AddError("Incorrect username or password");
                    return resultModel;
                }
                if (user.UserStatus == UserStatus.Activated)
                {
                    resultModel.AddError("Account already activated");
                    return resultModel;
                }
                if (user != null && await _userManager.CheckPasswordAsync(user, model.OldPassword))
                {
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var activateAccount = await _userManager.ConfirmEmailAsync(user, token);
                    if (activateAccount.Succeeded == false)
                    {
                        resultModel.AddError("Email not confirmed!");
                        return resultModel;
                    }
                    var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var response = await _userManager.ResetPasswordAsync(user, code, model.NewPassword);
                    if (response.Succeeded == false)
                    {
                        foreach (var item in response.Errors)
                        {
                            resultModel.AddError(item.Description);
                        }
                        return resultModel;
                    }
                    await UpdateUserLastLogin(user.Email, currentDate, true);

                }
                resultModel.Data = true;
                resultModel.Message = "Email confirmed and password changed";
                return resultModel;
            }
            catch (Exception ex)
            {
                resultModel.AddError(ex.Message);
                return resultModel;
            }
        }

        public async Task<ResultModel<LoginResponseVM>> RefreshTokenAsync(string token, DateTime date)
        {
            var resultModel = new ResultModel<LoginResponseVM>();
            var user = _context.Users.SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));
            if (user == null)
            {
                resultModel.AddError($"Token did not match any users.");
                return resultModel;
            }
            var refreshToken = user.RefreshTokens.Single(x => x.Token == token);
            if (!refreshToken.IsActive)
            {
                resultModel.AddError($"Token Not Active.");
                return resultModel;
            }

            //Revoke current refresh token
            refreshToken.Revoked = DateTime.UtcNow;

            //Generate new Refresh Token and save to Database
            var newRefreshToken = CreateRefreshToken(date);
            user.RefreshTokens.Add(newRefreshToken);
            _context.Update(user);
            _context.SaveChanges();

            var userRoles = await _userManager.GetRolesAsync(user);
            var (newtoken, expiration) = CreateJwtTokenAsync(user, userRoles);

            await UpdateUserLastLogin(user.Email, date, false);

            var data = new LoginResponseVM()
            {
                Token = token,
                Expiration = expiration,
                Roles = userRoles,
            };

            resultModel.Data = data;
            return resultModel;
        }
        private RefreshToken CreateRefreshToken(DateTime date)
        {
            var randomNumber = new byte[32];
            using var generator = new RNGCryptoServiceProvider();
            generator.GetBytes(randomNumber);
            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomNumber),
                Expires = date.AddMinutes(Convert.ToInt32(_configuration.GetSection("JWT:DurationInMinutes").Value)),
                Created = date
            };
        }
        private async Task<ResultModel<bool>> UpdateUserLastLogin(string emailAddress, DateTime CurrentDate, bool activated)
        {
            var result = new ResultModel<bool>();
            try
            {
                var user = await GetUser(emailAddress);

                if (user == null)
                {
                    result.AddError("Incoorrect username or password");
                    result.Data = false;
                    return result;
                };

                if (activated == true)
                    user.UserStatus = UserStatus.Activated;
                user.LastLoginDate = CurrentDate;
                await _context.SaveChangesAsync();

                result.Data = true;
                return result;
            }
            catch (Exception ex)
            {
                // _log.LogError("{0}--------at {1}()", ex.Message ?? ex.InnerException.Message, nameof(UpdateUserLastLogin));
                result.AddError(ex.Message);
                return result;
            }
        }
        public async Task<ResultModel<bool>> InviteUser(RegisterUserVM model)
        {
            var result = new ResultModel<bool>();
            try
            {
                var existingUser = await _userManager.FindByEmailAsync(model.Email);

                if (existingUser != null)
                {
                    result.Message = "Account exists";
                    result.AddError("Account exists");
                    result.Data = false;
                    return result;
                }

                var role = await _roleManager.FindByNameAsync(model.Role);

                if (role == null)
                {
                    result.AddError("Role does not exist");
                    result.Message = "Role does not exist";
                    return result;
                }

                var user = new User
                {
                    FirstName = model.FirstName,
                    Email = model.Email,
                    DateOfBirth = model.DateOfBirth,
                    LastName = model.LastName,
                    UserName = model.Email.ToLower(),
                    EmailConfirmed = false,
                    UserStatus = UserStatus.Deactivated,
                    PhoneNumber = model.PhoneNumber,
                    Gender = model.Gender,
                    //State = model.State,
                };
                switch (role.Name)
                {
                    case AppRoles.SuperAdmin:
                        user.UserType = UserType.SuperAdmin;
                        user.IsSuperAdmin = true;
                        break;
                    case AppRoles.CompanyAdmin:
                        user.UserType = UserType.CompanyAdmin;
                        user.IsCompanyAdmin = true;
                        break;
                    case AppRoles.Employee:
                        user.UserType = UserType.Employee;
                        user.IsEmployee = true;
                        break;
                    default:
                        break;
                }
                var password = GeneratePassword();

                var response = await _userManager.CreateAsync(user, password);
                // AN ERROR OCCURED WHILE CREATING USER
                if (!response.Succeeded)
                {
                    foreach (var error in response.Errors)
                    {
                        result.AddError(error.Description);
                    }
                    return result;
                };

                await _userManager.AddToRoleAsync(user, role.Name);
                var recipients = new List<string>
                    {
                        user.Email
                    };
                string body = $"<h3>This is to notify you that your account has been created at DCI.<br> Email Address: {model.Email}<br>Password: {password}<br>Please change your password before you can login</h3>";
                var status = await SendEmail(recipients, "Account Invitation", body);
                if (!status)
                {
                    result.AddError("UNABLE TO SEND MAIL");
                    return result;
                }
                result.Data = true;
                result.Message = "User Created Successfully";

                return result;
            }
            catch (Exception)
            {

                throw;
            }


        }
        public async Task<ResultModel<PaginatedModel<UserVM>>> GetAllUsers(QueryModel model)
        {
            var resultModel = new ResultModel<PaginatedModel<UserVM>>();
            var query = GetAllUsers();
            if (query == null)
            {
                resultModel.AddError("No existing user");
                return resultModel;
            }
            if (model != null)
                EntityFilter(query, model);
            var usersPaged = query.ToPagedList((int)model.PageIndex, (int)model.PageSize);
            var usersVms = usersPaged.Select(x => (UserVM)x).ToList();
            var data = new PaginatedModel<UserVM>(usersVms, (int)model.PageIndex, (int)model.PageSize, usersPaged.TotalItemCount);
            resultModel.Data = data;
            resultModel.Message = $"Found {usersPaged.Count} cases";
            return resultModel;
        }
        public async Task<ResultModel<UserVM>> GetUserAsync(string email)
        {
            var resultModel = new ResultModel<UserVM>();
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                resultModel.AddError("User not found");
                return resultModel;
            };

            var userRoles = await _userManager.GetRolesAsync(user);
            resultModel.Data = user;
            resultModel.Data.Roles = userRoles;
            resultModel.Message = "User retrieved";
            return resultModel;

        }
        public async Task<ResultModel<string>> AssignUserToRole(string email, string role, string CurrentUserID)
        {
            var result = new ResultModel<string>();
            try
            {
                var user = await GetUser(email);
                if (user == null)
                {
                    result.AddError("User does not exist");
                    return result;
                };

                if (await _roleManager.RoleExistsAsync(role))
                {
                    if (!await _userManager.IsInRoleAsync(user, role))
                    {
                        await _userManager.AddToRoleAsync(user, role);
                        result.Message = $"User with email address {email} added to role {role}";
                        //var adminUser = await GetUserById(CurrentUserID);

                        //await _emailSender.SendEmailAsync(email, "FSDH ROLE", $"You have been added to an {role} role");
                        // await _emailService.SendMail(email, "FSDH ROLE", $"You have been added to an {role} role");
                        return result;
                    }
                    result.AddError("User already has this role");
                    return result;
                }

                result.AddError("Role not found");
                return result;
            }
            catch (Exception ex)
            {
                result.AddError(ex.Message);
                return result;
            }
        }
        public async Task<ResultModel<List<RoleVm>>> GetAllRoles()
        {
            var result = new ResultModel<List<RoleVm>>();
            try
            {
                var roles = _roleManager.Roles.Select(r => new RoleVm
                {
                    Role = r.Name
                }).ToList();

                result.Data = roles;
                return result;
            }
            catch (Exception ex)
            {
                result.AddError(ex.Message);
                return result;
            }
        }
        public async Task<ResultModel<bool>> ResetPassword(ResetPasswordVM model, long userId, DateTime currentDate)
        {
            var resultModel = new ResultModel<bool>();

            try
            {
                var user = await GetUserById(userId);
                if (user == null)
                {
                    resultModel.AddError("Incorrect username or password");
                    return resultModel;
                };

                if (model.Password != model.ConfirmPassword)
                {
                    resultModel.AddError("Passwords do not match");
                    return resultModel;
                }


                user.PasswordHash = new PasswordHasher<User>().HashPassword(user, model.Password);
                user.LastLoginDate = currentDate;
                var resetResult = await _userManager.UpdateAsync(user);

                if (!resetResult.Succeeded)
                {
                    foreach (var error in resetResult.Errors)
                    {
                        resultModel.AddError(error.Description);
                    }
                    return resultModel;
                };

                resultModel.Message = "Password Changed Successfully";
                resultModel.Data = true;
                return resultModel;
            }
            catch (Exception ex)
            {
                //_log.LogError("{0}--------at {1}()", ex.Message ?? ex.InnerException.Message, nameof(ResetPassword));
                resultModel.AddError(ex.Message);
                return resultModel;
            }
        }
        private async Task<User> GetUserById(long id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            return user;
        }
        public string GeneratePassword()
        {
            var numbers = "982345173";
            var capital = "AQWSDETHFUJNGF";
            var small = "adginfhy";
            var specialChar = "#$%&(+";

            var randomNum = new string(Enumerable.Repeat(numbers, 5)
              .Select(s => s[random.Next(s.Length)]).ToArray());

            var randomcapital = new string(Enumerable.Repeat(capital, 1)
              .Select(s => s[random.Next(s.Length)]).ToArray());

            var randomsmall = new string(Enumerable.Repeat(small, 2)
              .Select(s => s[random.Next(s.Length)]).ToArray());

            var randomspecialChar = new string(Enumerable.Repeat(specialChar, 1)
              .Select(s => s[random.Next(s.Length)]).ToArray());


            return randomNum + randomspecialChar + randomsmall + randomcapital;
        }
        private IQueryable<User> GetAllUsers() => _context.Users.AsQueryable();
        private async Task<bool> SendEmail(List<string> recipients, string subject, string body)
        {
            //return await _emailService.SendMail(recipients, subject, body);
        }
        private IQueryable<User> EntityFilter(IQueryable<User> query, QueryModel model)
        {
            if (!string.IsNullOrEmpty(model.Keyword) && !string.IsNullOrEmpty(model.Filter))
            {
                var keyword = model.Keyword.ToLower().Trim();
                switch (model.Filter)
                {
                    case "email":
                        {
                            query = query.Where(x => x.Email.ToLower().Contains(keyword)).OrderByDescending(x => x.CreationTime);
                            break;
                        }
                    case "firstname":
                        {
                            query = query.Where(x => x.FirstName.ToLower().Contains(keyword)).OrderByDescending(x => x.CreationTime);
                            break;
                        }
                    case "lastname":
                        {
                            query = query.Where(x => x.LastName.ToLower().Contains(keyword)).OrderByDescending(x => x.CreationTime);
                            break;
                        }
                    case "phonenumber":
                        {
                            query = query.Where(x => x.PhoneNumber.ToLower().Contains(keyword)).OrderByDescending(x => x.CreationTime);
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }
            return query.OrderByDescending(x => x.Id);
        }
    }
}
