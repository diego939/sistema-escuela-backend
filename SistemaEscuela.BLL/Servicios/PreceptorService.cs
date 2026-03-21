using Microsoft.EntityFrameworkCore;
using SistemaEscuela.BLL.Contratos;
using SistemaEscuela.DAL.Repositorios.Contrato;
using SistemaEscuela.DTO.Preceptor;
using SistemaEscuela.Model;

namespace SistemaEscuela.BLL.Servicios
{
	public class PreceptorService : IPreceptorService
	{
		private readonly IGenericRepository<PreceptorCurso> _preceptorCursoRepository;
		private readonly IGenericRepository<Usuario> _usuarioRepository;
		private readonly IGenericRepository<Curso> _cursoRepository;

		public PreceptorService(
			IGenericRepository<PreceptorCurso> preceptorCursoRepository,
			IGenericRepository<Usuario> usuarioRepository,
			IGenericRepository<Curso> cursoRepository)
		{
			_preceptorCursoRepository = preceptorCursoRepository;
			_usuarioRepository = usuarioRepository;
			_cursoRepository = cursoRepository;
		}

		public async Task<PreceptorCursoResultDTO> AsignarPreceptorACurso(AsignarPreceptorDTO modelo)
		{
			// Validar que el preceptor existe
			var preceptor = await _usuarioRepository.Consultar(u =>
				u.Id == modelo.IdPreceptor &&
				u.FechaEliminacion == null)
				.FirstOrDefaultAsync();

			if (preceptor == null)
				throw new Exception("El preceptor no existe");

			// Validar que el curso existe
			var curso = await _cursoRepository.Consultar(c =>
				c.Id == modelo.IdCurso &&
				c.FechaEliminacion == null)
				.FirstOrDefaultAsync();

			if (curso == null)
				throw new Exception("El curso no existe");

			// Validar que no exista una asignación previa activa
			var asignacionExistente = await _preceptorCursoRepository.Consultar(pc =>
				pc.IdPreceptor == modelo.IdPreceptor &&
				pc.IdCurso == modelo.IdCurso &&
				pc.FechaEliminacion == null)
				.FirstOrDefaultAsync();

			if (asignacionExistente != null)
				throw new Exception("El preceptor ya está asignado a este curso");

			// Crear la nueva asignación
			var preceptorCurso = new PreceptorCurso
			{
				IdPreceptor = modelo.IdPreceptor,
				IdCurso = modelo.IdCurso,
				FechaCreacion = DateTime.Now
			};

			var creada = await _preceptorCursoRepository.Crear(preceptorCurso);

			return new PreceptorCursoResultDTO
			{
				Id = creada.Id,
				IdPreceptor = creada.IdPreceptor.Value,
				IdCurso = creada.IdCurso.Value,
				FechaCreacion = creada.FechaCreacion.Value
			};
		}

		public async Task<List<PreceptorCursoDTO>> ObtenerCursosDelPreceptor(int idPreceptor)
		{
			// Validar que el preceptor existe
			var preceptor = await _usuarioRepository.Consultar(u =>
				u.Id == idPreceptor &&
				u.FechaEliminacion == null)
				.FirstOrDefaultAsync();

			if (preceptor == null)
				throw new Exception("El preceptor no existe");

			// Obtener los cursos asignados al preceptor
			var cursos = await _preceptorCursoRepository.Consultar(pc =>
				pc.IdPreceptor == idPreceptor &&
				pc.FechaEliminacion == null)
				.Include(pc => pc.IdCursoNavigation)
				.Select(pc => new PreceptorCursoDTO
				{
					IdCurso = pc.IdCurso.Value,
					Modulo = pc.IdCursoNavigation.Modulo.Value,
					Division = pc.IdCursoNavigation.Division,
					Modalidad = pc.IdCursoNavigation.Modalidad,
					Turno = pc.IdCursoNavigation.Turno,
					Anio = pc.IdCursoNavigation.Anio.Value
				})
				.ToListAsync();

			return cursos;
		}

		public async Task<bool> DesasignarPreceptorDelCurso(int idPreceptor, int idCurso)
		{
			// Buscar la asignación activa
			var asignacion = await _preceptorCursoRepository.Consultar(pc =>
				pc.IdPreceptor == idPreceptor &&
				pc.IdCurso == idCurso &&
				pc.FechaEliminacion == null)
				.FirstOrDefaultAsync();

			if (asignacion == null)
				throw new Exception("La asignación no existe");

			// Marcar como eliminada
			asignacion.FechaEliminacion = DateTime.Now;
			await _preceptorCursoRepository.Editar(asignacion);

			return true;
		}
	}
}
