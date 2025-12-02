using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KiasRentCarApi.Data;
using KiasRentCarApi.Models;

namespace KiasRentCarApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class VehiculosController : ControllerBase
	{
		private readonly AppDbContext _context;
		private readonly IWebHostEnvironment _environment;

		public VehiculosController(AppDbContext context, IWebHostEnvironment environment)
		{
			_context = context;
			_environment = environment;
		}

		// GET: api/vehiculos (solo disponibles)
		[HttpGet]
		public async Task<IActionResult> GetVehiculosDisponibles()
		{
			var vehiculos = await _context.Vehiculos
				.Where(v => v.Disponible)
				.Select(v => MapVehiculo(v))
				.ToListAsync();

			return Ok(vehiculos);
		}

		// GET: api/vehiculos/all
		[HttpGet("all")]
		public async Task<IActionResult> GetAllVehiculos()
		{
			var vehiculos = await _context.Vehiculos
				.Select(v => MapVehiculo(v))
				.ToListAsync();

			return Ok(vehiculos);
		}

		// GET: api/vehiculos/5
		[HttpGet("{id}")]
		public async Task<IActionResult> GetVehiculo(int id)
		{
			var v = await _context.Vehiculos.FindAsync(id);
			if (v == null) return NotFound();

			return Ok(MapVehiculo(v));
		}

		// GET: api/vehiculos/categoria/{categoria}
		[HttpGet("categoria/{categoria}")]
		public async Task<IActionResult> GetVehiculosByCategoria(string categoria)
		{
			var vehiculos = await _context.Vehiculos
				.Where(v => v.Categoria.ToUpper() == categoria.ToUpper() && v.Disponible)
				.Select(v => MapVehiculo(v))
				.ToListAsync();

			return Ok(vehiculos);
		}

		// GET: api/vehiculos/buscar?query=
		[HttpGet("buscar")]
		public async Task<IActionResult> SearchVehiculos([FromQuery] string query)
		{
			if (string.IsNullOrWhiteSpace(query))
				return Ok(new List<object>());

			var vehiculos = await _context.Vehiculos
				.Where(v => v.Modelo.ToLower().Contains(query.ToLower()) && v.Disponible)
				.Select(v => MapVehiculo(v))
				.ToListAsync();

			return Ok(vehiculos);
		}

		// POST: api/vehiculos
		[HttpPost]
		public async Task<IActionResult> CreateVehiculo([FromBody] VehiculoRequest request)
		{
			var vehiculo = new Vehiculo
			{
				Modelo = request.Modelo,
				Descripcion = request.Descripcion,
				Categoria = request.Categoria,
				Asientos = request.Asientos,
				Transmision = request.Transmision,
				PrecioPorDia = request.PrecioPorDia,
				ImagenUrl = request.ImagenUrl,
				Disponible = request.Disponible,
				FechaIngreso = DateTime.UtcNow
			};

			_context.Vehiculos.Add(vehiculo);
			await _context.SaveChangesAsync();

			return Ok(MapVehiculo(vehiculo));
		}

		// PUT: api/vehiculos/5
		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateVehiculo(int id, [FromBody] VehiculoRequest request)
		{
			var vehiculo = await _context.Vehiculos.FindAsync(id);
			if (vehiculo == null) return NotFound();

			vehiculo.Modelo = request.Modelo;
			vehiculo.Descripcion = request.Descripcion;
			vehiculo.Categoria = request.Categoria;
			vehiculo.Asientos = request.Asientos;
			vehiculo.Transmision = request.Transmision;
			vehiculo.PrecioPorDia = request.PrecioPorDia;
			vehiculo.ImagenUrl = request.ImagenUrl;
			vehiculo.Disponible = request.Disponible;

			await _context.SaveChangesAsync();
			return NoContent();
		}

		// DELETE: api/vehiculos/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteVehiculo(int id)
		{
			var vehiculo = await _context.Vehiculos.FindAsync(id);
			if (vehiculo == null) return NotFound();

			_context.Vehiculos.Remove(vehiculo);
			await _context.SaveChangesAsync();
			return NoContent();
		}

		// POST: api/vehiculos/upload-image
		[HttpPost("upload-image")]
		public async Task<IActionResult> UploadImage(IFormFile file)
		{
			if (file == null || file.Length == 0)
				return BadRequest("No se ha proporcionado ningún archivo");

			var uploadsFolder = Path.Combine(_environment.WebRootPath ?? "wwwroot", "images", "vehiculos");
			if (!Directory.Exists(uploadsFolder))
				Directory.CreateDirectory(uploadsFolder);

			var uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
			var filePath = Path.Combine(uploadsFolder, uniqueFileName);

			using (var stream = new FileStream(filePath, FileMode.Create))
			{
				await file.CopyToAsync(stream);
			}

			return Ok(new { url = $"/images/vehiculos/{uniqueFileName}" });
		}

		private static object MapVehiculo(Vehiculo v)
		{
			return new
			{
				vehiculoId = v.VehiculoId,
				modelo = v.Modelo,
				descripcion = v.Descripcion,
				categoria = v.Categoria,
				asientos = v.Asientos,
				transmision = v.Transmision,
				precioPorDia = v.PrecioPorDia,
				imagenUrl = v.ImagenUrl,
				disponible = v.Disponible,
				fechaIngreso = v.FechaIngreso.ToString("yyyy-MM-dd")
			};
		}
	}

	// Request DTO
	public class VehiculoRequest
	{
		public string Modelo { get; set; } = string.Empty;
		public string? Descripcion { get; set; }
		public string Categoria { get; set; } = "SUV";
		public int Asientos { get; set; } = 5;
		public string Transmision { get; set; } = "AUTOMATIC";
		public double PrecioPorDia { get; set; }
		public string? ImagenUrl { get; set; }
		public bool Disponible { get; set; } = true;
	}
}