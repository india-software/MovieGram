using AutoMapper;
using Munna.Concentrix.MovieGram.Core;
using Munna.Concentrix.MovieGram.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Munna.Concentrix.MovieGram
{
    public class MovieMapperProfile : Profile
    {
        public MovieMapperProfile()
        {
            CreateMap<Movie, MovieModel>()
                .ForMember(x => x.ShowTimes, y => y.Ignore());
            CreateMap<MovieModel, Movie>()
                .ForMember(x => x.Showtimes, y => y.Ignore());
            CreateMap<MovieShowTime, MovieShowTimeModel>();
            CreateMap<MovieShowTimeModel, MovieShowTime>().ForMember(x => x.Movie, y => y.Ignore());

        }
    }
}
