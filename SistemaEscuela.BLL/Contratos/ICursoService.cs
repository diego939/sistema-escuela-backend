using SistemaEscuela.DTO.Curso;

namespace SistemaEscuela.BLL.Contratos
{
	public interface ICursoService
	{
		Task<CursoDTO> CrearCurso(CrearCursoDTO modelo);

		Task<List<CursoDTO>> ObtenerCursosDisponibles();
	}
}
