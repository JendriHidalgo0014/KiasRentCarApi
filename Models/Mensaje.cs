using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KiasRentCarApi.Models
{
	public class Mensaje
	{
		[Key]
		public int MensajeId { get; set; }

		[Required]
		public int UsuarioId { get; set; }

		[Required]
		[MaxLength(200)]
		public string Asunto { get; set; } = string.Empty;

		[Required]
		[MaxLength(2000)]
		public string Contenido { get; set; } = string.Empty;

		[MaxLength(2000)]
		public string? Respuesta { get; set; }

		public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

		public DateTime? FechaRespuesta { get; set; }

		public bool Leido { get; set; } = false;

		// Navegación
		[ForeignKey("UsuarioId")]
		public Usuario? Usuario { get; set; }
	}
}