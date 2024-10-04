using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using AuthDemo.Data;
using InventorizationBackend.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using AuthDemo.Swagger;
using Microsoft.Extensions.Options;
using InventorizationBackend.Interfaces;
using InventorizationBackend.Services;
using InventorizationBackend.Helper;

namespace InventorizationBackend
{
  public class Program
  {
    public static async Task Main(string[] args)
    {
      var builder = WebApplication.CreateBuilder(args);

      // Add services to the container.
      builder.Services.AddControllers();
      builder.Services.AddAutoMapper(typeof(MappingProfiles));
      builder.Services.AddScoped<IProductService, ProductService>();
      builder.Services.AddScoped<IAuthService, AuthService>();

      builder.Services.AddDbContext<DataContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

      // Configure Identity
      builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
          .AddEntityFrameworkStores<DataContext>()
          .AddDefaultTokenProviders();

      builder.Services.AddAuthentication(options =>
      {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
      })
      .AddJwtBearer(options =>
      {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
          ValidateIssuer = true,
          ValidateAudience = true,
          ValidAudience = builder.Configuration["JWT:ValidAudience"],
          ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
          IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
        };
      });

      builder.Services.AddEndpointsApiExplorer();
      builder.Services.AddSwaggerGen();
      builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, SwaggerConfig>();

      var app = builder.Build();

      // Configure the HTTP request pipeline.
      if (app.Environment.IsDevelopment())
      {
        app.UseSwagger();
        app.UseSwaggerUI();
      }

      app.UseHttpsRedirection();
      app.UseAuthentication();
      app.UseAuthorization();
      app.MapControllers();

      using (var scope = app.Services.CreateScope())
      {
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        await SeedRolesAndAdminUser(roleManager, userManager);
      }

      app.Run();
    }

    private static async Task SeedRolesAndAdminUser(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
    {
      // Seed Roles
      //if (!await roleManager.RoleExistsAsync("ADMIN"))
      //  await roleManager.CreateAsync(new IdentityRole("ADMIN"));

      //if (!await roleManager.RoleExistsAsync("OPERATOR"))
      //  await roleManager.CreateAsync(new IdentityRole("OPERATOR"));

      // Seed Initial Admin User
      var adminEmail = "georgievteodor281@gmail.com";
      var adminUser = await userManager.FindByEmailAsync(adminEmail);

      if (adminUser == null)
      {
        adminUser = new ApplicationUser
        {
          UserName = adminEmail,
          Email = adminEmail,
          EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(adminUser, "admin123");

        if (result.Succeeded)
        {
          await userManager.AddToRoleAsync(adminUser, "ADMIN");
        }
      }
    }
  }
}
