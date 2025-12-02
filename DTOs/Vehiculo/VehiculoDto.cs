namespace KiasRentCarApi.DTOs.Vehiculo
{
	public class VehiculoDto
	{
		public int VehiculoId { get; set; }
		public string Modelo { get; set; } = string.Empty;
		public string? Descripcion { get; set; }
		public string Categoria { get; set; } = string.Empty;
		public int Asientos { get; set; }
		public string Transmision { get; set; } = string.Empty;
		public double PrecioPorDia { get; set; }
		public string? ImagenUrl { get; set; }
		public bool Disponible { get; set; }
		public DateTime FechaIngreso { get; set; }
	}
}