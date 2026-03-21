namespace SistemaEscuela.DTO.Inscripcion
{
	public class InscribirAlumnoDTO
	{
		public int IdAlumno { get; set; }

		public int IdCurso { get; set; }

		public int? IdMediodepago { get; set; }

		public string? Direccion { get; set; }

		public string? Comprobante { get; set; }

		public decimal? Monto { get; set; }
	}
}
