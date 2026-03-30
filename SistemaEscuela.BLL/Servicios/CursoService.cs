using Microsoft.EntityFrameworkCore;
using SistemaEscuela.BLL.Contratos;
using SistemaEscuela.DAL.Repositorios.Contrato;
using SistemaEscuela.DTO.Comun;
using SistemaEscuela.DTO.Curso;
using SistemaEscuela.DTO.Usuario;
using SistemaEscuela.Model;

namespace SistemaEscuela.BLL.Servicios
{
	public class CursoService : ICursoService
	{
		private readonly IGenericRepository<Curso> _cursoRepository;

		public CursoService(IGenericRepository<Curso> cursoRepository)
		{
			_cursoRepository = cursoRepository;
		}

		public async Task<CursoDTO> CrearCurso(CrearCursoDTO modelo)
		{
			// Validar datos
			if (modelo.Modulo <= 0)
				throw new Exception("El módulo/grado debe ser mayor a 0");

			if (string.IsNullOrWhiteSpace(modelo.Division))
				throw new Exception("La división es requerida");

			if (string.IsNullOrWhiteSpace(modelo.Modalidad))
				throw new Exception("La modalidad es requerida");

			if (string.IsNullOrWhiteSpace(modelo.Turno))
				throw new Exception("El turno es requerido");

			if (modelo.Anio <= 0)
				throw new Exception("El año lectivo debe ser válido");

			if (modelo.CupoMaximo <= 0)
				throw new Exception("El cupo máximo debe ser mayor a 0");

			// Verificar que no exista un curso igual
			var cursoExistente = await _cursoRepository.Consultar(c =>
				c.Modulo == modelo.Modulo &&
				c.Division == modelo.Division &&
				c.Modalidad == modelo.Modalidad &&
				c.Turno == modelo.Turno &&
				c.Anio == modelo.Anio &&
				c.FechaEliminacion == null)
				.FirstOrDefaultAsync();

			if (cursoExistente != null)
				throw new Exception("Ya existe un curso con estas características");

			var curso = new Curso
			{
				Modulo = modelo.Modulo,
				Division = modelo.Division,
				Modalidad = modelo.Modalidad,
				Turno = modelo.Turno,
				Anio = modelo.Anio,
				CupoMaximo = modelo.CupoMaximo,
				FechaCreacion = DateTime.Now
			};

			var creado = await _cursoRepository.Crear(curso);

			return new CursoDTO
			{
				Id = creado.Id,
				Modulo = creado.Modulo,
				Division = creado.Division,
				Modalidad = creado.Modalidad,
				Turno = creado.Turno,
				Anio = creado.Anio,
				CupoMaximo = creado.CupoMaximo,
				FechaCreacion = creado.FechaCreacion
			};
		}

		public async Task<List<CursoDTO>> ObtenerCursosDisponibles()
		{
			var cursos = await _cursoRepository.Consultar(c => c.FechaEliminacion == null)
				.Select(c => new CursoDTO
				{
					Id = c.Id,
					Modulo = c.Modulo,
					Division = c.Division,
					Modalidad = c.Modalidad,
					Turno = c.Turno,
					Anio = c.Anio,
					CupoMaximo = c.CupoMaximo,
					FechaCreacion = c.FechaCreacion
				})
				.OrderBy(c => c.Anio)
				.ThenBy(c => c.Modulo)
				.ThenBy(c => c.Division)
				.ToListAsync();

			return cursos;
		}

		public async Task<PaginatedResult<CursoDTO>> ObtenerCursosPaginado(PaginationRequest request)
		{
			var query = _cursoRepository.Consultar(c => c.FechaEliminacion == null);

			// Aplicar búsqueda
			if (!string.IsNullOrWhiteSpace(request.Search))
			{
				var searchLower = request.Search.ToLower();
				query = query.Where(c =>
					c.Division.ToLower().Contains(searchLower) ||
					c.Modalidad.ToLower().Contains(searchLower) ||
					c.Turno.ToLower().Contains(searchLower) ||
					c.Anio.ToString().Contains(searchLower));
			}

			// Contar total de registros
			var totalRecords = await query.CountAsync();

			// Aplicar ordenamiento
			query = ApplySortingToCursos(query, request.SortBy, request.SortDescending);

			// Aplicar paginación
			var cursos = await query
				.Skip((request.PageNumber - 1) * request.PageSize)
				.Take(request.PageSize)
				.Select(c => new CursoDTO
				{
					Id = c.Id,
					Modulo = c.Modulo,
					Division = c.Division,
					Modalidad = c.Modalidad,
					Turno = c.Turno,
					Anio = c.Anio,
					CupoMaximo = c.CupoMaximo,
					FechaCreacion = c.FechaCreacion
				})
				.ToListAsync();

			var totalPages = (int)Math.Ceiling(totalRecords / (double)request.PageSize);

			return new PaginatedResult<CursoDTO>
			{
				Data = cursos,
				TotalRecords = totalRecords,
				TotalPages = totalPages,
				PageNumber = request.PageNumber,
				PageSize = request.PageSize
			};
		}

		private IQueryable<Curso> ApplySortingToCursos(IQueryable<Curso> query, string sortBy, bool sortDescending)
		{
			return sortBy.ToLower() switch
			{
				"id" => sortDescending ? query.OrderByDescending(c => c.Id) : query.OrderBy(c => c.Id),
				"modulo" => sortDescending ? query.OrderByDescending(c => c.Modulo) : query.OrderBy(c => c.Modulo),
				"division" => sortDescending ? query.OrderByDescending(c => c.Division) : query.OrderBy(c => c.Division),
				"modalidad" => sortDescending ? query.OrderByDescending(c => c.Modalidad) : query.OrderBy(c => c.Modalidad),
				"turno" => sortDescending ? query.OrderByDescending(c => c.Turno) : query.OrderBy(c => c.Turno),
				"anio" => sortDescending ? query.OrderByDescending(c => c.Anio) : query.OrderBy(c => c.Anio),
				"cupmaximo" => sortDescending ? query.OrderByDescending(c => c.CupoMaximo) : query.OrderBy(c => c.CupoMaximo),
				_ => sortDescending ? query.OrderByDescending(c => c.Anio).ThenByDescending(c => c.Modulo).ThenByDescending(c => c.Division) : query.OrderBy(c => c.Anio).ThenBy(c => c.Modulo).ThenBy(c => c.Division),
			};
		}

	}
}

