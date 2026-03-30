using SistemaEscuela.DTO.Comun;
using SistemaEscuela.DTO.Curso;
using SistemaEscuela.DTO.Usuario;

namespace SistemaEscuela.BLL.Contratos
{
	public interface ICursoService
	{
		Task<CursoDTO> CrearCurso(CrearCursoDTO modelo);

		Task<List<CursoDTO>> ObtenerCursosDisponibles();

		Task<PaginatedResult<CursoDTO>> ObtenerCursosPaginado(PaginationRequest request);
	}
}
