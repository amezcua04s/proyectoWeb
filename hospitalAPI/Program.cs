using Microsoft.EntityFrameworkCore;
using hospitalAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// Configuraci�n de CORS
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("http://localhost:5173")
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

// Configuraci�n de la base de datos
builder.Services.AddDbContext<ClinicaDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ClinicaDb")));

// Configuraci�n de MVC
builder.Services.AddControllersWithViews();

// Configuraci�n de autenticaci�n (si es necesaria)
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "Cookies";
    options.DefaultChallengeScheme = "Cookies";
}).AddCookie("Cookies");

var app = builder.Build();

// Configuraci�n del pipeline HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseCors(MyAllowSpecificOrigins);
app.UseAuthentication();
app.UseAuthorization();

// Configuraci�n de rutas
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();