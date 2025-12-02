using System.ComponentModel.DataAnnotations;

namespace KiasRentCarApi.DTOs.Reservacion
{
	public class EstadoRequest
	{
		[Required]
		public string Estado { get; set; } = string.Empty;
	}
}