using clinicaApp.Data;
using clinicaApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
// agregamos el servicio de Entity Framework Core SQL server y la cadena de conexión
builder.Services.AddDbContext<ClinicaAppDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("clinicaAppDB")));

// agregamos el servicio de Identity
builder.Services.AddIdentity<ClinicaUser, IdentityRole>()
    .AddEntityFrameworkStores<ClinicaAppDbContext>()
    .AddDefaultTokenProviders();

//si quieren personalizar las reglas de validación de copntraseñas comenten arriba
//y descomenten las siguientes lineas
//builder.Services.AddIdentity<FarmaciaUser, IdentityRole>(options =>
//{
//    // Personalización de contraseñas
//    options.Password.RequireDigit = false;             // No requiere número
//    options.Password.RequiredLength = 8;               // Longitud mínima: 8
//    options.Password.RequireNonAlphanumeric = false;   // No requiere símbolo (!, @, etc.)
//    options.Password.RequireUppercase = false;         // No requiere mayúsculas
//    options.Password.RequireLowercase = false;         // No requiere minúsculas
//})
//.AddEntityFrameworkStores<FarmaciaDbContext>()
//.AddDefaultTokenProviders();


//Logica de permisos de acceso a vistas a base de roles
builder.Services.AddControllersWithViews();

var app = builder.Build();
//llenar datos en la base de datos
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await SeedUsers.Initialize(services);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();