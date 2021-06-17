using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Munna.Concentrix.MovieGram.Core
{
    public class MovieShowTime : BaseEntity
    {
        public Movie Movie { get; set; }
        public long MovieId { get; set; }

        public DateTime StartShowTimeUTC { get; set; }
        public DateTime EndShowTimeUTC { get; set; }

        public MovieShowTime()
        {

        }
    }
}
