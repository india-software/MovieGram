using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Munna.Concentrix.MovieGram.Model
{
    public class MovieModel : BaseModel
    {
       

        [Required, MaxLength(250)]
        public string Title { get; set; }

        [Required,MaxLength(1000)]
        public string Description { get; set; }      

        public string ImageUrl { get; set; }

        public IEnumerable<MovieShowTimeModel> ShowTimes { get; set; }

        public MovieModel()
        {
            this.ShowTimes = new List<MovieShowTimeModel>();
        }
    }
    public class ImageUpload {
        [Required]
        public long MovieId { get; set; }
        [Required]
        public IFormFile ImagePic { get; set; }
    }
}
