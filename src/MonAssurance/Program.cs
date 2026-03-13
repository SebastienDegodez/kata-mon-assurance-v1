using Microsoft.EntityFrameworkCore;
using MonAssurance.Data;
using MonAssurance.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddDbContext<AssuranceDbContext>(opt =>
    opt.UseInMemoryDatabase("MonAssuranceDb"));

builder.Services.AddScoped<EligibiliteService>();
builder.Services.AddScoped<GestionRisqueService>();
builder.Services.AddScoped<UsageVehiculeService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
