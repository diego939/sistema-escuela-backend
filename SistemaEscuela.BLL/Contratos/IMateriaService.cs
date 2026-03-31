using SistemaEscuela.DTO.Comun;
using SistemaEscuela.DTO.Materia;

namespace SistemaEscuela.BLL.Contratos
{
	public interface IMateriaService
	{
		Task<MateriaDTO> CrearMateria(CrearMateriaDTO modelo);

		Task<List<MateriaDTO>> ObtenerMaterias();

		Task<CursoMateriaDTO> AsociarMateriaACurso(AsociarMateriaDTO modelo);

		Task<bool> DesasociarMateriaCurso(int idCurso, int idMateria);

		Task<List<CursoMateriaDTO>> ObtenerMateriasDelCurso(int idCurso);

		Task<MateriaDTO> EditarMateria(EditarMateriDTO modelo);
		Task<PaginatedResult<MateriaDTO>> ObtenerMateriasPaginado(PaginationRequest request);
	}
}
