using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KiasRentCarApi.Data;
using KiasRentCarApi.Models;

namespace KiasRentCarApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ReservacionesController : ControllerBase
	{
		private readonly AppDbContext _context;

		public ReservacionesController(AppDbContext context)
		{
			_context = context;
		}

		// GET: api/reservaciones
		[HttpGet]
		public async Task<IActionResult> GetReservaciones()
		{
			var reservaciones = await _context.Reservaciones
				.Include(r => r.Usuario)
				.Include(r => r.Vehiculo)
				.Include(r => r.UbicacionRecogida)
				.Include(r => r.UbicacionDevolucion)
				.OrderByDescending(r => r.FechaCreacion)
				.ToListAsync();

			return Ok(reservaciones.Select(r => MapReservacion(r)));
		}

		// GET: api/reservaciones/5
		[HttpGet("{id}")]
		public async Task<IActionResult> GetReservacion(int id)
		{
			var r = await _context.Reservaciones
				.Include(r => r.Usuario)
				.Include(r => r.Vehiculo)
				.Include(r => r.UbicacionRecogida)
				.Include(r => r.UbicacionDevolucion)
				.FirstOrDefaultAsync(r => r.ReservacionId == id);

			if (r == null) return NotFound();

			return Ok(MapReservacion(r));
		}

		// GET: api/reservaciones/usuario/5
		[HttpGet("usuario/{usuarioId}")]
		public async Task<IActionResult> GetReservacionesByUsuario(int usuarioId)
		{
			var reservaciones = await _context.Reservaciones
				.Include(r => r.Usuario)
				.Include(r => r.Vehiculo)
				.Include(r => r.UbicacionRecogida)
				.Include(r => r.UbicacionDevolucion)
				.Where(r => r.UsuarioId == usuarioId)
				.OrderByDescending(r => r.FechaCreacion)
				.ToListAsync();

			return Ok(reservaciones.Select(r => MapReservacion(r)));
		}

		// GET: api/reservaciones/codigo/{codigo}
		[HttpGet("codigo/{codigo}")]
		public async Task<IActionResult> GetReservacionByCodigo(string codigo)
		{
			var r = await _context.Reservaciones
				.Include(r => r.Usuario)
				.Include(r => r.Vehiculo)
				.Include(r => r.UbicacionRecogida)
				.Include(r => r.UbicacionDevolucion)
				.FirstOrDefaultAsync(r => r.CodigoReserva == codigo);

			if (r == null) return NotFound();

			return Ok(MapReservacion(r));
		}

		// POST: api/reservaciones
		[HttpPost]
		public async Task<IActionResult> CreateReservacion([FromBody] ReservacionRequest request)
		{
			var vehiculo = await _context.Vehiculos.FindAsync(request.VehiculoId);
			if (vehiculo == null || !vehiculo.Disponible)
				return BadRequest(new { message = "Vehículo no disponible" });

			// Calcular precios
			DateTime fechaRecogida = DateTime.Parse(request.FechaRecogida);
			DateTime fechaDevolucion = DateTime.Parse(request.FechaDevolucion);
			var dias = (fechaDevolucion - fechaRecogida).Days;
			if (dias < 1) dias = 1;

			var subtotal = vehiculo.PrecioPorDia * dias;
			var impuestos = subtotal * 0.18;
			var total = subtotal + impuestos;

			var codigoReserva = $"KR-{Guid.NewGuid().ToString().Substring(0, 6).ToUpper()}";

			var reservacion = new Reservacion
			{
				UsuarioId = int.Parse(request.UsuarioId),
				VehiculoId = request.VehiculoId,
				FechaRecogida = fechaRecogida,
				HoraRecogida = request.HoraRecogida,
				FechaDevolucion = fechaDevolucion,
				HoraDevolucion = request.HoraDevolucion,
				UbicacionRecogidaId = GetUbicacionIdByName(request.LugarRecogida).Result,
				UbicacionDevolucionId = GetUbicacionIdByName(request.LugarDevolucion).Result,
				Estado = "Confirmada",
				Subtotal = subtotal,
				Impuestos = impuestos,
				Total = total,
				CodigoReserva = codigoReserva,
				FechaCreacion = DateTime.UtcNow
			};

			_context.Reservaciones.Add(reservacion);
			await _context.SaveChangesAsync();

			// Cargar relaciones
			await _context.Entry(reservacion).Reference(r => r.Usuario).LoadAsync();
			await _context.Entry(reservacion).Reference(r => r.Vehiculo).LoadAsync();
			await _context.Entry(reservacion).Reference(r => r.UbicacionRecogida).LoadAsync();
			await _context.Entry(reservacion).Reference(r => r.UbicacionDevolucion).LoadAsync();

			return Ok(MapReservacion(reservacion));
		}

		// PUT: api/reservaciones/5
		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateReservacion(int id, [FromBody] ReservacionRequest request)
		{
			var reservacion = await _context.Reservaciones.FindAsync(id);
			if (reservacion == null) return NotFound();

			DateTime fechaRecogida = DateTime.Parse(request.FechaRecogida);
			DateTime fechaDevolucion = DateTime.Parse(request.FechaDevolucion);

			reservacion.FechaRecogida = fechaRecogida;
			reservacion.HoraRecogida = request.HoraRecogida;
			reservacion.FechaDevolucion = fechaDevolucion;
			reservacion.HoraDevolucion = request.HoraDevolucion;

			await _context.SaveChangesAsync();
			return NoContent();
		}

		// PUT: api/reservaciones/5/estado
		[HttpPut("{id}/estado")]
		public async Task<IActionResult> UpdateEstado(int id, [FromBody] EstadoRequest request)
		{
			var reservacion = await _context.Reservaciones.FindAsync(id);
			if (reservacion == null) return NotFound();

			reservacion.Estado = request.Estado;
			await _context.SaveChangesAsync();
			return NoContent();
		}

		// DELETE: api/reservaciones/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteReservacion(int id)
		{
			var reservacion = await _context.Reservaciones.FindAsync(id);
			if (reservacion == null) return NotFound();

			reservacion.Estado = "Cancelada";
			await _context.SaveChangesAsync();
			return NoContent();
		}

		private async Task<int> GetUbicacionIdByName(string nombre)
		{
			var ubicacion = await _context.Ubicaciones.FirstOrDefaultAsync(u => u.Nombre == nombre);
			return ubicacion?.UbicacionId ?? 1;
		}

		private static object MapReservacion(Reservacion r)
		{
			return new
			{
				reservacionId = r.ReservacionId,
				usuarioId = r.UsuarioId,
				vehiculoId = r.VehiculoId,
				fechaRecogida = r.FechaRecogida.ToString("yyyy-MM-dd"),
				horaRecogida = r.HoraRecogida,
				fechaDevolucion = r.FechaDevolucion.ToString("yyyy-MM-dd"),
				horaDevolucion = r.HoraDevolucion,
				ubicacionRecogidaId = r.UbicacionRecogidaId,
				ubicacionDevolucionId = r.UbicacionDevolucionId,
				estado = r.Estado,
				subtotal = r.Subtotal,
				impuestos = r.Impuestos,
				total = r.Total,
				codigoReserva = r.CodigoReserva,
				fechaCreacion = r.FechaCreacion.ToString("yyyy-MM-dd"),
				usuario = r.Usuario != null ? new
				{
					usuarioId = r.Usuario.UsuarioId,
					nombre = r.Usuario.Nombre,
					email = r.Usuario.Email
				} : null,
				vehiculo = r.Vehiculo != null ? new
				{
					vehiculoId = r.Vehiculo.VehiculoId,
					modelo = r.Vehiculo.Modelo,
					imagenUrl = r.Vehiculo.ImagenUrl,
					categoria = r.Vehiculo.Categoria,
					precioPorDia = r.Vehiculo.PrecioPorDia
				} : null,
				ubicacionRecogida = r.UbicacionRecogida != null ? new
				{
					ubicacionId = r.UbicacionRecogida.UbicacionId,
					nombre = r.UbicacionRecogida.Nombre
				} : null,
				ubicacionDevolucion = r.UbicacionDevolucion != null ? new
				{
					ubicacionId = r.UbicacionDevolucion.UbicacionId,
					nombre = r.UbicacionDevolucion.Nombre
				} : null
			};
		}
	}

	// Request DTOs
	public class ReservacionRequest
	{
		public int ReservaId { get; set; }
		public int VehiculoId { get; set; }
		public string UsuarioId { get; set; } = string.Empty;
		public string FechaRecogida { get; set; } = string.Empty;
		public string HoraRecogida { get; set; } = "10:00";
		public string FechaDevolucion { get; set; } = string.Empty;
		public string HoraDevolucion { get; set; } = "10:00";
		public string LugarRecogida { get; set; } = string.Empty;
		public string LugarDevolucion { get; set; } = string.Empty;
		public string Estado { get; set; } = "Pendiente";
		public double Subtotal { get; set; }
		public double Impuestos { get; set; }
		public double Total { get; set; }
		public string CodigoReserva { get; set; } = string.Empty;
	}

	public class EstadoRequest
	{
		public string Estado { get; set; } = string.Empty;
	}
}