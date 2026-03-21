using SistemaEscuela.DTO.Preceptor;

namespace SistemaEscuela.BLL.Contratos
{
	public interface IPreceptorService
	{
		Task<PreceptorCursoResultDTO> AsignarPreceptorACurso(AsignarPreceptorDTO modelo);

		Task<List<PreceptorCursoDTO>> ObtenerCursosDelPreceptor(int idPreceptor);

		Task<bool> DesasignarPreceptorDelCurso(int idPreceptor, int idCurso);
	}
}
