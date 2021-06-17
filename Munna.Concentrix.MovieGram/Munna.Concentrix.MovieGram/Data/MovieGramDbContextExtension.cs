using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Munna.Concentrix.MovieGram.Data
{
    public static class MovieGramDbContextExtension
    {
        public static IQueryable<TSource> Page<TSource>(this IQueryable<TSource> source, int pageIndex, int pageSize, out int totalPages, out int totalRecords)
        {
            totalRecords = source.Count();
            totalPages = (int)Math.Ceiling((decimal)totalRecords / (decimal)pageSize);
            return source.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }
    }
   
}
