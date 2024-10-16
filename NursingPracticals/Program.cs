
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NursingPracticals.Contexts;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace NursingPracticals
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                if (builder.Environment.IsDevelopment())
                {
                    options.EnableSensitiveDataLogging(true);
                }
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"), opt =>
                {
                    opt.UseRelationalNulls(true)
                    .EnableRetryOnFailure(3)
                    .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                }).UseLowerCaseNamingConvention();
            });

            builder.Services.AddIdentity<ApplicationUsers, IdentityRole>(x =>
            {
                x.SignIn.RequireConfirmedAccount = true;
                x.SignIn.RequireConfirmedEmail = true;
                x.Lockout.AllowedForNewUsers = true;
                x.Password.RequiredLength = 8;
                x.Password.RequireNonAlphanumeric = true;
                x.Password.RequireDigit = true;
                x.Password.RequireUppercase = true;
                x.Password.RequireLowercase = true;
                x.Tokens.AuthenticatorTokenProvider = "4Hy3ORZgRwe64{z0rC8L]J0x-KN8ZBeT";
                x.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
            }).AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddSingleton<IAppFeatures, AppFeatures>();
            builder.Services.AddAuthorizationBuilder()
                .AddPolicy("Administrators", auth =>
                {
                    auth.RequireAuthenticatedUser();
                    auth.RequireClaim(ClaimTypes.Role, "Administrator");
                })
                .AddPolicy("Tutors", auth =>
                {
                    auth.RequireAuthenticatedUser().RequireClaim(ClaimTypes.Role, "Tutor");
                });
            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("AppFeatures").GetSection("SigningKey").Value)),
                    ValidateIssuer = true,
                    RequireExpirationTime = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    IgnoreTrailingSlashWhenValidatingAudience = true,
                    ValidIssuer = builder.Configuration!.GetSection("AppFeatures").GetSection("Issuer").Value!,
                    ValidAudience = builder.Configuration.GetSection("AppFeatures").GetSection("Audience").Value
                };
            });
            builder.Services.AddDataProtection();
            builder.Services.AddAntiforgery(options =>
            {
                options.HeaderName = "X-XSRF-TOKEN";
                options.Cookie = new CookieBuilder()
                {
                    Name = "XSRF-TOKEN",
                    HttpOnly = false,
                    IsEssential = true,
                    SameSite = SameSiteMode.None,
                    //Domain = builder.Configuration.GetSection("AppFeatures").GetSection("Issuer").Value,
                    //SecurePolicy = CookieSecurePolicy.SameAsRequest
                };
            });
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("bStudioApps",
                    x => x.WithOrigins("http://localhost:4200", "https://edu-app.berntech-gh.online/")
                    .WithHeaders("Content-Type", "Accept", "Origin", "Authorization", "X-XSRF-TOKEN", "XSRF-TOKEN", "enctype", "Access-Control-Allow-Origin", "Access-Control-Allow-Credentials", "File-Details")
                    .WithMethods("GET", "POST", "OPTIONS", "PUT", "DELETE", "PATCH")
                        .AllowCredentials());
            });

            builder.Services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.KnownProxies.Add(IPAddress.Parse("10.0.0.100"));
            });
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped(typeof(CancellationToken),
                serviceProvider => serviceProvider.GetRequiredService<IHttpContextAccessor>()
                    .HttpContext!.RequestAborted);

            builder.Services.AddControllersWithViews();
            var app = builder.Build();
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseRouting();
            app.UseCors("bStuioApps");
            var antiforgery = app.Services.GetRequiredService<IAntiforgery>();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();

        }
    }
}
