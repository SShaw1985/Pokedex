using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pokedex.Models
{
    public partial class FilteredPokemonListWrapper
    {

        [JsonProperty("pokemon")]
        public PokemonListItem[] Pokemon { get; set; }
    }

    public partial class Generation
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("url")]
        public Uri Url { get; set; }
    }

    public partial class PokemonListItem
    {
        [JsonProperty("pokemon")]
        public Generation PokemonPokemon { get; set; }

        [JsonProperty("slot")]
        public long Slot { get; set; }
    }
}

