using Microsoft.EntityFrameworkCore;
using Munna.Concentrix.MovieGram.Core;
using Munna.Concentrix.MovieGram.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Munna.Concentrix.MovieGram { 

    public partial class MovieGramRepository<TEntity> : IMovieGramRepository<TEntity> where TEntity : BaseEntity
    {
        #region Fields

        private readonly IMovieGramDbContext _context;

        private DbSet<TEntity> _entities;

        #endregion

        #region Ctor

        public MovieGramRepository(IMovieGramDbContext context)
        {
            _context = context;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Rollback of entity changes and return full error message
        /// </summary>
        /// <param name="exception">Exception</param>
        /// <returns>Error message</returns>
        protected string GetFullErrorTextAndRollbackEntityChanges(DbUpdateException exception)
        {
            //rollback entity changes
            if (_context is DbContext dbContext)
            {
                var entries = dbContext.ChangeTracker.Entries()
                    .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified).ToList();

                entries.ForEach(entry =>
                {
                    try
                    {
                        entry.State = EntityState.Unchanged;
                    }
                    catch (InvalidOperationException)
                    {
                        // ignored
                    }
                });
            }

            try
            {
                _context.SaveChanges();
                return exception.ToString();
            }
            catch (Exception ex)
            {
                //if after the rollback of changes the context is still not saving,
                //return the full text of the exception that occurred when saving
                return ex.ToString();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get entity by identifier
        /// </summary>
        /// <param name="id">Identifier</param>
        /// <returns>Entity</returns>
        public virtual async Task<TEntity> GetById(long id)
        {
            return await Entities.FindAsync(id);
        }
        public virtual async Task<TEntity> GetInformatinDetail()
        {
            return await Entities.FirstOrDefaultAsync();
        }
        /// <summary>
        /// Insert entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual async Task Insert(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            try
            {
                await Entities.AddAsync(entity);
                _context.SaveChanges();
            }
            catch (DbUpdateException exception)
            {
                //ensure that the detailed error text is saved in the Log
                throw new Exception(GetFullErrorTextAndRollbackEntityChanges(exception), exception);
            }
        }

        /// <summary>
        /// Insert entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual async Task Insert(IEnumerable<TEntity> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            try
            {
                await Entities.AddRangeAsync(entities);
                _context.SaveChanges();
            }
            catch (DbUpdateException exception)
            {
                //ensure that the detailed error text is saved in the Log
                throw new Exception(GetFullErrorTextAndRollbackEntityChanges(exception), exception);
            }
        }

        /// <summary>
        /// Update entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual async Task Update(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            try
            {
                await Task.Run(() =>
                {
                    Entities.Update(entity);
                    _context.SaveChanges();
                });
               
            }
            catch (DbUpdateException exception)
            {
                //ensure that the detailed error text is saved in the Log
                throw new Exception(GetFullErrorTextAndRollbackEntityChanges(exception), exception);
            }
        }

        /// <summary>
        /// Update entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual async Task Update(IEnumerable<TEntity> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            try
            {
                await Task.Run(() =>
                {
                    Entities.UpdateRange(entities);
                    _context.SaveChanges();
                });
               
            }
            catch (DbUpdateException exception)
            {
                //ensure that the detailed error text is saved in the Log
                throw new Exception(GetFullErrorTextAndRollbackEntityChanges(exception), exception);
            }
        }

        /// <summary>
        /// Delete entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual async Task Delete(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            try
            {
                await Task.Run(() =>
                {
                    Entities.Remove(entity);
                    _context.SaveChanges();
                });
            }
            catch (DbUpdateException exception)
            {
                //ensure that the detailed error text is saved in the Log
                throw new Exception(GetFullErrorTextAndRollbackEntityChanges(exception), exception);
            }
        }

        /// <summary>
        /// Delete entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual async Task Delete(IEnumerable<TEntity> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            try
            {
                await Task.Run(() =>
                {
                    Entities.RemoveRange(entities);
                    _context.SaveChanges();
                });
               
            }
            catch (DbUpdateException exception)
            {
                //ensure that the detailed error text is saved in the Log
                throw new Exception(GetFullErrorTextAndRollbackEntityChanges(exception), exception);
            }
        }



        #endregion

        #region Properties

        /// <summary>
        /// Gets a table
        /// </summary>
        public virtual IQueryable<TEntity> Table => Entities;

        /// <summary>
        /// Gets a table with "no tracking" enabled (EF feature) Use it only when you load record(s) only for read-only operations
        /// </summary>
        public virtual IQueryable<TEntity> TableNoTracking => Entities.AsNoTracking();

        /// <summary>
        /// Gets an entity set
        /// </summary>
        protected virtual DbSet<TEntity> Entities
        {
            get
            {
                if (_entities == null)
                    _entities = _context.Set<TEntity>();

                return _entities;
            }
        }

        #endregion
       
    }
}
