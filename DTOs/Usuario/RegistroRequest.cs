using System.ComponentModel.DataAnnotations;

namespace KiasRentCarApi.DTOs.Usuario
{
	public class RegistroRequest
	{
		[Required]
		[MaxLength(100)]
		public string Nombre { get; set; } = string.Empty;

		[Required]
		[EmailAddress]
		[MaxLength(150)]
		public string Email { get; set; } = string.Empty;

		[Required]
		[MinLength(4)]
		public string Password { get; set; } = string.Empty;

		[MaxLength(20)]
		public string? Telefono { get; set; }

		public string Rol { get; set; } = "Cliente";
	}
}