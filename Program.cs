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
      builder.Services.AddMemoryCache();
      builder.Services.AddAutoMapper(typeof(MappingProfiles));
      builder.Services.AddScoped<IProductService, ProductService>();
      builder.Services.AddScoped<IAuthService, AuthService>();
      builder.Services.AddScoped<ICategoryService, CategoryService>();
      builder.Services.AddScoped<IUserService, UserService>();

      builder.Services.AddCors(options =>
      {
        options.AddDefaultPolicy(builder =>
        {
          builder.AllowAnyOrigin()
          .AllowAnyMethod()
          .AllowAnyHeader();
        });
      });

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
      })
      .AddJwtBearer(options =>
      {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
          ValidateIssuer = true,
          ValidIssuer = builder.Configuration["JWT:Issuer"],

          ValidateAudience = true,
          ValidAudiences = builder.Configuration.GetSection("JWT:Audiences").Get<string[]>(),

          ValidateIssuerSigningKey = true,
          IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
        };
      });

      builder.Services.AddEndpointsApiExplorer();
      builder.Services.AddSwaggerGen();
      builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, SwaggerConfig>();

      var app = builder.Build();

      // Create initial User Roles
      using (var scope = app.Services.CreateScope())
      {
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var roles = new[] { "ADMIN", "OPERATOR" };

        foreach (var role in roles)
        {
          if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
        }
      }

      // Configure the HTTP request pipeline.
      if (app.Environment.IsDevelopment())
      {
        app.UseSwagger();
        app.UseSwaggerUI();
      }

      app.UseHttpsRedirection();

      app.UseCors();

      app.UseAuthentication();

      app.UseAuthorization();

      app.MapControllers();


      app.Run();
    }
  }
}
