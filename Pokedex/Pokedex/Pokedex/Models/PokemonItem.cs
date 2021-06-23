using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace Pokedex.Models
{
    public class PokemonItem : INotifyPropertyChanged
    {
        public string name { get; set; }

        public string url { get; set; }

        public ImageSource image { get
            {
                var id = this.url.Replace("https://pokeapi.co/api/v2/pokemon/", "");
                id = id.Replace("/", "");
                return string.Format("https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/{0}.png",id);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
