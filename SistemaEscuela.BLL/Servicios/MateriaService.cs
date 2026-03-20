using Microsoft.EntityFrameworkCore;
using SistemaEscuela.BLL.Contratos;
using SistemaEscuela.DAL.Repositorios.Contrato;
using SistemaEscuela.DTO.Materia;
using SistemaEscuela.Model;

namespace SistemaEscuela.BLL.Servicios
{
	public class MateriaService : IMateriaService
	{
		private readonly IGenericRepository<Materia> _materiaRepository;
		private readonly IGenericRepository<CursoMateria> _cursoMateriaRepository;
		private readonly IGenericRepository<Curso> _cursoRepository;

		public MateriaService(
			IGenericRepository<Materia> materiaRepository,
			IGenericRepository<CursoMateria> cursoMateriaRepository,
			IGenericRepository<Curso> cursoRepository)
		{
			_materiaRepository = materiaRepository;
			_cursoMateriaRepository = cursoMateriaRepository;
			_cursoRepository = cursoRepository;
		}

		public async Task<MateriaDTO> CrearMateria(CrearMateriaDTO modelo)
		{
			// Validar datos
			if (string.IsNullOrWhiteSpace(modelo.Descripcion))
				throw new Exception("La descripción de la materia es requerida");

			// Verificar que no exista una materia igual
			var materiaExistente = await _materiaRepository.Consultar(m =>
				m.Descripcion == modelo.Descripcion &&
				m.FechaEliminacion == null)
				.FirstOrDefaultAsync();

			if (materiaExistente != null)
				throw new Exception("Ya existe una materia con esa descripción");

			var materia = new Materia
			{
				Descripcion = modelo.Descripcion,
				FechaCreacion = DateTime.Now
			};

			var creada = await _materiaRepository.Crear(materia);

			return new MateriaDTO
			{
				Id = creada.Id,
				Descripcion = creada.Descripcion,
				FechaCreacion = creada.FechaCreacion.Value,
			};
		}

		public async Task<List<MateriaDTO>> ObtenerMaterias()
		{
			var materias = await _materiaRepository.Consultar(m => m.FechaEliminacion == null)
				.Select(m => new MateriaDTO
				{
					Id = m.Id,
					Descripcion = m.Descripcion,
					FechaCreacion = m.FechaCreacion.Value
				})
				.OrderBy(m => m.Descripcion)
				.ToListAsync();

			return materias;
		}

		public async Task<CursoMateriaDTO> AsociarMateriaACurso(AsociarMateriaDTO modelo)
		{
			// Validar que el curso existe
			var curso = await _cursoRepository.Consultar(c =>
				c.Id == modelo.IdCurso &&
				c.FechaEliminacion == null)
				.FirstOrDefaultAsync();

			if (curso == null)
				throw new Exception("El curso no existe");

			// Validar que la materia existe
			var materia = await _materiaRepository.Consultar(m =>
				m.Id == modelo.IdMateria &&
				m.FechaEliminacion == null)
				.FirstOrDefaultAsync();

			if (materia == null)
				throw new Exception("La materia no existe");

			// Verificar que no exista una asociación igual
			var asociacionExistente = await _cursoMateriaRepository.Consultar(cm =>
				cm.IdCurso == modelo.IdCurso &&
				cm.IdMateria == modelo.IdMateria &&
				cm.FechaEliminacion == null)
				.FirstOrDefaultAsync();

			if (asociacionExistente != null)
				throw new Exception("Esta materia ya está asociada al curso");

			var cursoMateria = new CursoMateria
			{
				IdCurso = modelo.IdCurso,
				IdMateria = modelo.IdMateria,
				FechaCreacion = DateTime.Now
			};

			var creada = await _cursoMateriaRepository.Crear(cursoMateria);

			return new CursoMateriaDTO
			{
				Id = creada.Id,
				IdCurso = creada.IdCurso.Value,
				IdMateria = creada.IdMateria.Value,
				Materia = materia.Descripcion,
				FechaCreacion = creada.FechaCreacion.Value
			};
		}

		public async Task<List<CursoMateriaDTO>> ObtenerMateriasDelCurso(int idCurso)
		{
			// Validar que el curso existe
			var curso = await _cursoRepository.Consultar(c =>
				c.Id == idCurso &&
				c.FechaEliminacion == null)
				.FirstOrDefaultAsync();

			if (curso == null)
				throw new Exception("El curso no existe");

			var materias = await _cursoMateriaRepository.Consultar(cm =>
				cm.IdCurso == idCurso &&
				cm.FechaEliminacion == null)
				.Include(cm => cm.IdMateriaNavigation)
				.Select(cm => new CursoMateriaDTO
				{
					Id = cm.Id,
					IdCurso = cm.IdCurso.Value,
					IdMateria = cm.IdMateria.Value,
					Materia = cm.IdMateriaNavigation.Descripcion,
					FechaCreacion = cm.FechaCreacion.Value
				})
				.OrderBy(cm => cm.Materia)
				.ToListAsync();

			return materias;
		}
	}
}
