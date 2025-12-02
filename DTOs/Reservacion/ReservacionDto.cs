namespace KiasRentCarApi.DTOs.Reservacion
{
	public class ReservacionDto
	{
		public int ReservacionId { get; set; }
		public int UsuarioId { get; set; }
		public int VehiculoId { get; set; }
		public DateTime FechaRecogida { get; set; }
		public string HoraRecogida { get; set; } = string.Empty;
		public DateTime FechaDevolucion { get; set; }
		public string HoraDevolucion { get; set; } = string.Empty;
		public int UbicacionRecogidaId { get; set; }
		public int UbicacionDevolucionId { get; set; }
		public string Estado { get; set; } = string.Empty;
		public double Subtotal { get; set; }
		public double Impuestos { get; set; }
		public double Total { get; set; }
		public string CodigoReserva { get; set; } = string.Empty;
		public DateTime FechaCreacion { get; set; }

		// Objetos relacionados
		public UsuarioReservacionDto? Usuario { get; set; }
		public VehiculoReservacionDto? Vehiculo { get; set; }
		public UbicacionReservacionDto? UbicacionRecogida { get; set; }
		public UbicacionReservacionDto? UbicacionDevolucion { get; set; }
	}

	public class UsuarioReservacionDto
	{
		public int UsuarioId { get; set; }
		public string Nombre { get; set; } = string.Empty;
		public string Email { get; set; } = string.Empty;
	}

	public class VehiculoReservacionDto
	{
		public int VehiculoId { get; set; }
		public string Modelo { get; set; } = string.Empty;
		public string? ImagenUrl { get; set; }
		public string Categoria { get; set; } = string.Empty;
		public double PrecioPorDia { get; set; }
	}

	public class UbicacionReservacionDto
	{
		public int UbicacionId { get; set; }
		public string Nombre { get; set; } = string.Empty;
	}
}