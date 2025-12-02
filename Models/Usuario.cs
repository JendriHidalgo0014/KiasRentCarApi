using System.ComponentModel.DataAnnotations;

namespace KiasRentCarApi.Models
{
	public class Usuario
	{
		[Key]
		public int UsuarioId { get; set; }

		[Required]
		[MaxLength(100)]
		public string Nombre { get; set; } = string.Empty;

		[Required]
		[MaxLength(150)]
		public string Email { get; set; } = string.Empty;

		[Required]
		[MaxLength(255)]
		public string Password { get; set; } = string.Empty;

		[MaxLength(20)]
		public string? Telefono { get; set; }

		[Required]
		[MaxLength(20)]
		public string Rol { get; set; } = "Cliente";

		public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

		public bool Activo { get; set; } = true;

		// Navegación
		public ICollection<Reservacion>? Reservaciones { get; set; }
		public ICollection<Mensaje>? Mensajes { get; set; }
	}
}