using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Munna.Concentrix.MovieGram.Core;
using Munna.Concentrix.MovieGram.Data;
using Munna.Concentrix.MovieGram.Model;

namespace Munna.Concentrix.MovieGram.Service
{
    /// <summary>
    /// Product service
    /// </summary>
    public partial class MovieService : IMovieService
    {
        #region Fields
        private readonly IMovieGramDbContext _dbContext;
        private readonly IMovieGramRepository<Movie> _movieRepository;
        private readonly IMovieGramRepository<MovieShowTime> _movieShowTimeRepository;
        private readonly IMapper _mapper;
        #endregion

        #region Ctor

        public MovieService(IMovieGramDbContext dbContext,
           IMovieGramRepository<Movie> movieRepository,
            IMovieGramRepository<MovieShowTime> movieShowTimeRepository, IMapper mapper)
        {
            _dbContext = dbContext;
            _movieRepository = movieRepository;
            _movieShowTimeRepository = movieShowTimeRepository;
            _mapper = mapper;

        }

        #endregion


        #region Methods
        #region Movies
        /// <summary>
        /// Delete a movie
        /// </summary>
        /// <param name="movie">Product</param>
        public async Task DeleteMovie(Movie movie)
        {
            if (movie == null)
                throw new ArgumentNullException(nameof(movie));
            await _movieRepository.Delete(movie);
        }

        /// <summary>
        /// Gets Movie
        /// </summary>
        /// <param name="movieId">Product identifier</param>
        /// <returns>Movie</returns>
        public async Task<Movie> GetMovieById(long movieId)
        {
            if (movieId == 0)
                return null;
            return await _movieRepository.GetById(movieId);
        }

        public async Task<IList<Movie>> GetAllMovies()
        {
            var query = await Task.Run(() =>
            {
                return _movieRepository.Table;
            });
            return query.ToList();
        }

        /// <summary>
        /// Inserts a movie
        /// </summary>
        /// <param name="movie">Product</param>
        public async Task InsertMovie(Movie movie)
        {
            if (movie == null)
                throw new ArgumentNullException(nameof(movie));

            //insert
            await _movieRepository.Insert(movie);
        }

        /// <summary>
        /// Updates the movie
        /// </summary>
        /// <param name="movie">Product</param>
        public async Task UpdateMovie(Movie movie)
        {
            if (movie == null)
                throw new ArgumentNullException(nameof(movie));
            //update
            await _movieRepository.Update(movie);
        }

        public async Task<IList<Movie>> GetMoviesByName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return null;

            name = name.Trim();
            var query = await Task.Run(() =>
            {
                return from p in _movieRepository.Table
                       orderby p.Id
                       where
                     p.Title == name
                       select p;
            });
            return query.ToList();
        }

        #endregion

        #region Movie Show Time

        /// <summary>
        /// Insert Movie SHow Time
        /// </summary>
        /// <param name="movieShowTime"></param>
        /// <returns></returns>
        public async Task InsertMovieShowTime(MovieShowTime movieShowTime)
        {
            if (movieShowTime == null)
                throw new ArgumentNullException(nameof(movieShowTime));

            await _movieShowTimeRepository.Insert(movieShowTime);

        }
        /// <summary>
        /// Update Movie Show Time
        /// </summary>
        /// <param name="movieShowTime"></param>
        /// <returns></returns>
        public async Task UpdateMovieShowTime(MovieShowTime movieShowTime)
        {
            if (movieShowTime == null)
                throw new ArgumentNullException(nameof(movieShowTime));

            await _movieShowTimeRepository.Update(movieShowTime);

        }
        /// <summary>
        /// Get List of Movie Show time
        /// </summary>
        /// <param name="movieId"></param>
        /// <returns></returns>
        public async Task<IList<MovieShowTime>> GetMovieShowTimeByMovieId(long movieId)
        {
            var query = await Task.Run(() =>
            {
                return from ms in _movieShowTimeRepository.Table
                       where ms.MovieId == movieId
                       select ms;
            });
            return query.ToList();
        }
        /// <summary>
        /// Delete movie show time
        /// </summary>
        /// <param name="movieShowTime"></param>
        /// <returns></returns>
        public async Task DeleteMovieShowTime(MovieShowTime movieShowTime)
        {
            if (movieShowTime == null)
                throw new ArgumentNullException(nameof(movieShowTime));

            await _movieShowTimeRepository.Delete(movieShowTime);

        }


        public async Task<MovieShowTime> GetMovieShowTimeById(long movieShowTimeId)
        {
            if (movieShowTimeId == 0)
                return null;

            return await _movieShowTimeRepository.GetById(movieShowTimeId);
        }



        #endregion
        #region movie search and filter
        public async Task<PageListModel<MovieModel>> SearchMovies(
           MovieSearchModel searchModel
          )
        {
            if (searchModel.PageSize == 0) 
                searchModel.PageSize = int.MaxValue;
            if (searchModel.PageIndex == 0)
                searchModel.PageIndex = 1;
            
            var resultData = new List<MovieModel>();
            var allMovies = _movieRepository.Table;
            if (string.IsNullOrEmpty(searchModel.SearchTerm))
                allMovies.Where(x => x.Title.Contains(searchModel.SearchTerm));
            int totalPages = 0,
                 totalRecords = 0;
            if (allMovies.Count() > 0)
            {
                var pagedData = allMovies.Page(searchModel.PageIndex, searchModel.PageSize, out totalPages, out totalRecords);
            
                foreach (var item in pagedData)
                {
                    if (item != null)
                    {
                        var movieModel = _mapper.Map<Movie, MovieModel>(item);
                        if (movieModel != null)
                        {
                            if (item.Showtimes == null || item.Showtimes.Count > 0)
                                item.Showtimes = await GetMovieShowTimeByMovieId(movieModel.Id);

                            var showTimeModels = _mapper.Map<List<MovieShowTime>, List<MovieShowTimeModel>>(item.Showtimes.ToList());
                            movieModel.ShowTimes = showTimeModels;
                        }
                        resultData.Add(movieModel);
                    }
                }
            }
            return new PageListModel<MovieModel>(resultData, searchModel.PageIndex, totalPages, totalRecords);
        }
        #endregion

        #endregion
    }
}