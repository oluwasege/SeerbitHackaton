using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SeerbitHackaton.Core.AspnetCore;
using SeerbitHackaton.Core.DataAccess.EfCore;
using SeerbitHackaton.Core.DataAccess.EfCore.Context;
using SeerbitHackaton.Core.DataAccess.EfCore.UnitOfWork;
using SeerbitHackaton.Core.Entities;
using SeerbitHackaton.Core.FileStorage;
using SeerbitHackaton.Core.Utils;
using System.Configuration;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using System;

namespace SeerbitHackaton.API
{
    public static class ServiceExtension
    {
        public static void AddDatabaseConfiguration(this IServiceCollection services, IConfiguration Configuration)
        {
            //set the database to use SQL Server.
            services.AddDbContext<ApplicationDbContext>(
              options => options.UseSqlServer(Configuration.GetConnectionString("Default"),
              p =>
              {
                  p.EnableRetryOnFailure();
                  p.MaxBatchSize(150);
              }));

            services.AddIdentity<User, Role>(
               options =>
               {
                   // Password settings.
                   options.Password.RequireDigit = false;
                   options.Password.RequireLowercase = false;
                   options.Password.RequireUppercase = false;
                   options.Password.RequireNonAlphanumeric = false;
                   options.Password.RequiredLength = 6;

                   options.Lockout.AllowedForNewUsers = true;
                   options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
                   options.Lockout.MaxFailedAccessAttempts = 3;
                   options.User.RequireUniqueEmail = false;
               }
              ).AddEntityFrameworkStores<ApplicationDbContext>()
              .AddDefaultTokenProviders();

            services.Configure<DataProtectionTokenProviderOptions>(options =>
            {
                options.TokenLifespan = TimeSpan.FromHours(24);
            });
            //services.AddHangfire(configuration =>
            //{
            //    var sqlOptions = new SqlServerStorageOptions
            //    {
            //        CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
            //        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
            //        QueuePollInterval = TimeSpan.Zero,
            //        UseRecommendedIsolationLevel = true,
            //        UsePageLocksOnDequeue = true,
            //        DisableGlobalLocks = true
            //    };

            //    configuration.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
            //    .UseSimpleAssemblyNameTypeSerializer()
            //    .UseRecommendedSerializerSettings()
            //    .UseSqlServerStorage(
            //    Configuration.GetConnectionString(connection),
            //    sqlOptions);
            //});

            //// Add the processing server as IHostedService
            //services.AddHangfireServer();
        }

        public static void AddSettingsAndAuthentication(this IServiceCollection services, IConfiguration Configuration)
        {

            //set JWT
            var JwtSettingsSection = Configuration.GetSection(nameof(JwtSettings));
            services.Configure<JwtSettings>(JwtSettingsSection);

            var jwtSettings = JwtSettingsSection.Get<JwtSettings>();

            //Encode Secret Key
            var secretKey = Encoding.UTF8.GetBytes(jwtSettings.SecretKey);

            //Add jwt authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
             .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
             {
                 options.SaveToken = true;
                 options.RequireHttpsMetadata = true;
                 options.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidateAudience = true,
                     ValidateIssuer = true,
                     ValidateIssuerSigningKey = false,
                     ValidateLifetime = true,
                     ValidIssuer = jwtSettings.Site,
                     //ValidAudience = jwtSettings.Audience,
                     IssuerSigningKey = new SymmetricSecurityKey(secretKey),
                     RequireExpirationTime = true,
                     ClockSkew = TimeSpan.Zero
                 };
             });



        }

        public static void AddServices(this IServiceCollection services, IWebHostEnvironment HostingEnvironment, IConfiguration Configuration)
        {
            services.AddTransient<DbContext, ApplicationDbContext>();
            services.AddScoped<IUnitOfWork, EfCoreUnitOfWork>();
            services.AddScoped(typeof(IDbContextProvider<>), typeof(UnitOfWorkDbContextProvider<>));
            services.RegisterGenericRepos(typeof(ApplicationDbContext));

            services.AddScoped<IHttpUserService, HttpUserService>();
            services.AddHttpContextAccessor();
            services.AddHttpClient();
            Directory.CreateDirectory(Path.Combine(HostingEnvironment.ContentRootPath, Configuration.GetValue<string>("StoragePath")));

            services.AddSingleton<IFileProvider>(new PhysicalFileProvider(Path.Combine(
                          HostingEnvironment.ContentRootPath, Configuration.GetValue<string>("StoragePath"))));

            services.AddScoped<IFileStorageService, FileStorageService>();
        }

    }
}
