using Microsoft.EntityFrameworkCore;
using SistemaEscuela.BLL.Contratos;
using SistemaEscuela.DAL.Repositorios.Contrato;
using SistemaEscuela.DTO.Posteo;
using SistemaEscuela.Model;

namespace SistemaEscuela.BLL.Servicios
{
	public class PosteoService : IPosteoService
	{
		private readonly IGenericRepository<Posteo> _posteoRepository;
		private readonly IGenericRepository<Usuario> _usuarioRepository;
		private readonly IGenericRepository<CursoMateria> _cursoMateriaRepository;
		private readonly IGenericRepository<Curso> _cursoRepository;
		private readonly IGenericRepository<Materia> _materiaRepository;

		public PosteoService(
			IGenericRepository<Posteo> posteoRepository,
			IGenericRepository<Usuario> usuarioRepository,
			IGenericRepository<CursoMateria> cursoMateriaRepository,
			IGenericRepository<Curso> cursoRepository,
			IGenericRepository<Materia> materiaRepository)
		{
			_posteoRepository = posteoRepository;
			_usuarioRepository = usuarioRepository;
			_cursoMateriaRepository = cursoMateriaRepository;
			_cursoRepository = cursoRepository;
			_materiaRepository = materiaRepository;
		}

		public async Task<PosteoResultDTO> PublicarPosteo(CrearPosteoDTO modelo, int idProfesor)
		{
			// Validar que el profesor existe
			var profesor = await _usuarioRepository.Consultar(u =>
				u.Id == idProfesor &&
				u.FechaEliminacion == null)
				.FirstOrDefaultAsync();

			if (profesor == null)
				throw new Exception("El profesor no existe");

			// Validar que la asignación de materia al curso existe
			var cursoMateria = await _cursoMateriaRepository.Consultar(cm =>
				cm.Id == modelo.IdCursomateria &&
				cm.FechaEliminacion == null)
				.FirstOrDefaultAsync();

			if (cursoMateria == null)
				throw new Exception("La asignación de materia al curso no existe");

			// Validar que el título no esté vacío
			if (string.IsNullOrWhiteSpace(modelo.Titulo))
				throw new Exception("El título del posteo es obligatorio");

			// Validar que la descripción no esté vacía
			if (string.IsNullOrWhiteSpace(modelo.Descripcion))
				throw new Exception("La descripción del posteo es obligatoria");

			// Crear el nuevo posteo
			var posteo = new Posteo
			{
				IdUsuario = idProfesor,
				IdCursomateria = modelo.IdCursomateria,
				Titulo = modelo.Titulo,
				Descripcion = modelo.Descripcion,
				UrlArchivo = modelo.UrlArchivo,
				FechaCreacion = DateTime.Now
			};

			var creado = await _posteoRepository.Crear(posteo);

			return new PosteoResultDTO
			{
				Id = creado.Id,
				IdUsuario = creado.IdUsuario.Value,
				ProfesorNombre = $"{profesor.Nombres} {profesor.Apellidos}",
				IdCursomateria = creado.IdCursomateria.Value,
				Titulo = creado.Titulo,
				Descripcion = creado.Descripcion,
				UrlArchivo = creado.UrlArchivo,
				FechaPublicacion = creado.FechaCreacion.Value
			};
		}

		public async Task<PosteoResultDTO> ActualizarPosteo(ActualizarPosteoDTO modelo)
		{
			// Buscar el posteo
			var posteo = await _posteoRepository.Consultar(p =>
				p.Id == modelo.Id &&
				p.FechaEliminacion == null)
				.FirstOrDefaultAsync();

			if (posteo == null)
				throw new Exception("El posteo no existe");

			// Validar que el título no esté vacío
			if (string.IsNullOrWhiteSpace(modelo.Titulo))
				throw new Exception("El título del posteo es obligatorio");

			// Validar que la descripción no esté vacía
			if (string.IsNullOrWhiteSpace(modelo.Descripcion))
				throw new Exception("La descripción del posteo es obligatoria");

			// Actualizar el posteo
			posteo.Titulo = modelo.Titulo;
			posteo.Descripcion = modelo.Descripcion;
			posteo.UrlArchivo = modelo.UrlArchivo;

			await _posteoRepository.Editar(posteo);

			// Obtener el profesor para retornar el nombre
			var profesor = await _usuarioRepository.Consultar(u => u.Id == posteo.IdUsuario)
				.FirstOrDefaultAsync();

			return new PosteoResultDTO
			{
				Id = posteo.Id,
				IdUsuario = posteo.IdUsuario.Value,
				ProfesorNombre = $"{profesor.Nombres} {profesor.Apellidos}",
				IdCursomateria = posteo.IdCursomateria.Value,
				Titulo = posteo.Titulo,
				Descripcion = posteo.Descripcion,
				UrlArchivo = posteo.UrlArchivo,
				FechaPublicacion = posteo.FechaCreacion.Value
			};
		}

