using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KiasRentCarApi.Data;
using KiasRentCarApi.Models;

namespace KiasRentCarApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class MensajesController : ControllerBase
	{
		private readonly AppDbContext _context;

		public MensajesController(AppDbContext context)
		{
			_context = context;
		}

		// GET: api/mensajes
		[HttpGet]
		public async Task<IActionResult> GetMensajes()
		{
			var mensajes = await _context.Mensajes
				.Include(m => m.Usuario)
				.OrderByDescending(m => m.FechaCreacion)
				.Select(m => MapMensaje(m))
				.ToListAsync();

			return Ok(mensajes);
		}

		// GET: api/mensajes/5
		[HttpGet("{id}")]
		public async Task<IActionResult> GetMensaje(int id)
		{
			var m = await _context.Mensajes
				.Include(m => m.Usuario)
				.FirstOrDefaultAsync(m => m.MensajeId == id);

			if (m == null) return NotFound();

			return Ok(MapMensaje(m));
		}

		// GET: api/mensajes/usuario/5
		[HttpGet("usuario/{usuarioId}")]
		public async Task<IActionResult> GetMensajesByUsuario(int usuarioId)
		{
			var mensajes = await _context.Mensajes
				.Include(m => m.Usuario)
				.Where(m => m.UsuarioId == usuarioId)
				.OrderByDescending(m => m.FechaCreacion)
				.Select(m => MapMensaje(m))
				.ToListAsync();

			return Ok(mensajes);
		}

		// POST: api/mensajes
		[HttpPost]
		public async Task<IActionResult> CreateMensaje([FromBody] MensajeRequest request)
		{
			var mensaje = new Mensaje
			{
				UsuarioId = request.UsuarioId,
				Asunto = request.Asunto,
				Contenido = request.Contenido,
				FechaCreacion = DateTime.UtcNow,
				Leido = false
			};

			_context.Mensajes.Add(mensaje);
			await _context.SaveChangesAsync();

			await _context.Entry(mensaje).Reference(m => m.Usuario).LoadAsync();

			return Ok(MapMensaje(mensaje));
		}

		// PUT: api/mensajes/5/responder
		[HttpPut("{id}/responder")]
		public async Task<IActionResult> ResponderMensaje(int id, [FromBody] RespuestaRequest request)
		{
			var mensaje = await _context.Mensajes.FindAsync(id);
			if (mensaje == null) return NotFound();

			mensaje.Respuesta = request.Respuesta;
			mensaje.FechaRespuesta = DateTime.UtcNow;
			mensaje.Leido = true;

			await _context.SaveChangesAsync();
			return NoContent();
		}

		// PUT: api/mensajes/5/leido
		[HttpPut("{id}/leido")]
		public async Task<IActionResult> MarcarLeido(int id)
		{
			var mensaje = await _context.Mensajes.FindAsync(id);
			if (mensaje == null) return NotFound();

			mensaje.Leido = true;
			await _context.SaveChangesAsync();
			return NoContent();
		}

		// DELETE: api/mensajes/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteMensaje(int id)
		{
			var mensaje = await _context.Mensajes.FindAsync(id);
			if (mensaje == null) return NotFound();

			_context.Mensajes.Remove(mensaje);
			await _context.SaveChangesAsync();
			return NoContent();
		}

		private static object MapMensaje(Mensaje m)
		{
			return new
			{
				mensajeId = m.MensajeId,
				usuarioId = m.UsuarioId,
				nombreUsuario = m.Usuario?.Nombre ?? "Usuario",
				asunto = m.Asunto,
				contenido = m.Contenido,
				respuesta = m.Respuesta,
				fechaCreacion = m.FechaCreacion.ToString("yyyy-MM-dd"),
				leido = m.Leido
			};
		}
	}

	// Request DTOs
	public class MensajeRequest
	{
		public int UsuarioId { get; set; }
		public string Asunto { get; set; } = string.Empty;
		public string Contenido { get; set; } = string.Empty;
	}

	public class RespuestaRequest
	{
		public string Respuesta { get; set; } = string.Empty;
	}
}