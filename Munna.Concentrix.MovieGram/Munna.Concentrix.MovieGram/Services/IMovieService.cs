using Munna.Concentrix.MovieGram.Core;
using Munna.Concentrix.MovieGram.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Munna.Concentrix.MovieGram.Service
{
    public interface IMovieService
    {
        Task DeleteMovie(Movie movie);
        Task DeleteMovieShowTime(MovieShowTime movieShowTime);
        Task<IList<Movie>> GetAllMovies();
        Task<Movie> GetMovieById(long movieId);
        Task<IList<Movie>> GetMoviesByName(string name);
        Task<MovieShowTime> GetMovieShowTimeById(long movieShowTimeId);
        Task<IList<MovieShowTime>> GetMovieShowTimeByMovieId(long movieId);
        Task InsertMovie(Movie movie);
        Task InsertMovieShowTime(MovieShowTime movieShowTime);
        Task<PageListModel<MovieModel>> SearchMovies(MovieSearchModel searchModel);
        Task UpdateMovie(Movie movie);
        Task UpdateMovieShowTime(MovieShowTime movieShowTime);
    }
}