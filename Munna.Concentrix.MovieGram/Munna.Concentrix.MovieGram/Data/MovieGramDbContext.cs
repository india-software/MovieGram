using Microsoft.EntityFrameworkCore;
using Munna.Concentrix.MovieGram.Core;
using Munna.Concentrix.MovieGram.Data.Mapper;
using System;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;

namespace Munna.Concentrix.MovieGram.Data
{
    public partial class MovieGramDbContext : DbContext, IMovieGramDbContext
    {
        #region Ctor
        public MovieGramDbContext(DbContextOptions<MovieGramDbContext> options) : base(options)
        {
            this.Database.EnsureCreated();
        }
        #endregion

        #region Utilities
        /// <summary>
        /// Further configuration the model
        /// </summary>
        /// <param name="modelBuilder">The builder being used to construct the model for this context</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //dynamically load all entity and query type configurations
            var typeConfigurations = Assembly.GetExecutingAssembly().GetTypes().Where(type =>
                (type.BaseType?.IsGenericType ?? false)
                    && type.BaseType.GetGenericTypeDefinition() == typeof(MovieGramEntityTypeConfiguration<>));

            foreach (var typeConfiguration in typeConfigurations)
            {
                var configuration = (IMappingConfiguration)Activator.CreateInstance(typeConfiguration);
                configuration.ApplyConfiguration(modelBuilder);
            }

            base.OnModelCreating(modelBuilder);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates a DbSet that can be used to query and save instances of entity
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <returns>A set for the given entity type</returns>
        public new DbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity
        {
            return base.Set<TEntity>();
        }

        /// <summary>
        /// Detach an entity from the context
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <param name="entity">Entity</param>
        public virtual void Detach<TEntity>(TEntity entity) where TEntity : BaseEntity
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var entityEntry = Entry(entity);
            if (entityEntry == null)
                return;
            //set the entity is not being tracked by the context
            entityEntry.State = EntityState.Detached;
        }
        
        
        #endregion
    }

}