		public async Task<List<PosteoDTO>> ObtenerPosteosPorCursoMateria(int idCursomateria)
		{
			// Validar que la asignación de materia al curso existe
			var cursoMateria = await _cursoMateriaRepository.Consultar(cm =>
				cm.Id == idCursomateria &&
				cm.FechaEliminacion == null)
				.FirstOrDefaultAsync();

			if (cursoMateria == null)
				throw new Exception("La asignación de materia al curso no existe");

			// Obtener los posteos de la materia en el curso
			var datos = await _posteoRepository.Consultar(p =>
				p.IdCursomateria == idCursomateria &&
				p.FechaEliminacion == null)
				.Include(p => p.IdUsuarioNavigation)
				.Include(p => p.IdCursomateriaNavigation)
				.ThenInclude(cm => cm.IdCursoNavigation)
				.Include(p => p.IdCursomateriaNavigation)
				.ThenInclude(cm => cm.IdMateriaNavigation)
				.ToListAsync();

			// Mapear en memoria
			var posteos = datos
				.Select(p => new PosteoDTO
				{
					Id = p.Id,
					IdUsuario = p.IdUsuario.Value,
					ProfesorNombre = $"{p.IdUsuarioNavigation.Nombres} {p.IdUsuarioNavigation.Apellidos}",
					IdCurso = p.IdCursomateriaNavigation.IdCurso.Value,
					CursoNombre = GenerarNombreCurso(p.IdCursomateriaNavigation.IdCursoNavigation),
					IdMateria = p.IdCursomateriaNavigation.IdMateria.Value,
					MateriaNombre = p.IdCursomateriaNavigation.IdMateriaNavigation.Descripcion,
					Titulo = p.Titulo,
					Descripcion = p.Descripcion,
					UrlArchivo = p.UrlArchivo,
					FechaPublicacion = p.FechaCreacion.Value
				})
				.OrderByDescending(x => x.FechaPublicacion)
				.ToList();

			return posteos;
		}

		public async Task<List<PosteoDTO>> ObtenerPosteosPorCurso(int idCurso)
		{
			// Validar que el curso existe
			var curso = await _cursoRepository.Consultar(c =>
				c.Id == idCurso &&
				c.FechaEliminacion == null)
				.FirstOrDefaultAsync();

			if (curso == null)
				throw new Exception("El curso no existe");

			// Obtener los posteos del curso
			var datos = await _posteoRepository.Consultar(p =>
				p.IdCursomateriaNavigation.IdCurso == idCurso &&
				p.FechaEliminacion == null)
				.Include(p => p.IdUsuarioNavigation)
				.Include(p => p.IdCursomateriaNavigation)
				.ThenInclude(cm => cm.IdCursoNavigation)
				.Include(p => p.IdCursomateriaNavigation)
				.ThenInclude(cm => cm.IdMateriaNavigation)
				.ToListAsync();

			// Mapear en memoria
			var posteos = datos
				.Select(p => new PosteoDTO
				{
					Id = p.Id,
					IdUsuario = p.IdUsuario.Value,
					ProfesorNombre = $"{p.IdUsuarioNavigation.Nombres} {p.IdUsuarioNavigation.Apellidos}",
					IdCurso = p.IdCursomateriaNavigation.IdCurso.Value,
					CursoNombre = GenerarNombreCurso(p.IdCursomateriaNavigation.IdCursoNavigation),
					IdMateria = p.IdCursomateriaNavigation.IdMateria.Value,
					MateriaNombre = p.IdCursomateriaNavigation.IdMateriaNavigation.Descripcion,
					Titulo = p.Titulo,
					Descripcion = p.Descripcion,
					UrlArchivo = p.UrlArchivo,
					FechaPublicacion = p.FechaCreacion.Value
				})
				.OrderByDescending(x => x.FechaPublicacion)
				.ToList();

			return posteos;
		}

