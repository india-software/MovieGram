using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Munna.Concentrix.MovieGram.Model
{
    public class MovieSearchModel
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string SearchTerm { get; set; }
    }
}
