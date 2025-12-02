using System.ComponentModel.DataAnnotations;

namespace KiasRentCarApi.DTOs.Reservacion
{
	public class ReservacionRequest
	{
		[Required]
		public int UsuarioId { get; set; }

		[Required]
		public int VehiculoId { get; set; }

		[Required]
		public DateTime FechaRecogida { get; set; }

		[Required]
		public string HoraRecogida { get; set; } = "10:00";

		[Required]
		public DateTime FechaDevolucion { get; set; }

		[Required]
		public string HoraDevolucion { get; set; } = "10:00";

		[Required]
		public int UbicacionRecogidaId { get; set; }

		[Required]
		public int UbicacionDevolucionId { get; set; }
	}
}