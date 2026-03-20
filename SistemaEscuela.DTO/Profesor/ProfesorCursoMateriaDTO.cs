namespace SistemaEscuela.DTO.Profesor
{
	public class ProfesorCursoMateriaDTO
	{
		public int Id { get; set; }

		public int IdProfesor { get; set; }

		public string ProfesorNombre { get; set; }

		public int IdCursoMateria { get; set; }

		public string Materia { get; set; }

		public int IdCurso { get; set; }

		public string Curso { get; set; }

		public DateTime FechaAsignacion { get; set; }
	}
}
