using SistemaEscuela.DTO.Posteo;

namespace SistemaEscuela.BLL.Contratos
{
	public interface IPosteoService
	{
		Task<PosteoResultDTO> PublicarPosteo(CrearPosteoDTO modelo, int idProfesor);

		Task<PosteoResultDTO> ActualizarPosteo(ActualizarPosteoDTO modelo);

		Task<List<PosteoDTO>> ObtenerPosteosPorCursoMateria(int idCursomateria);

		Task<List<PosteoDTO>> ObtenerPosteosPorCurso(int idCurso);

		Task<List<PosteoDTO>> ObtenerPosteosPorProfesor(int idProfesor);

		Task<List<PosteoDTO>> ObtenerTodosLosPosteos();

		Task<bool> EliminarPosteo(int idPosteo);
	}
}
