using System.ComponentModel.DataAnnotations;

namespace KiasRentCarApi.DTOs.Mensaje
{
	public class RespuestaRequest
	{
		[Required]
		[MaxLength(2000)]
		public string Respuesta { get; set; } = string.Empty;
	}
}