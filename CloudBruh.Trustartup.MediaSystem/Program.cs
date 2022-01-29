using System.Text.Json.Serialization;
using CloudBruh.Trustartup.MediaSystem.Models;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

var storagePath = builder.Configuration.GetValue<string>("Storage:Path");
Directory.CreateDirectory(storagePath);

string connectionString = builder.Configuration.GetConnectionString("Database");

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddDbContext<MediaContext>(options => options.UseNpgsql(connectionString));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

WebApplication app = builder.Build();

// Apply migrations on development environment.
if (app.Environment.IsDevelopment())
{
    using IServiceScope scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<MediaContext>();
    await db.Database.MigrateAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();