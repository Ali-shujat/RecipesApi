using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using RecipesApi.Data;

var builder = WebApplication.CreateBuilder(args);

// DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// AutoMapper
builder.Services.AddAutoMapper(typeof(Program).Assembly);

// Controllers + FluentValidation
builder.Services.AddControllers()
    .AddNewtonsoftJson(); // optional

builder.Services.AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters();
// Register validators via assembly scanning (FluentValidation will pick them up)

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Auto apply migrations on startup (development)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
