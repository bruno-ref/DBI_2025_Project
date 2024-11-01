using Microsoft.EntityFrameworkCore;
using MongoVsRelational.Data;

var builder = WebApplication.CreateBuilder(args);

// SQLite-Datenbank und Entity Framework Core konfigurieren
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=dishes.db"));

// Swagger konfigurieren
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

// Controller-Services hinzufügen
builder.Services.AddControllers();

var app = builder.Build();

// Migrationen anwenden
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
}

// Swagger und Controller-Endpunkte aktivieren
app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
