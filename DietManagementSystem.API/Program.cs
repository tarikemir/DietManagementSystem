using DietManagementSystem.Persistence;
using DietManagementSystem.Application.Common;
using DietManagementSystem.Infrastructure.Repositories;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DietManagementSystem.Application.Features.Clients.Create.CreateClientCommand).Assembly));
builder.Services.AddValidatorsFromAssembly(typeof(DietManagementSystem.Application.Features.Clients.Create.CreateClientCommandValidator).Assembly);

builder.Services.AddDbContext<DietManagementSystemDbContext>((serviceProvider, options) =>
{
    //options.Use...(builder.Configuration.GetConnectionString("DietManagementSystemDb");
});

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();