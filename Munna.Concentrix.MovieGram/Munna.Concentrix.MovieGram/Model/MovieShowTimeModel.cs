using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Munna.Concentrix.MovieGram.Model
{
    public class MovieShowTimeModel : BaseModel
    {
        public int MovieId { get; set; }

        [Required]
        public DateTime StartShowTime { get; set; }
        [Required]
        public DateTime EndShowTime { get; set; }
    }
}
