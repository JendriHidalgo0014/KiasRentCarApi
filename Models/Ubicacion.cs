using System.ComponentModel.DataAnnotations;

namespace KiasRentCarApi.Models
{
	public class Ubicacion
	{
		[Key]
		public int UbicacionId { get; set; }

		[Required]
		[MaxLength(200)]
		public string Nombre { get; set; } = string.Empty;

		[MaxLength(300)]
		public string? Direccion { get; set; }

		public bool Activa { get; set; } = true;

		// Navegación - AGREGAR ESTAS LÍNEAS
		public ICollection<Reservacion>? ReservacionesRecogida { get; set; }
		public ICollection<Reservacion>? ReservacionesDevolucion { get; set; }
	}
}