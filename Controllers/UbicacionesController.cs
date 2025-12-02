using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KiasRentCarApi.Data;
using KiasRentCarApi.Models;

namespace KiasRentCarApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UbicacionesController : ControllerBase
	{
		private readonly AppDbContext _context;

		public UbicacionesController(AppDbContext context)
		{
			_context = context;
		}

		// GET: api/ubicaciones
		[HttpGet]
		public async Task<IActionResult> GetUbicaciones()
		{
			var ubicaciones = await _context.Ubicaciones
				.Where(u => u.Activa)
				.Select(u => new
				{
					ubicacionId = u.UbicacionId,
					nombre = u.Nombre,
					direccion = u.Direccion,
					activa = u.Activa
				})
				.ToListAsync();

			return Ok(ubicaciones);
		}

		// GET: api/ubicaciones/5
		[HttpGet("{id}")]
		public async Task<IActionResult> GetUbicacion(int id)
		{
			var u = await _context.Ubicaciones.FindAsync(id);
			if (u == null) return NotFound();

			return Ok(new
			{
				ubicacionId = u.UbicacionId,
				nombre = u.Nombre,
				direccion = u.Direccion,
				activa = u.Activa
			});
		}

		// POST: api/ubicaciones
		[HttpPost]
		public async Task<IActionResult> CreateUbicacion([FromBody] UbicacionRequest request)
		{
			var ubicacion = new Ubicacion
			{
				Nombre = request.Nombre,
				Direccion = request.Direccion,
				Activa = true
			};

			_context.Ubicaciones.Add(ubicacion);
			await _context.SaveChangesAsync();

			return Ok(new
			{
				ubicacionId = ubicacion.UbicacionId,
				nombre = ubicacion.Nombre,
				direccion = ubicacion.Direccion,
				activa = ubicacion.Activa
			});
		}

		// PUT: api/ubicaciones/5
		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateUbicacion(int id, [FromBody] UbicacionRequest request)
		{
			var ubicacion = await _context.Ubicaciones.FindAsync(id);
			if (ubicacion == null) return NotFound();

			ubicacion.Nombre = request.Nombre;
			ubicacion.Direccion = request.Direccion;

			await _context.SaveChangesAsync();
			return NoContent();
		}

		// DELETE: api/ubicaciones/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteUbicacion(int id)
		{
			var ubicacion = await _context.Ubicaciones.FindAsync(id);
			if (ubicacion == null) return NotFound();

			ubicacion.Activa = false;
			await _context.SaveChangesAsync();
			return NoContent();
		}
	}

	// Request DTO
	public class UbicacionRequest
	{
		public string Nombre { get; set; } = string.Empty;
		public string? Direccion { get; set; }
	}
}