using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MoviesHome.Models
{
    public class Movie
    {
        public Movie()
        {

        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }
        
        [Required]
        public string Description { get; set; }

        [Required]
        public int Duration { get; set; }
        
        [Required]
        public double Rating { get; set; }

        [Required]
        public DateTime ReleaseDate { get; set; }
        
        [Required]
        public string Country { get; set; }
        public string Image { get; set; }

        [NotMapped]
        public HttpPostedFileBase ImageFile { get; set; }
    }
}