using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KiasRentCarApi.Data;
using KiasRentCarApi.Models;

namespace KiasRentCarApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UsuariosController : ControllerBase
	{
		private readonly AppDbContext _context;

		public UsuariosController(AppDbContext context)
		{
			_context = context;
		}

		// POST: api/usuarios/login
		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginRequest request)
		{
			var usuario = await _context.Usuarios
				.FirstOrDefaultAsync(u => u.Email == request.Email && u.Password == request.Password && u.Activo);

			if (usuario == null)
				return Unauthorized(new { message = "Credenciales inválidas" });

			return Ok(new
			{
				usuarioId = usuario.UsuarioId,
				nombre = usuario.Nombre,
				email = usuario.Email,
				password = usuario.Password,
				telefono = usuario.Telefono,
				rol = usuario.Rol,
				fechaRegistro = usuario.FechaRegistro.ToString("yyyy-MM-dd")
			});
		}

		// POST: api/usuarios/registro
		[HttpPost("registro")]
		public async Task<IActionResult> Registro([FromBody] RegistroRequest request)
		{
			if (await _context.Usuarios.AnyAsync(u => u.Email == request.Email))
				return BadRequest(new { message = "El email ya está registrado" });

			var usuario = new Usuario
			{
				Nombre = request.Nombre,
				Email = request.Email,
				Password = request.Password,
				Telefono = request.Telefono,
				Rol = request.Rol ?? "Cliente",
				FechaRegistro = DateTime.UtcNow,
				Activo = true
			};

			_context.Usuarios.Add(usuario);
			await _context.SaveChangesAsync();

			return Ok(new
			{
				usuarioId = usuario.UsuarioId,
				nombre = usuario.Nombre,
				email = usuario.Email,
				password = usuario.Password,
				telefono = usuario.Telefono,
				rol = usuario.Rol,
				fechaRegistro = usuario.FechaRegistro.ToString("yyyy-MM-dd")
			});
		}

		// GET: api/usuarios
		[HttpGet]
		public async Task<IActionResult> GetUsuarios()
		{
			var usuarios = await _context.Usuarios
				.Where(u => u.Activo)
				.Select(u => new
				{
					usuarioId = u.UsuarioId,
					nombre = u.Nombre,
					email = u.Email,
					password = (string?)null,
					telefono = u.Telefono,
					rol = u.Rol,
					fechaRegistro = u.FechaRegistro.ToString("yyyy-MM-dd")
				})
				.ToListAsync();

			return Ok(usuarios);
		}

		// GET: api/usuarios/5
		[HttpGet("{id}")]
		public async Task<IActionResult> GetUsuario(int id)
		{
			var u = await _context.Usuarios.FindAsync(id);
			if (u == null || !u.Activo) return NotFound();

			return Ok(new
			{
				usuarioId = u.UsuarioId,
				nombre = u.Nombre,
				email = u.Email,
				password = (string?)null,
				telefono = u.Telefono,
				rol = u.Rol,
				fechaRegistro = u.FechaRegistro.ToString("yyyy-MM-dd")
			});
		}

		// GET: api/usuarios/email/{email}
		[HttpGet("email/{email}")]
		public async Task<IActionResult> GetUsuarioByEmail(string email)
		{
			var u = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email && u.Activo);
			if (u == null) return NotFound();

			return Ok(new
			{
				usuarioId = u.UsuarioId,
				nombre = u.Nombre,
				email = u.Email,
				password = (string?)null,
				telefono = u.Telefono,
				rol = u.Rol,
				fechaRegistro = u.FechaRegistro.ToString("yyyy-MM-dd")
			});
		}

		// PUT: api/usuarios/5
		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateUsuario(int id, [FromBody] RegistroRequest request)
		{
			var usuario = await _context.Usuarios.FindAsync(id);
			if (usuario == null) return NotFound();

			usuario.Nombre = request.Nombre;
			usuario.Email = request.Email;
			if (!string.IsNullOrEmpty(request.Password))
				usuario.Password = request.Password;
			usuario.Telefono = request.Telefono;

			await _context.SaveChangesAsync();
			return NoContent();
		}

		// DELETE: api/usuarios/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteUsuario(int id)
		{
			var usuario = await _context.Usuarios.FindAsync(id);
			if (usuario == null) return NotFound();

			usuario.Activo = false;
			await _context.SaveChangesAsync();
			return NoContent();
		}
	}

	// Request DTOs
	public class LoginRequest
	{
		public string Email { get; set; } = string.Empty;
		public string Password { get; set; } = string.Empty;
	}

	public class RegistroRequest
	{
		public string Nombre { get; set; } = string.Empty;
		public string Email { get; set; } = string.Empty;
		public string Password { get; set; } = string.Empty;
		public string? Telefono { get; set; }
		public string? Rol { get; set; }
	}
}