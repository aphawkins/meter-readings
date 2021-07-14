﻿namespace MeterReadingsService
{
	using System;
	using System.Linq;
	using System.Linq.Expressions;
	using AutoMapper;
	using AutoMapper.QueryableExtensions;
	using MeterReadingsData;
	using Microsoft.EntityFrameworkCore;

	public abstract class RepositoryBase<TDto, TEntity> : IRepository<TDto> where TDto : class where TEntity : class
	{
		protected MainDbContext RepositoryContext { get; set; }

		protected RepositoryBase(MainDbContext repositoryContext) => RepositoryContext = repositoryContext;

		public IQueryable<TDto> Read()
		{
			return RepositoryContext.Set<TEntity>().AsNoTracking().ProjectTo<TDto>(MapperConfig.Config);
		}

		public IQueryable<TDto> Read(Expression<Func<TDto, bool>> expression)
		{
			return RepositoryContext.Set<TEntity>().AsNoTracking().ProjectTo<TDto>(MapperConfig.Config).Where(expression);
		}

		public TDto Create(TDto dto)
		{
			Mapper mapper = new(MapperConfig.Config);
			TDto newDto = mapper.Map<TDto>(RepositoryContext.Set<TEntity>().Add(mapper.Map<TEntity>(dto)).Entity);
			RepositoryContext.SaveChanges();
			return newDto;
		}

		public TDto Update(TDto dto)
		{
			Mapper mapper = new(MapperConfig.Config);
			TDto updated = mapper.Map<TDto>(RepositoryContext.Set<TEntity>().Update(mapper.Map<TEntity>(dto)).Entity);
			RepositoryContext.SaveChanges();
			return updated;
		}

		public void Delete()
		{
			foreach (TEntity entity in RepositoryContext.Set<TEntity>())
			{
				RepositoryContext.Remove(entity);
			}
			RepositoryContext.SaveChanges();
		}

		public void Delete(TDto dto)
		{
			if (dto == null)
			{
				return;
			}

			Mapper mapper = new(MapperConfig.Config);
			RepositoryContext.Set<TEntity>().Remove(mapper.Map<TEntity>(dto));
			RepositoryContext.SaveChanges();
		}
	}
}
