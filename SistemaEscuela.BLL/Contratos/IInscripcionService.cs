using SistemaEscuela.DTO.Inscripcion;

namespace SistemaEscuela.BLL.Contratos
{
	public interface IInscripcionService
	{
		Task<InscripcionResultDTO> InscribirAlumnoEnCurso(InscribirAlumnoDTO modelo);

		Task<List<InscripcionDTO>> ObtenerInscripcionesDelAlumno(int idAlumno);

		Task<List<InscripcionDTO>> ObtenerAlumnosInscritosEnCurso(int idCurso);

		Task<bool> DesinscribirAlumno(int idAlumno, int idCurso);
	}
}
