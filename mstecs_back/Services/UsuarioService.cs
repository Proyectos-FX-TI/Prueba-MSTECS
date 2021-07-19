using System;
using System.Collections.Generic;
using System.Linq;
using mstecs_back.Entities;
using mstecs_back.Helpers;

namespace mstecs_back.Services
{
    public interface IUserService
    {
        UsuarioEntitie Authenticate(string correo, string password);
        IEnumerable<UsuarioEntitie> GetAll();
        UsuarioEntitie GetById(int id_usuario);
        UsuarioEntitie Create(UsuarioEntitie usuario, string password);
        RolEntitie GetRolById(int id_rol);
        public IEnumerable<RolEntitie> GetAllRol();
    }

    public class UsuarioService : IUserService
    {
        private DataContext _context;
        public UsuarioService(DataContext context)
        {
            _context = context;
        }

        public UsuarioEntitie Authenticate(string correo, string password)
        {
            if (string.IsNullOrEmpty(correo) || string.IsNullOrEmpty(password))
            {
                return null;
            }

            var usuario = _context.Usuarios.SingleOrDefault(x => x.correo == correo);

            if (usuario == null)
            {
                return null;
            }

            if (!Settings.VerifyPasswordHash(password, usuario.password_hash, usuario.password_salt))
            {
                return null;
            }

            return usuario;
        }

        public IEnumerable<UsuarioEntitie> GetAll()
        {
            return _context.Usuarios;
        }

        public UsuarioEntitie GetById(int id_usuario)
        {
            return _context.Usuarios.Find(id_usuario);
        }
        public UsuarioEntitie Create(UsuarioEntitie usuario, string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new AppException("Contraseña requerida");
            }

            if (_context.Usuarios.Any(x => x.correo == usuario.correo))
            {
                throw new AppException("Correo \"" + usuario.correo + "\" ya existe");
            }

            byte[] password_hash, password_salt;
            Settings.CreatePasswordHash(password, out password_hash, out password_salt);

            usuario.password_hash = password_hash;
            usuario.password_salt = password_salt;
            usuario.id_rol = 2;

            _context.Usuarios.Add(usuario);
            _context.SaveChanges();

            return usuario;
        }

        public RolEntitie GetRolById(int id_rol)
        {
            return _context.Roles.Find(id_rol);
        }

        public IEnumerable<RolEntitie> GetAllRol()
        {
            return _context.Roles;
        }
    }
}