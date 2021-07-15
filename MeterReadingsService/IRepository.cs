// <copyright file="IRepository.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace MeterReadingsService
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Threading.Tasks;
	using AutoMapper;

	/// <summary>
	/// A generic repository.
	/// </summary>
	/// <typeparam name="TDto">The type of items stored in the repository.</typeparam>
	public interface IRepository<TDto>
    {
        /// <summary>
        /// Adds a new item to the repository.
        /// </summary>
        /// <param name="entity">The new item to add.</param>
        TDto Create(TDto dto);

		/// <summary>
		/// Retrieves all the items.
		/// </summary>
		/// <returns>All the items.</returns>
		Task<IEnumerable<TDto>> ReadAsync();

		/// <summary>
		/// Retrieves all the items.
		/// </summary>
		/// <returns>All the items.</returns>
		Task<IEnumerable<T>> ReadAsync<T>(MapperConfiguration mapperConfig);

		/// <summary>
		/// Retrieves an item by expression.
		/// </summary>
		/// <returns>The item by expression.</returns>
		IQueryable<TDto> Read(Expression<Func<TDto, bool>> expression);

		/// <summary>
		/// Updates an item in the repository.
		/// </summary>
		/// <param name="entity">The item to update.</param>
		TDto Update(TDto dto);

		/// <summary>
		/// Removes all items from the repository.
		/// </summary>
		void Delete();

		/// <summary>
		/// Removes an entity from the repository.
		/// </summary>
		void Delete(TDto dto);
	}
}