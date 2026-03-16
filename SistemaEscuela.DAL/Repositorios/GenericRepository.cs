using System;
using System.Collections.Generic;
using System.Text;
using SistemaEscuela.DAL.Repositorios.Contrato;
using SistemaEscuela.DAL.DBContext;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace SistemaEscuela.DAL.Repositorios
{
	public class GenericRepository<TModel> : IGenericRepository<TModel> where TModel : class
	{
		private readonly SistemaEscolarContext _dbContext;
		public GenericRepository(SistemaEscolarContext dbContext)
		{
			_dbContext = dbContext;
		}
		public async Task<TModel> Obtener(Expression<Func<TModel, bool>> filtro)
		{
			try
			{
				return await _dbContext
					.Set<TModel>()
					.AsNoTracking()
					.FirstOrDefaultAsync(filtro);
			}
			catch (Exception ex)
			{
				throw new Exception("Error al obtener el registro.", ex);
			}
		}

		public async Task<TModel> Crear(TModel modelo)
		{
			try
			{
				await _dbContext.Set<TModel>().AddAsync(modelo);
				await _dbContext.SaveChangesAsync();
				return modelo;
			}
			catch (Exception ex)
			{
				throw new Exception("Error al crear el registro.", ex);
			}
		}

		public async Task<bool> Editar(TModel modelo)
		{
			try
			{
				_dbContext.Set<TModel>().Update(modelo);
				await _dbContext.SaveChangesAsync();
				return true;
			}
			catch (Exception ex)
			{
				throw new Exception("Error al editar el registro.", ex);
			}
		}

		public async Task<bool> Eliminar(TModel modelo)
		{
			try
			{
				// Eliminación lógica si existe propiedad FechaEliminacion
				var prop = modelo.GetType().GetProperty("FechaEliminacion");

				if (prop != null)
				{
					prop.SetValue(modelo, DateTime.Now);
					_dbContext.Set<TModel>().Update(modelo);
				}
				else
				{
					_dbContext.Set<TModel>().Remove(modelo);
				}

				await _dbContext.SaveChangesAsync();
				return true;
			}
			catch (Exception ex)
			{
				throw new Exception("Error al eliminar el registro.", ex);
			}
		}
		public IQueryable<TModel> Consultar(Expression<Func<TModel, bool>>? filtro = null)
        {
            try
            {
                IQueryable<TModel> query = _dbContext
                    .Set<TModel>()
                    .AsNoTracking();

                if (filtro != null)
                    query = query.Where(filtro);

                return query;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al consultar los registros.", ex);
            }
        }

	}
}
