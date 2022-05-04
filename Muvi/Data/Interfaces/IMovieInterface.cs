using Muvi.Data.Base;
using Muvi.Data.ViewModels;
using Muvi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Muvi.Data.Interfaces
{
    public interface IMovieInterface : IGenericService<Movie>
    {
        Task<Movie> GetMovieById(int id);
        Task<NewMovieDropdownsVM> GetNewMovieDropdownsValues();
        Task AddMovie(NewMovieVM data);
        Task UpdateMovie(NewMovieVM data);
    }
}
