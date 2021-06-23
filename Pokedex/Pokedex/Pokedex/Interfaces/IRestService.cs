using Pokedex.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pokedex.Interfaces
{
    public interface IRestService
    {
        Task<ResponseWrapper> GetAsync(string uri);
    }
}
