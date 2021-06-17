using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Munna.Concentrix.MovieGram.Core
{
    public class Movie : BaseEntity
    {
        public string Title { get; set; }

        public string Description { get; set; }
        public string ImageUrl { get; set; }

        public virtual ICollection<MovieShowTime> Showtimes { get; set; }

        public Movie()
        {
            Showtimes = new List<MovieShowTime>();
        }
    }
}
