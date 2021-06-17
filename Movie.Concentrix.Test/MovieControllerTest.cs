using AutoMapper;
using Munna.Concentrix.MovieGram.Controllers;
using Munna.Concentrix.MovieGram.Data;
using Munna.Concentrix.MovieGram.Service;
using NUnit.Framework;

namespace Movie.Concentrix.Test
{
    public class MovieControllerTest
    {
        MovieController _controller;
        private  IMovieGramDbContext _dbContext;
        private  IMapper _mapper;
        private  IMovieService _movieService;

        [SetUp]
        public void Initialise()
        {
            
        }

        [Test]
        public void Add_Product_Test()
        {
            Assert.Pass();
        }
    }
}