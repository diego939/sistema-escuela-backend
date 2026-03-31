using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaEscuela.DTO.Curso
{
	public class EditarCursoDTO
	{
		public int Id { get; set; }
		public int Modulo { get; set; }

		public string Division { get; set; }

		public string Modalidad { get; set; }

		public string Turno { get; set; }

		public int Anio { get; set; }

		public int CupoMaximo { get; set; }
	}
}
