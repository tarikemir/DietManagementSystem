using DietManagementSystem.Persistence;
using DietManagementSystem.Application.Common;
using DietManagementSystem.Infrastructure.Repositories;
using FluentValidation;
using DietManagementSystem.Application.Settings;
using DietManagementSystem.Application.Services;
using DietManagementSystem.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using DietManagementSystem.Domain.Enums;
using DietManagementSystem.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Serilog;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using DietManagementSystem.Application.Features.Auth.Login;

// Serilog Configuration
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json")
        .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development"}.json", true)
        .Build())
    .CreateLogger();

try
{
    Log.Information("Starting Diet Management System API");

    var builder = WebApplication.CreateBuilder(args);

    // Configure Serilog for the application
    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 30)
        .WriteTo.Seq("http://localhost:5341"));

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();

    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Diet Management System API", Version = "v1" });

        var jwtSecurityScheme = new OpenApiSecurityScheme
        {
            Scheme = "bearer",
            BearerFormat = "JWT",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Description = "Enter your JWT Bearer token below. Example: Bearer {your token}",

            Reference = new OpenApiReference
            {
                Id = JwtBearerDefaults.AuthenticationScheme,
                Type = ReferenceType.SecurityScheme
            }
        };

        c.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

        c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            jwtSecurityScheme,
            Array.Empty<string>()
        }
    });
    });

    builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
    builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DietManagementSystem.Application.Features.Auth.Login.LoginCommand).Assembly));
    builder.Services.AddValidatorsFromAssembly(typeof(LoginCommandValidator).Assembly);

    builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
    {
        options.Password.RequireDigit = true;
        options.Password.RequireUppercase = true;
    })
    .AddEntityFrameworkStores<DietManagementSystemDbContext>()
    .AddDefaultTokenProviders();

    builder.Services.AddDbContext<DietManagementSystemDbContext>((serviceProvider, options) =>
    {
        options.UseNpgsql(builder.Configuration.GetConnectionString("DietManagementSystemDb"));
    });

    var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>() ?? throw new ArgumentNullException("JwtSettings cannot be found in the appsettings!");
    var key = Encoding.ASCII.GetBytes(jwtSettings.Secret);

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtSettings.Audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            RoleClaimType = ClaimTypes.Role
        };
    });


    builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
    builder.Services.AddScoped<IAuthService, AuthService>();
    builder.Services.AddScoped<IDietPlanService, DietPlanService>();
    builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
    builder.Services.AddScoped<IClientService, ClientService>();
    builder.Services.AddScoped<ILoggingService, LoggingService>();


    var app = builder.Build();
    using (var scope = app.Services.CreateScope())
    {
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        try
        {
            await SeedAdminUserAsync(userManager);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Seeding roles/admin failed");
        }
    }

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    Log.Information("Diet Management System API started successfully");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Diet Management System API terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

static async Task SeedAdminUserAsync(UserManager<ApplicationUser> userManager)
{
    var adminEmail = "root@admin.com";
    var adminPassword = "Final!00";

    var existingUser = await userManager.FindByEmailAsync(adminEmail);
    if (existingUser is null)
    {
        var adminUser = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            UserType = UserType.Admin,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = "ZERODAY"
        };

        var result = await userManager.CreateAsync(adminUser, adminPassword);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, UserType.Admin.ToString());
            Console.WriteLine("Admin user created.");
        }
        else
        {
            foreach (var error in result.Errors)
                Console.WriteLine($"Error creating admin: {error.Description}");
        }
    }
    else
    {
        Console.WriteLine("Admin user already exists.");
    }
}
