using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Pokedex.Models
{
    public class PokemonListWrapper : INotifyPropertyChanged
    {
        public int count { get; set; }
        public string next { get; set; }
        public string previous { get; set; }
        public List<PokemonItem> results { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
