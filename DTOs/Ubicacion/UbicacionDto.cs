namespace KiasRentCarApi.DTOs.Ubicacion
{
	public class UbicacionDto
	{
		public int UbicacionId { get; set; }
		public string Nombre { get; set; } = string.Empty;
		public string? Direccion { get; set; }
		public bool Activa { get; set; }
	}

	public class UbicacionRequest
	{
		public string Nombre { get; set; } = string.Empty;
		public string? Direccion { get; set; }
		public bool Activa { get; set; } = true;
	}
}