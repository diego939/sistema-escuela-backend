using SistemaEscuela.DTO.Comun;
using SistemaEscuela.DTO.Profesor;
using SistemaEscuela.DTO.Usuario;

namespace SistemaEscuela.BLL.Contratos
{
	public interface IProfesorService
	{
		Task<ProfesorCursoMateriaDTO> AsignarProfesorAMateria(AsignarProfesorDTO modelo);

		Task<List<MateriaProfesorDTO>> ObtenerMateriasDelProfesor(int idProfesor);

		Task<List<CursoProfesorDTO>> ObtenerCursosDelProfesor(int idProfesor);

		Task<PaginatedResult<ProfesorDTO>> ListaProfesoresPaginado(PaginationRequest request);
	}
}
