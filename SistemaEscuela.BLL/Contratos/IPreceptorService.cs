using SistemaEscuela.DTO.Comun;
using SistemaEscuela.DTO.Preceptor;
using SistemaEscuela.DTO.Profesor;

namespace SistemaEscuela.BLL.Contratos
{
	public interface IPreceptorService
	{
		Task<PreceptorCursoResultDTO> AsignarPreceptorACurso(AsignarPreceptorDTO modelo);

		Task<List<PreceptorCursoDTO>> ObtenerCursosDelPreceptor(int idPreceptor);

		Task<bool> DesasignarPreceptorDelCurso(int idPreceptor, int idCurso);

		Task<PaginatedResult<PreceptorDTO>> ListaPreceptoresPaginado(PaginationRequest request);
	}
}
