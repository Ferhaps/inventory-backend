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
    public static void Main(string[] args)
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

      using (var scope = app.Services.CreateScope())
      {
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        SeedRolesAndAdminUserAsync(roleManager, userManager);
      }

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
      app.Run();
    }

    static async Task SeedRolesAndAdminUserAsync(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
    {
      string[] roles = ["ADMIN", "OPERATOR"];

      foreach (var role in roles)
      {
        if (!await roleManager.RoleExistsAsync(role))
        {
          await roleManager.CreateAsync(new IdentityRole(role));
        }
      }

      // Step 2: Create the user if it doesn't exist
      string email = "georgievteodor281@gmail.com";
      string password = "admin123";
      var user = await userManager.FindByEmailAsync(email);

      if (user == null)
      {
        user = new ApplicationUser
        {
          UserName = email,
          Email = email,
          EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(user, password);
        if (!result.Succeeded)
        {
          foreach (var error in result.Errors)
          {
            Console.WriteLine(error.Description);
          }
          return;
        }
      }

      // Step 3: Assign the ADMIN role to the user if they don't already have it
      if (!await userManager.IsInRoleAsync(user, "ADMIN"))
      {
        await userManager.AddToRoleAsync(user, "ADMIN");
      }
    }

  }
}