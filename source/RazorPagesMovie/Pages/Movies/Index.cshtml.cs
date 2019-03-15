using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RazorPagesMovie.Models;

namespace RazorPagesMovie.Pages.Movies
{
    public class IndexModel : PageModel
    {
        private readonly RazorPagesMovie.Models.RazorPagesMovieContext _context;

        public IndexModel(RazorPagesMovie.Models.RazorPagesMovieContext context)
        {
            _context = context;
        }

        public IList<Movie> Movie { get;set; }
        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }
        public SelectList Genres { get; set; }
        [BindProperty(SupportsGet = true)]
        public string MovieGenre { get; set; }

        public async Task OnGetAsync()
        {
            var movies = _context.Movie.Select(m => m);
            var genreQuery = _context.Movie
                .Select(m => m.Genre)
                .Distinct()
                .OrderBy(g => g);

            if (!string.IsNullOrEmpty(SearchString))
                movies = movies.Where(s => s.Title.Contains(SearchString));
            if (!string.IsNullOrEmpty(MovieGenre))
            {
                var ors = MovieGenre.Split('|');
                if (ors.Length == 1)
                    movies = movies.Where(s => s.Genre.Equals(ors.Single()));
                else
                    movies = movies.Where(s => ors.Contains(s.Genre));
            }

            Genres = new SelectList(await genreQuery.ToListAsync());
            Movie = await movies.ToListAsync();
        }
    }
}
