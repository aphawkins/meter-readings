// <copyright file="IRepository.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace MeterReadingsService
{
	using System.Linq;
	using System.Threading.Tasks;

	/// <summary>
	/// A generic repository.
	/// </summary>
	/// <typeparam name="T">The type of items stored in the repository.</typeparam>
	public interface IRepository<T>
    {
        /// <summary>
        /// Adds a new item to the repository.
        /// </summary>
        /// <param name="item">The new item to add.</param>
        Task<T> CreateAsync(T item);

        /// <summary>
        /// Retrieves all the items.
        /// </summary>
        /// <returns>All the items.</returns>
        IQueryable<T> Read();

		/// <summary>
		/// Retrieves an item.
		/// </summary>
		/// <returns>The item by id.</returns>
		Task<T> ReadAsync(int id);

		/// <summary>
		/// Updates an item in the repository.
		/// </summary>
		/// <param name="item">The item to update.</param>
		Task<T> UpdateAsync(T item);

		/// <summary>
		/// Removes all items from the repository.
		/// </summary>
		Task<int> DeleteAsync();

		/// <summary>
		/// Removes an item from the repository.
		/// </summary>
		/// <param name="item">The id of the item to delete.</param>
		Task<bool> DeleteAsync(int id);

	}
}