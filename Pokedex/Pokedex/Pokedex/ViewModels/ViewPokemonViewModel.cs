using Acr.UserDialogs;
using Pokedex.Interfaces;
using Pokedex.Models;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Color = Xamarin.Forms.Color;

namespace Pokedex.ViewModels
{
    public class ViewPokemonViewModel : ViewModelBase
    {
        private IPokemonService _pokemonService { get; set; }


        public Pokemon ThisPokemon { get; set; }
        public PokemonSpecies SpeciesDetails { get; set; }
        public ObservableCollection<string> Images { get; set; }

        public List<TypeObject> Types { get; set; }
        
        public ICommand CloseCommand { get { return new Command(Close); } }
        public ICommand SetFavouriteCommand { get { return new Command(SetFavourite); } }

        public bool IsFavourite { get; set; }
        public string Description { get; set; }

        public Color BackgroundColorBasedOnType { get; set; }
        public ImageSource FavoutiteImage{ get; set; }


        public ViewPokemonViewModel(IAppNavigationService navigationService, IPokemonService pokemonService)
          : base(navigationService)
        {
            _pokemonService = pokemonService;
        }

        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            GetData(parameters);          
        }

        private async void GetData(INavigationParameters parameters)
        {
            try
            {
                UserDialogs.Instance.ShowLoading();

                var url = parameters.GetValue<string>("url");
                if (!string.IsNullOrEmpty(url))
                {
                    this.ThisPokemon = await _pokemonService.GetPokemonDetails(url);
                    this.SpeciesDetails = await _pokemonService.GetPokemonSpecies("https://pokeapi.co/api/v2/pokemon-species/"+this.ThisPokemon.Id);
                    
                    this.Description = this.SpeciesDetails?.FlavorTextEntries.FirstOrDefault()?.FlavorText;
                    
                    if (!string.IsNullOrEmpty(Description))
                        this.Description = Description.Replace("\f", " ").Replace("\r", " ").Replace("\n", " ");

                    this.BackgroundColorBasedOnType = GetBackgroundColorForType(ThisPokemon.Types.FirstOrDefault().Type.Name);

                    this.Types = new List<TypeObject>();
                    foreach(var type in ThisPokemon.Types.Select(c => c.Type.Name).ToList())
                    {
                        this.Types.Add( new TypeObject()
                        {
                            TypeName = type,
                            BackgroundColor = GetBackgroundColorForType(type.ToLower())
                        });
                    }

                    MessagingCenter.Send<ViewPokemonViewModel, List<TypeObject>>(this, "TypesUpdated", Types);

                   
                    this.Images = new ObservableCollection<string>();
                    if(ThisPokemon.Sprites.FrontDefault !=null)
                        this.Images.Add(ThisPokemon.Sprites.FrontDefault?.AbsoluteUri);
                    if (ThisPokemon.Sprites.FrontFemale != null)
                        this.Images.Add(ThisPokemon.Sprites.FrontFemale?.AbsoluteUri);
                    if (ThisPokemon.Sprites.FrontShiny != null)
                        this.Images.Add(ThisPokemon.Sprites.FrontShiny?.AbsoluteUri);
                    if (ThisPokemon.Sprites.FrontShinyFemale != null)
                        this.Images.Add(ThisPokemon.Sprites.FrontShinyFemale?.AbsoluteUri);
                    if (ThisPokemon.Sprites.BackDefault != null)
                        this.Images.Add(ThisPokemon.Sprites.BackDefault?.AbsoluteUri);
                    if (ThisPokemon.Sprites.BackFemale != null)
                        this.Images.Add(ThisPokemon.Sprites.BackFemale?.AbsoluteUri);
                    if (ThisPokemon.Sprites.BackShiny != null)
                        this.Images.Add(ThisPokemon.Sprites.BackShiny?.AbsoluteUri);
                    if (ThisPokemon.Sprites.BackShinyFemale != null)
                        this.Images.Add(ThisPokemon.Sprites.BackShinyFemale?.AbsoluteUri);


                    this.FavoutiteImage = await _pokemonService.GetIsFavourite(ThisPokemon) ? "star.png" : "starunselected";
                }

                await Task.Delay(500);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        private Color GetBackgroundColorForType(string type)
        {
            Color retval = Color.White;

            switch(type.ToLower())
            {
                case "normal":
                    retval = Color.FromHex("#A8A878");
                    break;
                case "grass":
                    retval = Color.FromHex("#78C850");
                    break;

                case "fire":
                    retval = Color.FromHex("#F08030");
                    break;

                case "water":
                    retval = Color.FromHex("#6890F0");
                    break;
                case "electric":
                    retval = Color.FromHex("#F8D030");
                    break;
                case "ice":
                    retval = Color.FromHex("#98D8D8");
                    break;
                case "fighting":
                    retval = Color.FromHex("#C03028");
                    break;
                case "poison":
                    retval = Color.FromHex("#A040A0");
                    break;
                case "ground":
                    retval = Color.FromHex("#E0C068");
                    break;


                case "flying":
                    retval = Color.FromHex("#A890F0");
                    break;
                case "psycic":
                    retval = Color.FromHex("#F85888");
                    break;
                case "bug":
                    retval = Color.FromHex("#A8B820");
                    break;
                case "rock":
                    retval = Color.FromHex("#B8A038");
                    break;
                case "ghost":
                    retval = Color.FromHex("#705898");
                    break;
                case "dark":
                    retval = Color.FromHex("#705848");
                    break;
                    
                case "dragon":
                    retval = Color.FromHex("#7038F8");
                    break;
                case "steel":
                    retval = Color.FromHex("#B8B8D0");
                    break;
                case "fairy":
                    retval = Color.FromHex("#F0B6BC");
                    break;
            }

            return retval;
        }

        private async void Close()
        {
            MessagingCenter.Unsubscribe<ViewPokemonViewModel, List<TypeObject>>(this, "TypesUpdated");
            await _navigationService.GoBackAsync(null, true, true);
        }

        private async void SetFavourite()
        {

             this.FavoutiteImage = await _pokemonService.SetFavourite(ThisPokemon)?"star.png":"starunselected";
        }
    }
}
