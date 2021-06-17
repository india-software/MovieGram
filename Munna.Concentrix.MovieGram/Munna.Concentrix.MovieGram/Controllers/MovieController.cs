using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Munna.Concentrix.MovieGram.Core;
using Munna.Concentrix.MovieGram.Model;
using Munna.Concentrix.MovieGram.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Munna.Concentrix.MovieGram.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly IMovieService _movieService;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IMapper _mapper;
        public MovieController(IMovieService movieService, 
            IMapper mapper, IWebHostEnvironment hostingEnvironment)
        {
            _movieService = movieService;
            _hostingEnvironment = hostingEnvironment;
            _mapper = mapper;
        }
        // GET: api/<ValuesController>
        /// <summary>
        /// Get all movies
        /// </summary>
        /// <returns></returns>
        [HttpGet]        
        public async Task<IActionResult> Get()
        {
            var movies = await _movieService.SearchMovies(new MovieSearchModel());
            if (movies == null || movies.Count == 0)
                return NotFound();
            return Ok(movies);
        }
        /// <summary>
        /// Search Prodcut Based On Search Term
        /// </summary>
        /// <param name="movieSearchModel"></param>
        /// <returns></returns>
        [HttpGet, Route("SearchMovie/{PageIndex:long}/{PageSize:long}/{SearchTerm}")]
        public async Task<IActionResult> SearchMovie(MovieSearchModel movieSearchModel)
        {
            var movies = await _movieService.SearchMovies(movieSearchModel);
            if (movies == null || movies.Count == 0)
                return NotFound();
            return Ok(movies);
        }

        // GET api/<ValuesController>/5
        /// <summary>
        /// Get Movie by movie id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            var movie = await _movieService.GetMovieById(id);
            if (movie == null)
                return NoContent();
            else
            {
                var movieModel = _mapper.Map<MovieModel>(movie);
                if (movie.Showtimes == null)
                    movie.Showtimes = await _movieService.GetMovieShowTimeByMovieId(id);
                movieModel.ShowTimes = _mapper.Map<List<MovieShowTimeModel>>(movie.Showtimes);
                return Ok(movieModel);
            }
        }
        /// <summary>
        /// Create Movie
        /// </summary>
        /// <param name="movieModel"></param>
        /// <returns></returns>
        // POST api/<ValuesController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] MovieModel movieModel)
        {
            var movie = _mapper.Map<Movie>(movieModel);            
            await _movieService.InsertMovie(movie);
            movieModel.Id = movie.Id;
            if(movieModel.ShowTimes!=null && movieModel.ShowTimes.Count() > 0 && movie.Id>0)
            {
                var movieShowTime = _mapper.Map<List<MovieShowTime>>(movieModel.ShowTimes);
                if (movieShowTime != null)
                {
                    foreach(var item in movieShowTime)
                    {
                        item.MovieId = movie.Id;
                       await _movieService.InsertMovieShowTime(item);
                    }
                }
            }
            return Ok(movieModel);
        }
        /// <summary>
        /// Upload Image to movie  and pass IformFile and Product Id
        /// </summary>
        /// <param name="movieModel"></param>
        /// <returns></returns>
        [HttpPost,Route("UploadMovieImage")]
        public async Task<IActionResult> UploadMovieImage([FromForm] ImageUpload movieModel)
        {
            var movie = await _movieService.GetMovieById(movieModel.MovieId);
            if (movie == null)
                return NoContent();
            if (movieModel.ImagePic != null && movieModel.ImagePic.Length > 0)
            {
                string fileName = Guid.NewGuid() + "--" + movieModel.ImagePic.FileName;
                string filePath = $"{ _hostingEnvironment.WebRootPath}\\Upload\\{fileName}";
                using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await movieModel.ImagePic.CopyToAsync(fileStream);
                }
                movie.ImageUrl = $"/Upload/{fileName}";
            }
            await _movieService.UpdateMovie(movie);
          
            return Ok(movie);
        }
        /// <summary>
        /// Update Movie
        /// </summary>
        /// <param name="id"></param>
        /// <param name="movieModel"></param>
        /// <returns></returns>
        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id, [FromBody] MovieModel movieModel)
        {
            var movie = await _movieService.GetMovieById(id);
            if (movie == null)
                return NotFound();

            movie = _mapper.Map<Movie>(movieModel);
            movie.Id = id;
            await _movieService.UpdateMovie(movie);

            return Ok(movieModel);
        }
        /// <summary>
        /// Delete Movie
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var movie = await _movieService.GetMovieById(id);
            if (movie == null)
                return NotFound();
            await _movieService.DeleteMovie(movie);
            return Ok();
        }
        /// <summary>
        /// Add Show Time In Movie
        /// </summary>
        /// <param name="movieShowTimeModel"></param>
        /// <returns></returns>
        [HttpPost, Route("AddShowTime")]
        public async Task<IActionResult> AddShowTime(MovieShowTimeModel movieShowTimeModel)
        {
            var showTime = _mapper.Map<MovieShowTime>(movieShowTimeModel);
            await _movieService.InsertMovieShowTime(showTime);
            movieShowTimeModel.Id = showTime.Id;
            return Ok(movieShowTimeModel);
        }
        /// <summary>
        /// Edit Movie Show Time
        /// </summary>
        /// <param name="movieShowTimeModel"></param>
        /// <returns></returns>
        [HttpPost, Route("EditShowTime")]
        public async Task<IActionResult> EditShowTime(MovieShowTimeModel movieShowTimeModel)
        {
            var showTime = _mapper.Map<MovieShowTime>(movieShowTimeModel);
            await _movieService.UpdateMovieShowTime(showTime);
            movieShowTimeModel.Id = showTime.Id;
            return Ok(movieShowTimeModel);
        }
        /// <summary>
        /// Delete Movie Show Time
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet, Route("DeleteShowTime")]
        public async Task<IActionResult> DeleteShowTime(long id)
        {
            var movieShowTime = await _movieService.GetMovieShowTimeById(id);
            if (movieShowTime == null)
                return NotFound();
            await _movieService.DeleteMovieShowTime(movieShowTime);
            return Ok();
        }
        /// <summary>
        /// Get list of show time based on product id
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpGet, Route("GetAllShowTimeByMovieId/{productId:long}")]
        public async Task<IActionResult> GetAllShowTimeByMovieId(long productId)
        {
            var movieShowTimes = await _movieService.GetMovieShowTimeByMovieId(productId);
            if (movieShowTimes == null)
                return NotFound();
            var movieShowTimeList = _mapper.Map<List<MovieShowTimeModel>>(movieShowTimes);

            return Ok(movieShowTimeList);
        }
    }
}
