using Microsoft.EntityFrameworkCore;
using Munna.Concentrix.MovieGram.Core;
using System.Linq;

namespace Munna.Concentrix.MovieGram.Data
{
    public interface IMovieGramDbContext
    {
        void Detach<TEntity>(TEntity entity) where TEntity : BaseEntity;
        int SaveChanges();
        DbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity;
    }
}