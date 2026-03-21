using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SistemaEscuela.DAL.DBContext;
using SistemaEscuela.DAL.Repositorios;
using SistemaEscuela.DAL.Repositorios.Contrato;
using SistemaEscuela.BLL.Contratos;
using SistemaEscuela.BLL.Servicios;
using SistemaEscuela.Utility;

namespace SistemaEscuela.IOC
{
	public static class Dependencia
	{
		public static void InyectarDependencias(this IServiceCollection services, IConfiguration configuration)
		{
			// DBContext
			services.AddDbContext<SistemaEscolarContext>(options =>
			{
				options.UseSqlServer(configuration.GetConnectionString("cadenaSQL"));
			});

			// Repositorios
			services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

			// Utilidades
			services.AddScoped<JwtHelper>();

			// Servicios
				services.AddScoped<IUsuarioService, UsuarioService>();
				services.AddScoped<ICursoService, CursoService>();
				services.AddScoped<IMateriaService, MateriaService>();
				services.AddScoped<IProfesorService, ProfesorService>();
				services.AddScoped<IPreceptorService, PreceptorService>();
				services.AddScoped<IInscripcionService, InscripcionService>();
				services.AddScoped<ICalificacionService, CalificacionService>();
				services.AddScoped<IPosteoService, PosteoService>();
			}
	}
}
