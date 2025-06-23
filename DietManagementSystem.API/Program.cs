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

// Configure Serilog
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
    builder.Services.AddValidatorsFromAssembly(typeof(DietManagementSystem.Application.Features.Auth.Login.LoginCommandValidator).Assembly);
    builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
    {
        options.Password.RequireDigit = true;
        options.Password.RequireUppercase = true;
    })
    .AddEntityFrameworkStores<DietManagementSystemDbContext>()
    .AddDefaultTokenProviders();

    builder.Services.AddDbContext<DietManagementSystemDbContext>((serviceProvider, options) =>
    {
        //options.Use...(builder.Configuration.GetConnectionString("DietManagementSystemDb");
    });

    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("AdminOnly", policy =>
            policy.RequireRole(UserType.Admin.ToString()));

        options.AddPolicy("DietitianOrAdmin", policy =>
            policy.RequireRole(UserType.Dietitian.ToString(), UserType.Admin.ToString()));
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
            ClockSkew = TimeSpan.Zero
        };
    });


    builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
    builder.Services.AddScoped<IAuthService, AuthService>();
    builder.Services.AddScoped<IDietPlanService, DietPlanService>();
    builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
    builder.Services.AddScoped<IClientService, ClientService>();
    builder.Services.AddScoped<ILoggingService, LoggingService>();

    var app = builder.Build();

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