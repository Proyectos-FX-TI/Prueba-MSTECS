using Microsoft.EntityFrameworkCore;
using mstecs_back.Helpers;
using mstecs_back.Entities;
using Bogus;
namespace mstecs_back.Seeders
{
    public static class UsuarioSeeder
    {
        private enum Gender
        {
            Male,
            Female
        }
        public static void Seed(ModelBuilder modelBuilder)
        {
            byte[] password_hash, password_salt;
            Settings.CreatePasswordHash("password", out password_hash, out password_salt);
            int id_usuario = 1;

            var usuarios = new Faker<UsuarioEntitie>()
                .RuleFor(c => c.id_usuario, k => id_usuario++)
                .RuleFor(c => c.nombre, (k, a) => (a.id_usuario == 1 ? "Cristhian Carrasco Herrerías" : (k.Name.FirstName() + " " + k.Name.LastName())))
                .RuleFor(c => c.correo, (k, a) => (a.id_usuario == 1 ? "crithianfx@gmail.com" : k.Internet.Email(a.nombre)))
                .RuleFor(c => c.password_hash, k => password_hash)
                .RuleFor(c => c.password_salt, k => password_salt)
                .RuleFor(c => c.id_rol, (k, a) => (a.id_usuario == 1 ? 1 : 2));

            //modelBuilder.Entity<UsuarioEntitie>()
            //    .HasData(
            //       new UsuarioEntitie { 
            //           id_usuario = id_usuario, 
            //           nombre = "Cristhian Carrasco Herrerías", 
            //           correo = "crithianfx@gmail.com",
            //           password_hash = password_hash,
            //           password_salt = password_salt,
            //           id_rol = 1
            //       }
            //);

            modelBuilder.Entity<UsuarioEntitie>()
               .HasData(usuarios.Generate(100));


        }
    }
}
