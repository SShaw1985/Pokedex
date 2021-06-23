using Pokedex.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pokedex.Interfaces
{
    public interface IPokemonService
    {
        Task<PokemonListWrapper> GetPaginatedList(int pageSize, int offSet);
        Task<PokemonListWrapper> GetPaginatedList(string url);

        Task<Pokemon> GetPokemonDetails(string url);
        
        Task<PokemonSpecies> GetPokemonSpecies(string url);
        
        Task<PokemonListWrapper> GetListOfTypes();


        Task<FilteredPokemonListWrapper> GetAllTypes(string url);
        Task<List<Pokemon>> GetFavourite();

        Task<bool> GetIsFavourite(Pokemon ThisPokemon);
        Task<bool> SetFavourite(Pokemon ThisPokemon);
    }
}
