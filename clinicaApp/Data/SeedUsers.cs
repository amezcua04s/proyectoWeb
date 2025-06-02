using Microsoft.AspNetCore.Identity;
using clinicaApp.Models;

namespace clinicaApp.Data
{
    public class SeedUsers
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {

            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ClinicaUser>>();

            string[] roles = new[] { "Administrador", "Doctor", "Paciente" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            // Crear usuario administrador
            string adminEmail = "admin@clinica.com";
            string password = "cl1nic.Admin";

            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var user = new ClinicaUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    Correo = adminEmail,
                    Nombre = "Admin",
                    Materno = "Principal",
                    Registro = DateTime.Now
                };

                var result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                    await userManager.AddToRoleAsync(user, "Administrador");
            }


        }



    }
}
