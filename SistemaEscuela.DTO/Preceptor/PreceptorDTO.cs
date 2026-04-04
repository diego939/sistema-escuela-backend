using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaEscuela.DTO.Preceptor
{
	public class PreceptorDTO
	{
		public int Id { get; set; }

		public string Nombres { get; set; }

		public string Apellidos { get; set; }

		public string Email { get; set; }

		public string Telefono { get; set; }

		public string Rol { get; set; }

		public int? IdRol { get; set; }

		public string Dni { get; set; }

		public string UrlImagen { get; set; }

		public string Token { get; set; }

		public DateTime? FechaEliminacion { get; set; }
	}
}
