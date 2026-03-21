using SistemaEscuela.DTO.Calificacion;

namespace SistemaEscuela.BLL.Contratos
{
	public interface ICalificacionService
	{
		Task<CalificacionResultDTO> RegistrarCalificacion(RegistrarCalificacionDTO modelo);

		Task<CalificacionResultDTO> ActualizarCalificacion(ActualizarCalificacionDTO modelo);

		Task<List<CalificacionDTO>> ObtenerTodasLasCalificaciones();

		Task<List<CalificacionDTO>> ObtenerCalificacionesPorAlumno(int idAlumno);

		Task<List<CalificacionDTO>> ObtenerCalificacionesPorAlumnoYMateria(int idAlumno, int idMateria);

		Task<List<CalificacionDTO>> ObtenerCalificacionesPorCursoYMateria(int idCurso, int idMateria);

		Task<bool> EliminarCalificacion(int idCalificacion);
	}
}
