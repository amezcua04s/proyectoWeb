using Microsoft.EntityFrameworkCore;
using hospitalAPI.Data;


var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("http://localhost:5173") // agrega la URL de tu frontend
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

// agregamos el servicio de Entity Framework Core SQL server y la cadena de conexión
builder.Services.AddDbContext<ClinicaDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ClinicaDb")));
builder.Services.AddControllers();


var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCors(MyAllowSpecificOrigins);
app.UseAuthorization();

app.MapControllers();

app.Run();
