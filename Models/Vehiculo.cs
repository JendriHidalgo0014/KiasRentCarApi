using System.ComponentModel.DataAnnotations;

namespace KiasRentCarApi.Models
{
	public class Vehiculo
	{
		[Key]
		public int VehiculoId { get; set; }

		[Required]
		[MaxLength(100)]
		public string Modelo { get; set; } = string.Empty;

		[MaxLength(500)]
		public string? Descripcion { get; set; }

		[Required]
		[MaxLength(50)]
		public string Categoria { get; set; } = "SUV";

		public int Asientos { get; set; } = 5;

		[Required]
		[MaxLength(50)]
		public string Transmision { get; set; } = "AUTOMATIC";

		[Required]
		public double PrecioPorDia { get; set; }

		[MaxLength(500)]
		public string? ImagenUrl { get; set; }

		public bool Disponible { get; set; } = true;

		public DateTime FechaIngreso { get; set; } = DateTime.UtcNow;

		// Navegación
		public ICollection<Reservacion>? Reservaciones { get; set; }
	}
}