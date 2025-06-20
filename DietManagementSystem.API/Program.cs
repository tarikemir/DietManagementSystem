using DietManagementSystem.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DietManagementSystemDbContext>((serviceProvider, options) =>
{
    //options.Use...(builder.Configuration.GetConnectionString("DietManagementSystemDb");

});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();