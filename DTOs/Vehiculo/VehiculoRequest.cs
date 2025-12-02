using System.ComponentModel.DataAnnotations;

namespace KiasRentCarApi.DTOs.Vehiculo
{
	public class VehiculoRequest
	{
		[Required]
		[MaxLength(100)]
		public string Modelo { get; set; } = string.Empty;

		[MaxLength(500)]
		public string? Descripcion { get; set; }

		[Required]
		public string Categoria { get; set; } = "SUV";

		public int Asientos { get; set; } = 5;

		[Required]
		public string Transmision { get; set; } = "Automatic";

		[Required]
		public double PrecioPorDia { get; set; }

		public string? ImagenUrl { get; set; }

		public bool Disponible { get; set; } = true;
	}
}