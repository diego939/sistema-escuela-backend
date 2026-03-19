namespace SistemaEscuela.DTO.Usuario
{
	public class CambiarPasswordDTO
	{
		public int IdUsuario { get; set; }

		public string PasswordActual { get; set; }

		public string PasswordNueva { get; set; }

		public string RepetirPasswordNueva { get; set; }
	}
}
