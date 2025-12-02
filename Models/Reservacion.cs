using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KiasRentCarApi.Models
{
	public class Reservacion
	{
		[Key]
		public int ReservacionId { get; set; }

		[Required]
		public int UsuarioId { get; set; }

		[Required]
		public int VehiculoId { get; set; }

		[Required]
		public DateTime FechaRecogida { get; set; }

		[Required]
		[MaxLength(10)]
		public string HoraRecogida { get; set; } = "10:00";

		[Required]
		public DateTime FechaDevolucion { get; set; }

		[Required]
		[MaxLength(10)]
		public string HoraDevolucion { get; set; } = "10:00";

		[Required]
		public int UbicacionRecogidaId { get; set; }

		[Required]
		public int UbicacionDevolucionId { get; set; }

		[Required]
		[MaxLength(50)]
		public string Estado { get; set; } = "Pendiente";

		public double Subtotal { get; set; }

		public double Impuestos { get; set; }

		public double Total { get; set; }

		[Required]
		[MaxLength(20)]
		public string CodigoReserva { get; set; } = string.Empty;

		public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

		// Navegación
		[ForeignKey("UsuarioId")]
		public Usuario? Usuario { get; set; }

		[ForeignKey("VehiculoId")]
		public Vehiculo? Vehiculo { get; set; }

		[ForeignKey("UbicacionRecogidaId")]
		public Ubicacion? UbicacionRecogida { get; set; }

		[ForeignKey("UbicacionDevolucionId")]
		public Ubicacion? UbicacionDevolucion { get; set; }
	}
}