namespace SistemaEscuela.DTO.Materia
{
	public class CursoMateriaDTO
	{
		public int Id { get; set; }

		public int IdCurso { get; set; }

		public int IdMateria { get; set; }

		public string Materia { get; set; }

		public DateTime FechaCreacion { get; set; }
	}
}
