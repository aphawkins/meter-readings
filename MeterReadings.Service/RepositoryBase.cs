namespace MeterReadings.Service
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Threading.Tasks;
	using AutoMapper;
	using AutoMapper.QueryableExtensions;
	using MeterReadings.Data;
	using Microsoft.EntityFrameworkCore;

	public abstract class RepositoryBase<TDto, TEntity> : IRepository<TDto> where TDto : class where TEntity : class
	{
		protected MainDbContext RepositoryContext { get; set; }

		protected RepositoryBase(MainDbContext repositoryContext) => RepositoryContext = repositoryContext;

		public async Task <IEnumerable<TDto>> ReadAsync()
		{
			return await RepositoryContext.Set<TEntity>().AsNoTracking().ProjectTo<TDto>(DtoMapperConfig.Config).ToListAsync();
		}

		public async Task<IEnumerable<T>> ReadAsync<T>(MapperConfiguration mapperConfig)
		{
			return await RepositoryContext.Set<TEntity>().AsNoTracking().ProjectTo<TDto>(DtoMapperConfig.Config).ProjectTo<T>(mapperConfig).ToListAsync();
		}
		
		public async Task<IEnumerable<TDto>> ReadAsync(Expression<Func<TDto, bool>> expression)
		{
			return await RepositoryContext.Set<TEntity>().AsNoTracking().ProjectTo<TDto>(DtoMapperConfig.Config).Where(expression).ToListAsync();
		}

		public async Task<TDto> CreateAsync(TDto dto)
		{
			Mapper mapper = new(DtoMapperConfig.Config);
			TDto newDto = mapper.Map<TDto>(RepositoryContext.Set<TEntity>().Add(mapper.Map<TEntity>(dto)).Entity);

			try
			{
				await RepositoryContext.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				throw new MeterReadingsServiceException("Error creating entity.", ex);
			}

			return newDto;
		}

		public async Task<TDto> UpdateAsync(TDto dto)
		{
			Mapper mapper = new(DtoMapperConfig.Config);
			TDto updated = mapper.Map<TDto>(RepositoryContext.Set<TEntity>().Update(mapper.Map<TEntity>(dto)).Entity);
			try
			{
				await RepositoryContext.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				throw new MeterReadingsServiceException("Error updating entity.", ex);
			}
			return updated;
		}

		public async Task DeleteAsync()
		{
			foreach (TEntity entity in RepositoryContext.Set<TEntity>())
			{
				RepositoryContext.Remove(entity);
			}
			await RepositoryContext.SaveChangesAsync();
		}

		public async Task DeleteAsync(TDto dto)
		{
			if (dto == null)
			{
				return;
			}

			Mapper mapper = new(DtoMapperConfig.Config);
			RepositoryContext.Set<TEntity>().Remove(mapper.Map<TEntity>(dto));
			await RepositoryContext.SaveChangesAsync();
		}
	}
}
