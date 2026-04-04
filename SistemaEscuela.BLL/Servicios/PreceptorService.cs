using Microsoft.EntityFrameworkCore;
using SistemaEscuela.BLL.Contratos;
using SistemaEscuela.DAL.Repositorios.Contrato;
using SistemaEscuela.DTO.Comun;
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

		public async Task<PaginatedResult<PreceptorDTO>> ListaPreceptoresPaginado(PaginationRequest request)
		{
			var query = _usuarioRepository.Consultar().Where(u => u.IdRolNavigation.Descripcion.ToLower() == "preceptor");

			// Aplicar búsqueda
			if (!string.IsNullOrWhiteSpace(request.Search))
			{
				var searchLower = request.Search.ToLower();
				query = query.Where(u =>
					u.Nombres.ToLower().Contains(searchLower) ||
					u.Apellidos.ToLower().Contains(searchLower) ||
					u.Email.ToLower().Contains(searchLower) ||
					u.Dni.ToLower().Contains(searchLower));
			}

			// Contar total de registros
			var totalRecords = await query.CountAsync();

			// Aplicar ordenamiento
			query = ApplySortingToUsuarios(query, request.SortBy, request.SortDescending);

			// Aplicar paginación
			var usuarios = await query
				.Include(u => u.IdRolNavigation)
				.Skip((request.PageNumber - 1) * request.PageSize)
				.Take(request.PageSize)
				.Select(u => new PreceptorDTO
				{
					Id = u.Id,
					Nombres = u.Nombres,
					Apellidos = u.Apellidos,
					Email = u.Email,
					Telefono = u.Telefono,
					IdRol = u.IdRol,
					Rol = u.IdRolNavigation.Descripcion,
					Dni = u.Dni,
					UrlImagen = u.UrlImagen,
					FechaEliminacion = u.FechaEliminacion
				})
				.ToListAsync();

			var totalPages = (int)Math.Ceiling(totalRecords / (double)request.PageSize);

			return new PaginatedResult<PreceptorDTO>
			{
				Data = usuarios,
				TotalRecords = totalRecords,
				TotalPages = totalPages,
				PageNumber = request.PageNumber,
				PageSize = request.PageSize
			};
		}

		private IQueryable<Usuario> ApplySortingToUsuarios(IQueryable<Usuario> query, string sortBy, bool sortDescending)
		{
			return sortBy.ToLower() switch
			{
				"id" => sortDescending ? query.OrderByDescending(u => u.Id) : query.OrderBy(u => u.Id),
				"apellidos" => sortDescending ? query.OrderByDescending(u => u.Apellidos) : query.OrderBy(u => u.Apellidos),
				"email" => sortDescending ? query.OrderByDescending(u => u.Email) : query.OrderBy(u => u.Email),
				"dni" => sortDescending ? query.OrderByDescending(u => u.Dni) : query.OrderBy(u => u.Dni),
				"telefono" => sortDescending ? query.OrderByDescending(u => u.Telefono) : query.OrderBy(u => u.Telefono),
				_ => sortDescending ? query.OrderByDescending(u => u.Nombres) : query.OrderBy(u => u.Nombres),
			};
		}
	}
}
