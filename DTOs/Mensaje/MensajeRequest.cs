using System.ComponentModel.DataAnnotations;

namespace KiasRentCarApi.DTOs.Mensaje
{
	public class MensajeRequest
	{
		[Required]
		public int UsuarioId { get; set; }

		[Required]
		[MaxLength(200)]
		public string Asunto { get; set; } = string.Empty;

		[Required]
		[MaxLength(2000)]
		public string Contenido { get; set; } = string.Empty;
	}
}