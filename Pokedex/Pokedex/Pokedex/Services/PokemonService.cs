using Pokedex.Interfaces;
using Pokedex.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Reactive.Linq;
using Akavache;
using System.Linq;

namespace Pokedex.Services
{
    public class PokemonService :IPokemonService
    {
        private IRestService _rest { get; set; }

        public PokemonService(IRestService rest)
        {
            this._rest = rest;
            BlobCache.ApplicationName = "PokeDexCache";
        }

        public async Task<PokemonListWrapper> GetPaginatedList(int pageSize, int offSet)
        {
            var url = string.Format("pokemon/?limit={0}&offset={1}", pageSize, offSet);

            var cache = await GetFromCache<PokemonListWrapper>(url);
            if (cache != default(PokemonListWrapper))
                return cache;

            var resp = await _rest.GetAsync(url);

            if (resp.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var Pokemon = JsonConvert.DeserializeObject<PokemonListWrapper>(resp.Content);
                return Pokemon;
            }
            else
            {
                return null;
            }
        }

        public async Task<PokemonListWrapper> GetPaginatedList(string url)
        {
           
            var resp = await _rest.GetAsync(url);

            if (resp.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var Pokemon = JsonConvert.DeserializeObject<PokemonListWrapper>(resp.Content);
                return Pokemon;
            }
            else
            {
                return null;
            }
        }


        public async Task<PokemonListWrapper> GetListOfTypes()
        {
            var cache = await GetFromCache<PokemonListWrapper>("Types");
            if (cache != default(PokemonListWrapper))
                return cache;

            var resp = await _rest.GetAsync("type");

            if (resp.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var Pokemon = JsonConvert.DeserializeObject<PokemonListWrapper>(resp.Content);

                await BlobCache.LocalMachine.InsertObject<PokemonListWrapper>("Types", Pokemon);
                return Pokemon;
            }
            else
            {
                return null;
            }
        }

        public async Task<Pokemon> GetPokemonDetails(string url)
        {
            var cache = await GetFromCache<Pokemon>(url);
            if (cache != default(Pokemon))
                return cache;

            var resp = await _rest.GetAsync(url);

            if (resp != null && resp.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var Pokemon = JsonConvert.DeserializeObject<Pokemon>(resp.Content);

                await BlobCache.LocalMachine.InsertObject<Pokemon>(url, Pokemon);

                return Pokemon;
            }
            else
            {
                return null;
            }
        }


        public async Task<PokemonSpecies> GetPokemonSpecies(string url)
        {
            var cache = await GetFromCache<PokemonSpecies>(url);
            if (cache != default(PokemonSpecies))
                return cache;
           
            var resp = await _rest.GetAsync(url);

            if (resp != null && resp.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var Pokemon = JsonConvert.DeserializeObject<PokemonSpecies>(resp.Content);

                // store the pokemon under the api url
                await BlobCache.LocalMachine.InsertObject<PokemonSpecies>(url, Pokemon);

                return Pokemon;
            }
            else
            {
                return null;
            }
        }

        public async Task<FilteredPokemonListWrapper> GetAllTypes(string url)
        {
            var cache = await GetFromCache<FilteredPokemonListWrapper>(url);
            if (cache != default(FilteredPokemonListWrapper))
                return cache;
            var resp = await _rest.GetAsync(url);

            if (resp.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var Pokemon = JsonConvert.DeserializeObject<FilteredPokemonListWrapper>(resp.Content);

                await BlobCache.LocalMachine.InsertObject<FilteredPokemonListWrapper>(url, Pokemon);
                return Pokemon;
            }
            else
            {
                return null;
            }
        }


        public async Task<bool> GetIsFavourite(Pokemon ThisPokemon)
        {
            var lst = await GetFromCache<List<Pokemon>>("Favourites");

            return (lst!=default(List<Pokemon>) && lst.Any(c=>c.Id==ThisPokemon.Id));
        }


        public async Task<List<Pokemon>> GetFavourite()
        {
            return await GetFromCache<List<Pokemon>>("Favourites");
        }
        public async Task<bool> SetFavourite(Pokemon ThisPokemon)
        {
            try
            {
                bool retVal = false;
                var lst = await GetFromCache<List<Pokemon>>("Favourites");

                if (lst!=null && lst.Any(c=>c.Id==ThisPokemon.Id))
                {
                    var value = lst.FirstOrDefault(c=>c.Id==ThisPokemon.Id);
                    lst.Remove(value);
                    retVal = false;
                }
                else
                {
                    if (lst == null)
                        lst = new List<Pokemon>();
                    lst.Add(ThisPokemon);
                    retVal = true;
                }
                await BlobCache.LocalMachine.InvalidateObject<List<Pokemon>>("Favourites");
                await BlobCache.LocalMachine.InsertObject<List<Pokemon>>("Favourites", lst);
                return retVal;
            }
            catch(Exception ex)
            {
            }

            return false;
        }

        private async Task<T> GetFromCache<T>(string key)
        {
            try
            {
                var question = await BlobCache.LocalMachine.GetObject<T>(key);

                if (question != null)
                    return question;
            }
            catch (Exception ex)
            {

            }

            return default(T);
        }

    }
}
