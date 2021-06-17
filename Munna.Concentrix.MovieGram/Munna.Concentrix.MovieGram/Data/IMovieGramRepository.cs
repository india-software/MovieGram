using Munna.Concentrix.MovieGram.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace  Munna.Concentrix.MovieGram.Data
{
    public partial interface IMovieGramRepository<TEntity> where TEntity : BaseEntity
    {
        #region Methods

        /// <summary>
        /// Get entity by identifier
        /// </summary>
        /// <param name="id">Identifier</param>
        /// <returns>Entity</returns>
        Task<TEntity> GetById(long id);

        /// <summary>
        /// Insert entity
        /// </summary>
        /// <param name="entity">Entity</param>
        Task Insert(TEntity entity);

        /// <summary>
        /// Insert entities
        /// </summary>
        /// <param name="entities">Entities</param>
        Task Insert(IEnumerable<TEntity> entities);

        /// <summary>
        /// Update entity
        /// </summary>
        /// <param name="entity">Entity</param>
        Task Update(TEntity entity);

        /// <summary>
        /// Update entities
        /// </summary>
        /// <param name="entities">Entities</param>
        Task Update(IEnumerable<TEntity> entities);

        /// <summary>
        /// Delete entity
        /// </summary>
        /// <param name="entity">Entity</param>
        Task Delete(TEntity entity);

        /// <summary>
        /// Delete entities
        /// </summary>
        /// <param name="entities">Entities</param>
        Task Delete(IEnumerable<TEntity> entities);
        /// <summary>
        /// Get Information about entity
        /// </summary>
        /// <returns></returns>
        Task<TEntity> GetInformatinDetail();

        #endregion

        #region Properties

        /// <summary>
        /// Gets a table
        /// </summary>
        IQueryable<TEntity> Table { get; }

        /// <summary>
        /// Gets a table with "no tracking" enabled (EF feature) Use it only when you load record(s) only for read-only operations
        /// </summary>
        IQueryable<TEntity> TableNoTracking { get; }

        #endregion
    }
}
