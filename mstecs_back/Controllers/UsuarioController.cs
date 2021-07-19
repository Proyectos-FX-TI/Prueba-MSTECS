using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using System.IdentityModel.Tokens.Jwt;
using mstecs_back.Helpers;
using Microsoft.Extensions.Options;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using mstecs_back.Services;
using mstecs_back.Models.Usuarios;
using System.Linq;
using System.Data;

using System.Diagnostics;

using System.Web;

using System.Reflection;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Configuration;
using System.Runtime;
using System.Threading.Tasks;
using System.Globalization;
using System.Net.Http;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;

namespace mstecs_back.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UsuarioController : ControllerBase
    {
        private IUserService _userService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public UsuarioController(
            IUserService userService,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _userService = userService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        [AllowAnonymous]
        [HttpPost("autentificar")]
        public IActionResult Authenticate([FromBody] AutentificarModel model)
        {
            var usuario = _userService.Authenticate(model.correo, model.password);

            if (usuario == null)
            {
                return BadRequest(new { message = "Correo o contraseña incorrecta" });
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, usuario.id_usuario.ToString()),
                    new Claim(ClaimTypes.Email, usuario.correo),
                    new Claim(ClaimTypes.Role, usuario.id_rol.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new
            {
                id_usuario = usuario.id_usuario,
                correo = usuario.correo,
                nombre = usuario.nombre,
                Token = tokenString
            });
        }

        [AllowAnonymous]
        [HttpPost("registrar")]
        public IActionResult Register([FromBody]RegistrarModel model)
        {
            var usuario = _mapper.Map<Entities.UsuarioEntitie>(model);

            try
            {
                _userService.Create(usuario, model.password);
                return Ok();
            }
            catch (AppException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            var model = _mapper.Map<IEnumerable<UsuarioModel>>(users)
                .Select(c => { c.rol = _userService.GetRolById(c.id_rol).rol; return c; });
            return Ok(model);
        }

        [HttpGet("exportar")]
        public async Task<IActionResult> Exportar()
        {
            try
            {
                var users = _userService.GetAll();
                string filename = string.Empty;
                string filepath = string.Empty;
                IEnumerable<UsuarioModel> model = _mapper.Map<IEnumerable<UsuarioModel>>(users)
                .Select(c => { c.rol = _userService.GetRolById(c.id_rol).rol; return c; });
                Exportar(model, ref filename, ref filepath);
                if (!System.IO.File.Exists(filepath))
                {
                    return NotFound();
                }
                var memory = new MemoryStream();
                await using (var stream = new FileStream(filepath, FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;
                return File(memory, GetContentType(filepath), filename);
            }
            catch (AppException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        private void Exportar(IEnumerable<UsuarioModel> usuarios, ref string filename, ref string filepath)
        {
            if (usuarios.Count() > 0)
            {
                int pdfRowIndex = 1;
                filename = "Usuarios_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf";
                filepath = Settings.MapPath(filename);
                Document document = new Document(PageSize.A4, 5f, 5f, 10f, 10f);
                FileStream fs = new FileStream(filepath, FileMode.Create);
                PdfWriter writer = PdfWriter.GetInstance(document, fs);
                document.Open();

                Font font1 = FontFactory.GetFont(FontFactory.COURIER_BOLD, 10);
                Font font2 = FontFactory.GetFont(FontFactory.COURIER, 8);

                float[] columnDefinitionSize = { 5F, 5F, 2F };
                PdfPTable table;
                PdfPCell cell;

                table = new PdfPTable(columnDefinitionSize)
                {
                    WidthPercentage = 100
                };

                cell = new PdfPCell
                {
                    BackgroundColor = new BaseColor(0xC0, 0xC0, 0xC0)
                };

                table.AddCell(new Phrase("Nombre", font1));
                table.AddCell(new Phrase("Correo", font1));
                table.AddCell(new Phrase("Rol", font1));
                table.HeaderRows = 1;

                foreach (var usuario in usuarios)
                {
                    table.AddCell(new Phrase(usuario.nombre, font2));
                    table.AddCell(new Phrase(usuario.correo, font2));
                    table.AddCell(new Phrase(usuario.rol, font2));

                    pdfRowIndex++;
                }

                document.Add(table);
                document.Close();
                document.CloseDocument();
                document.Dispose();
                writer.Close();
                writer.Dispose();
                fs.Close();
                fs.Dispose();             
            }
        }

        private string GetContentType(string path)
        {
            var provider = new FileExtensionContentTypeProvider();
            string contentType;

            if (!provider.TryGetContentType(path, out contentType))
            {
                contentType = "application/octet-stream";
            }

            return contentType;
        }
    }
}
