using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using MoviesHome.Models;

namespace MoviesHome
{
    public class Context : DbContext
    {
        public Context() : base("MoviesDatabase")
        {

        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<User> Users { get; set; }
    }
}