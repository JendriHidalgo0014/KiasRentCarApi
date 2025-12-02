namespace KiasRentCarApi.DTOs.Mensaje
{
	public class MensajeDto
	{
		public int MensajeId { get; set; }
		public int UsuarioId { get; set; }
		public string NombreUsuario { get; set; } = string.Empty;
		public string Asunto { get; set; } = string.Empty;
		public string Contenido { get; set; } = string.Empty;
		public string? Respuesta { get; set; }
		public DateTime FechaCreacion { get; set; }
		public DateTime? FechaRespuesta { get; set; }
		public bool Leido { get; set; }
	}
}