		public async Task<List<PosteoDTO>> ObtenerPosteosPorProfesor(int idProfesor)
		{
			// Validar que el profesor existe
			var profesor = await _usuarioRepository.Consultar(u =>
				u.Id == idProfesor &&
				u.FechaEliminacion == null)
				.FirstOrDefaultAsync();

			if (profesor == null)
				throw new Exception("El profesor no existe");

			// Obtener los posteos del profesor
			var datos = await _posteoRepository.Consultar(p =>
				p.IdUsuario == idProfesor &&
				p.FechaEliminacion == null)
				.Include(p => p.IdUsuarioNavigation)
				.Include(p => p.IdCursomateriaNavigation)
				.ThenInclude(cm => cm.IdCursoNavigation)
				.Include(p => p.IdCursomateriaNavigation)
				.ThenInclude(cm => cm.IdMateriaNavigation)
				.ToListAsync();

			// Mapear en memoria
			var posteos = datos
				.Select(p => new PosteoDTO
				{
					Id = p.Id,
					IdUsuario = p.IdUsuario.Value,
					ProfesorNombre = $"{p.IdUsuarioNavigation.Nombres} {p.IdUsuarioNavigation.Apellidos}",
					IdCurso = p.IdCursomateriaNavigation.IdCurso.Value,
					CursoNombre = GenerarNombreCurso(p.IdCursomateriaNavigation.IdCursoNavigation),
					IdMateria = p.IdCursomateriaNavigation.IdMateria.Value,
					MateriaNombre = p.IdCursomateriaNavigation.IdMateriaNavigation.Descripcion,
					Titulo = p.Titulo,
					Descripcion = p.Descripcion,
					UrlArchivo = p.UrlArchivo,
					FechaPublicacion = p.FechaCreacion.Value
				})
				.OrderByDescending(x => x.FechaPublicacion)
				.ToList();

			return posteos;
		}

		public async Task<List<PosteoDTO>> ObtenerTodosLosPosteos()
		{
			// Obtener todos los posteos activos
			var datos = await _posteoRepository.Consultar(p =>
				p.FechaEliminacion == null)
				.Include(p => p.IdUsuarioNavigation)
				.Include(p => p.IdCursomateriaNavigation)
				.ThenInclude(cm => cm.IdCursoNavigation)
				.Include(p => p.IdCursomateriaNavigation)
				.ThenInclude(cm => cm.IdMateriaNavigation)
				.ToListAsync();

			// Mapear en memoria
			var posteos = datos
				.Select(p => new PosteoDTO
				{
					Id = p.Id,
					IdUsuario = p.IdUsuario.Value,
					ProfesorNombre = $"{p.IdUsuarioNavigation.Nombres} {p.IdUsuarioNavigation.Apellidos}",
					IdCurso = p.IdCursomateriaNavigation.IdCurso.Value,
					CursoNombre = GenerarNombreCurso(p.IdCursomateriaNavigation.IdCursoNavigation),
					IdMateria = p.IdCursomateriaNavigation.IdMateria.Value,
					MateriaNombre = p.IdCursomateriaNavigation.IdMateriaNavigation.Descripcion,
					Titulo = p.Titulo,
					Descripcion = p.Descripcion,
					UrlArchivo = p.UrlArchivo,
					FechaPublicacion = p.FechaCreacion.Value
				})
				.OrderByDescending(x => x.FechaPublicacion)
				.ToList();

			return posteos;
		}

		public async Task<bool> EliminarPosteo(int idPosteo)
		{
			// Buscar el posteo
			var posteo = await _posteoRepository.Consultar(p =>
				p.Id == idPosteo &&
				p.FechaEliminacion == null)
				.FirstOrDefaultAsync();

			if (posteo == null)
				throw new Exception("El posteo no existe");

			// Marcar como eliminado
			posteo.FechaEliminacion = DateTime.Now;
			await _posteoRepository.Editar(posteo);

			return true;
		}

		private string GenerarNombreCurso(Curso curso)
		{
			return $"Año: {curso.Anio} - Grado {curso.Modulo}° - División ({curso.Division}) - Turno {curso.Turno}";
		}
	}
}
