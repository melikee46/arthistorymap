using Microsoft.EntityFrameworkCore;
using ArtHistoryMap.Api.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ArtHistoryMap.Api.Services.ArtMovementService>();

// .NET 10'un yerleşik OpenAPI desteği (Swashbuckle yerine)
builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    // Seed: sadece Development'ta. DbContext scoped olduğu için
    // elle bir scope açıyoruz (uygulama henüz istek almıyor).
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await DbSeeder.SeedAsync(context);
}

app.UseCors("AllowFrontend");

app.UseAuthorization();

app.MapControllers();

app.Run();