using SistemaEscuela.DTO.Materia;

namespace SistemaEscuela.BLL.Contratos
{
	public interface IMateriaService
	{
		Task<MateriaDTO> CrearMateria(CrearMateriaDTO modelo);

		Task<List<MateriaDTO>> ObtenerMaterias();

		Task<CursoMateriaDTO> AsociarMateriaACurso(AsociarMateriaDTO modelo);

		Task<List<CursoMateriaDTO>> ObtenerMateriasDelCurso(int idCurso);
	}
}
