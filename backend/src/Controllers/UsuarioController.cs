using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TrabalhoCapacitacao.Data;
using TrabalhoCapacitacao.DTOs.Usuario;
using TrabalhoCapacitacao.Models;

namespace TrabalhoCapacitacao.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IPasswordHasher<Usuario> _passwordHasher;
        private readonly IConfiguration _configuration;

        public UsuariosController(
            AppDbContext context,
            IPasswordHasher<Usuario> passwordHasher,
            IConfiguration configuration)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _configuration = configuration;
        }

        // POST: api/Usuarios/registrar
        [HttpPost("registrar")]
        [AllowAnonymous]
        public async Task<ActionResult<UsuarioResponseDto>> RegistrarUsuario([FromBody] UsuarioCreateDto usuarioCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (await _context.Usuarios.AnyAsync(u => u.Email == usuarioCreateDto.Email))
            {
                ModelState.AddModelError("Email", "Este e-mail já está em uso.");
                return BadRequest(ModelState);
            }

            var usuario = new Usuario
            {
                Id = Guid.NewGuid().ToString(),
                Nome = usuarioCreateDto.Nome,
                Email = usuarioCreateDto.Email,
                Perfil = usuarioCreateDto.Perfil,
                Telefone = usuarioCreateDto.Telefone,
                BairroResidencia = usuarioCreateDto.BairroResidencia
            };

            usuario.SenhaHash = _passwordHasher.HashPassword(usuario, usuarioCreateDto.Senha);

            _context.Usuarios.Add(usuario);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Erro ao registrar utilizador: {ex.InnerException?.Message ?? ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Erro ao registrar o utilizador no banco de dados." });
            }

            var usuarioResponseDto = new UsuarioResponseDto
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Perfil = usuario.Perfil,
                Telefone = usuario.Telefone,
                BairroResidencia = usuario.BairroResidencia 
            };

            return CreatedAtAction(nameof(GetUsuario), new { id = usuario.Id }, usuarioResponseDto);
        }

        // POST: api/Usuarios/login
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == loginDto.Email);

            if (usuario == null)
            {
                return Unauthorized(new { message = "Credenciais inválidas." });
            }

            var resultadoPassword = _passwordHasher.VerifyHashedPassword(usuario, usuario.SenhaHash, loginDto.Senha);
            if (resultadoPassword == PasswordVerificationResult.Failed)
            {
                return Unauthorized(new { message = "Credenciais inválidas." });
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Name, usuario.Nome),
                new Claim(ClaimTypes.Role, usuario.Perfil),
                new Claim("Bairro", usuario.BairroResidencia ?? "")
            };

            var jwtSecret = _configuration["JWT:Secret"];
            if (string.IsNullOrEmpty(jwtSecret))
            {
                Console.WriteLine("Erro crítico: Chave secreta JWT não configurada.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Erro de configuração do servidor." });
            }

            var jwtKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));
            var credenciais = new SigningCredentials(jwtKey, SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                Audience = _configuration["JWT:ValidAudience"],
                Issuer = _configuration["JWT:ValidIssuer"],
                SigningCredentials = credenciais
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new
            {
                token = tokenHandler.WriteToken(token),
                expiration = token.ValidTo,
                usuario = new UsuarioResponseDto
                {
                    Id = usuario.Id,
                    Nome = usuario.Nome,
                    Email = usuario.Email,
                    Perfil = usuario.Perfil,
                    Telefone = usuario.Telefone,
                    BairroResidencia = usuario.BairroResidencia
                }
            });
        }

        // GET: api/Usuarios/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioResponseDto>> GetUsuario(string id)
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var isAdmin = User.IsInRole("admin");

            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                return NotFound(new { message = $"Utilizador com ID {id} não encontrado." });
            }

            var usuarioDto = new UsuarioResponseDto
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Perfil = usuario.Perfil,
                Telefone = usuario.Telefone,
                BairroResidencia = usuario.BairroResidencia
            };
            return Ok(usuarioDto);
        }

        // GET: api/Usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioResponseDto>>> GetUsuarios()
        {
            var usuarios = await _context.Usuarios
                .Select(u => new UsuarioResponseDto
                {
                    Id = u.Id,
                    Nome = u.Nome,
                    Email = u.Email,
                    Perfil = u.Perfil,
                    Telefone = u.Telefone,
                    BairroResidencia = u.BairroResidencia
                })
                .ToListAsync();
            return Ok(usuarios);
        }

        // PUT: api/Usuarios/{id}
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutUsuario(string id, [FromBody] UsuarioUpdateDto usuarioUpdateDto)
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var isAdmin = User.IsInRole("admin");

            if (id != currentUserId && !isAdmin)
            {
                return Forbid();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var usuarioExistente = await _context.Usuarios.FindAsync(id);

            if (usuarioExistente == null)
            {
                return NotFound(new { message = $"Utilizador com ID {id} não encontrado para atualização." });
            }

            usuarioExistente.Nome = usuarioUpdateDto.Nome;
            usuarioExistente.Telefone = usuarioUpdateDto.Telefone;
            usuarioExistente.BairroResidencia = usuarioUpdateDto.BairroResidencia;


            if (isAdmin)
            {
                usuarioExistente.Perfil = usuarioUpdateDto.Perfil;
            }

            _context.Entry(usuarioExistente).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!await UsuarioExistsAsync(id))
                {
                    return NotFound(new { message = $"Utilizador com ID {id} não encontrado (concorrência)." });
                }
                else
                {
                    Console.WriteLine($"Erro de concorrência: {ex}");
                    return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Erro de concorrência." });
                }
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Erro ao atualizar: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Erro ao atualizar o utilizador." });
            }

            return NoContent();
        }

        // DELETE: api/Usuarios/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteUsuario(string id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound(new { message = $"Utilizador com ID {id} não encontrado." });
            }

            var inscricoesAssociadas = await _context.Inscricoes.AnyAsync(i => i.UsuarioId == id);
            if (inscricoesAssociadas)
            {
                return BadRequest(new { message = "Não é possível excluir usuário com inscrições ativas." });
            }

            _context.Usuarios.Remove(usuario);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Erro ao excluir: {ex}");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Erro ao excluir o utilizador." });
            }

            return NoContent();
        }

        private async Task<bool> UsuarioExistsAsync(string id)
        {
            return await _context.Usuarios.AnyAsync(e => e.Id == id);
        }
    }
}