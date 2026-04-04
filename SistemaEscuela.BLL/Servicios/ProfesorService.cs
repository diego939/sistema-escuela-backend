using Microsoft.EntityFrameworkCore;
using SistemaEscuela.BLL.Contratos;
using SistemaEscuela.DAL.Repositorios.Contrato;
using SistemaEscuela.DTO.Comun;
using SistemaEscuela.DTO.Profesor;
using SistemaEscuela.Model;

namespace SistemaEscuela.BLL.Servicios
{
	public class ProfesorService : IProfesorService
	{
		private readonly IGenericRepository<ProfesorCursoMateria> _profesorCursoMateriaRepository;
		private readonly IGenericRepository<Usuario> _usuarioRepository;
		private readonly IGenericRepository<CursoMateria> _cursoMateriaRepository;
		private readonly IGenericRepository<Materia> _materiaRepository;
		private readonly IGenericRepository<Curso> _cursoRepository;

		public ProfesorService(
			IGenericRepository<ProfesorCursoMateria> profesorCursoMateriaRepository,
			IGenericRepository<Usuario> usuarioRepository,
			IGenericRepository<CursoMateria> cursoMateriaRepository,
			IGenericRepository<Materia> materiaRepository,
			IGenericRepository<Curso> cursoRepository)
		{
			_profesorCursoMateriaRepository = profesorCursoMateriaRepository;
			_usuarioRepository = usuarioRepository;
			_cursoMateriaRepository = cursoMateriaRepository;
			_materiaRepository = materiaRepository;
			_cursoRepository = cursoRepository;
		}

		public async Task<ProfesorCursoMateriaDTO> AsignarProfesorAMateria(AsignarProfesorDTO modelo)
		{
			// Validar que el profesor existe
			var profesor = await _usuarioRepository.Consultar(u =>
				u.Id == modelo.IdProfesor &&
				u.FechaEliminacion == null)
				.FirstOrDefaultAsync();

			if (profesor == null)
				throw new Exception("El profesor no existe");

			// Validar que la asignación de materia al curso existe
			var cursoMateria = await _cursoMateriaRepository.Consultar(cm =>
				cm.Id == modelo.IdCursoMateria &&
				cm.FechaEliminacion == null)
				.Include(cm => cm.IdCursoNavigation)
				.Include(cm => cm.IdMateriaNavigation)
				.FirstOrDefaultAsync();

			if (cursoMateria == null)
				throw new Exception("La materia no está asignada a este curso");

			// Verificar que el profesor no esté ya asignado a esta materia en este curso
			var asignacionExistente = await _profesorCursoMateriaRepository.Consultar(pcm =>
				pcm.IdProfesor == modelo.IdProfesor &&
				pcm.IdCursomateria == modelo.IdCursoMateria &&
				pcm.FechaEliminacion == null)
				.FirstOrDefaultAsync();

			if (asignacionExistente != null)
				throw new Exception("Este profesor ya está asignado a esta materia en este curso");

			var profesorCursoMateria = new ProfesorCursoMateria
			{
				IdProfesor = modelo.IdProfesor,
				IdCursomateria = modelo.IdCursoMateria,
				FechaCreacion = DateTime.Now
			};

			var creada = await _profesorCursoMateriaRepository.Crear(profesorCursoMateria);

			return new ProfesorCursoMateriaDTO
			{
				Id = creada.Id,
				IdProfesor = creada.IdProfesor.Value,
				ProfesorNombre = $"{profesor.Nombres} {profesor.Apellidos}",
				IdCursoMateria = creada.IdCursomateria.Value,
				Materia = cursoMateria.IdMateriaNavigation.Descripcion,
				IdCurso = cursoMateria.IdCursoNavigation.Id,
				Curso = GenerarNombreCurso(cursoMateria.IdCursoNavigation),
				FechaAsignacion = creada.FechaCreacion.Value
			};
		}

		public async Task<List<MateriaProfesorDTO>> ObtenerMateriasDelProfesor(int idProfesor)
		{
			// Validar que el profesor existe
			var profesorExiste = await _usuarioRepository.Consultar(u =>
				u.Id == idProfesor &&
				u.FechaEliminacion == null)
				.AnyAsync();

			if (!profesorExiste)
				throw new Exception("El profesor no existe");

			// Traer solo lo necesario y ordenar en SQL
			var datos = await _profesorCursoMateriaRepository.Consultar(pcm =>
				pcm.IdProfesor == idProfesor &&
				pcm.FechaEliminacion == null)
				.Select(pcm => new
				{
					pcm.Id,
					Materia = pcm.IdCursomateriaNavigation.IdMateriaNavigation.Descripcion,
					Curso = pcm.IdCursomateriaNavigation.IdCursoNavigation
				})
				.OrderBy(x => x.Curso.Anio)
				.ThenBy(x => x.Curso.Modulo)
				.ThenBy(x => x.Curso.Division)
				.ThenBy(x => x.Materia)
				.AsNoTracking()
				.ToListAsync();

			// Mapear en memoria (acá sí usamos el método custom)
			var resultado = datos
				.Select(x => new MateriaProfesorDTO
				{
					Id = x.Id,
					Materia = x.Materia,
					IdCurso = x.Curso.Id,
					Curso = GenerarNombreCurso(x.Curso)
				})
				.ToList();

			return resultado;
		}

		public async Task<List<CursoProfesorDTO>> ObtenerCursosDelProfesor(int idProfesor)
		{
			// Validar que el profesor existe
			var profesor = await _usuarioRepository.Consultar(u =>
				u.Id == idProfesor &&
				u.FechaEliminacion == null)
				.FirstOrDefaultAsync();

			if (profesor == null)
				throw new Exception("El profesor no existe");

			var datos = await _profesorCursoMateriaRepository.Consultar(pcm =>
				pcm.IdProfesor == idProfesor &&
				pcm.FechaEliminacion == null)
				.Include(pcm => pcm.IdCursomateriaNavigation)
				.ThenInclude(cm => cm.IdCursoNavigation)
				.ToListAsync();

			var cursos = datos
				.Select(pcm => pcm.IdCursomateriaNavigation.IdCursoNavigation)
				.DistinctBy(c => c.Id)
				.Select(c => new CursoProfesorDTO
				{
					IdCurso = c.Id,
					Modulo = c.Modulo.Value,
					Division = c.Division,
					Modalidad = c.Modalidad,
					Turno = c.Turno,
					Anio = c.Anio.Value
				})
				.OrderBy(c => c.Anio)
				.ThenBy(c => c.Modulo)
				.ThenBy(c => c.Division)
				.ToList();

			return cursos;
		}

		private string GenerarNombreCurso(Curso curso)
		{
			return $"Año: {curso.Anio} - Grado {curso.Modulo} ° - División ({curso.Division}) - Turno {curso.Turno}";
		}

		public async Task<PaginatedResult<ProfesorDTO>> ListaProfesoresPaginado(PaginationRequest request)
		{
			var query = _usuarioRepository.Consultar().Where(u => u.IdRolNavigation.Descripcion.ToLower() == "profesor");

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
				.Select(u => new ProfesorDTO
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

			return new PaginatedResult<ProfesorDTO>
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
