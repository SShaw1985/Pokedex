using Acr.UserDialogs;
using Pokedex.Interfaces;
using Pokedex.Models;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Pokedex.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private IPokemonService _pokemonService { get; set; }

        private int paging { get; set; }
        private int offset { get; set; }

        private string NextUrl { get; set; }
        private string PreviousUrl { get; set; }

        public bool isRefreshing { get; set; }
        public bool ShowNextButton { get; set; }
        public bool ShowPreviousButton { get; set; }


        public ObservableCollection<PokemonItem> PokemonList { get; set; }
        public ObservableCollection<PokemonItem> PokemonTypes { get; set; }

        private PokemonItem _selectedValue;
        public PokemonItem SelectedFilter { get
            {
                return _selectedValue;
            }
            set
            {
                _selectedValue = value;
                FilterList();
            }
        }

        public PokemonListWrapper PokemonWrapper { get; set; }

        public ICommand GoToItemCommand { get { return new Command(GoToItem); } }
        public ICommand PullToRefreshCommand { get { return new Command(GetData); } }
        public ICommand PaginateCommand { get { return new Command((obj) => { PaginateData(obj); }); } }


        public MainPageViewModel(IAppNavigationService navigationService, IPokemonService pokemonService)
          : base(navigationService)
        {
            _pokemonService = pokemonService;
            paging = 20;
            offset = 0;
        }

        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);


            if (this.PokemonList == null || this.PokemonList.Count == 0)
            {
                GetData();

                var types = await _pokemonService.GetListOfTypes();
                if (types != null)
                {
                    PokemonTypes = new ObservableCollection<PokemonItem>(types.results);
                    PokemonTypes.Insert(0, new PokemonItem() { name = "All", url = "" });
                    PokemonTypes.Insert(1, new PokemonItem() { name = "Favourites", url = "" });
                    this.SelectedFilter = PokemonTypes[0];
                }
            }

            if(SelectedFilter.name== "Favourites")
            {
                this.GetFavouriteData();
            }
        }

        private async void GetData()
        {
            try
            {
                isRefreshing = true;
                UserDialogs.Instance.ShowLoading();

                PokemonWrapper = await _pokemonService.GetPaginatedList(paging, offset);
                this.PokemonList = new ObservableCollection<PokemonItem>(PokemonWrapper?.results);
                this.NextUrl = PokemonWrapper.next;
                this.ShowNextButton = !string.IsNullOrEmpty(this.NextUrl);

            }
            finally
            {
                UserDialogs.Instance.HideLoading();
                isRefreshing = false;
            }
        }


        private async void FilterList()
        {
            try
            {
                if (SelectedFilter.name == "All")
                {
                    GetData();
                }
                else if (SelectedFilter.name == "Favourites")
                {
                    GetFavouriteData();
                }
                else
                {
                    isRefreshing = true;
                    UserDialogs.Instance.ShowLoading();

                    var filtered = await _pokemonService.GetAllTypes(SelectedFilter.url);
                    var list = filtered.Pokemon.Select(c => new PokemonItem() { name = c.PokemonPokemon.Name, url = c.PokemonPokemon.Url.AbsoluteUri });
                    this.PokemonList = new ObservableCollection<PokemonItem>(list);
                    this.ShowNextButton = false;
                    this.ShowPreviousButton = false;
                }

            }
            finally
            {
                UserDialogs.Instance.HideLoading();
                isRefreshing = false;
            }
        }



        private async void GoToItem(object obj)
        {
            if (obj != null)
            {
                UserDialogs.Instance.ShowLoading();
                NavigationParameters paramaters = new NavigationParameters();
                paramaters.Add("url", obj.ToString());
                var res = await _navigationService.NavigateAsync("ViewPokemon", paramaters, true, true);
                UserDialogs.Instance.HideLoading();
            }

        }


        private async void PaginateData(object obj)
        {
            try
            {
                var isNext = bool.Parse(obj.ToString());
                isRefreshing = true;
                UserDialogs.Instance.ShowLoading();
                var wrapper = await _pokemonService.GetPaginatedList(isNext ? NextUrl : PreviousUrl);

                this.PokemonList = new ObservableCollection<PokemonItem>(wrapper?.results);

                this.NextUrl = wrapper.next;
                this.PreviousUrl = wrapper.previous;

                this.ShowNextButton = !string.IsNullOrEmpty(this.NextUrl);
                this.ShowPreviousButton = !string.IsNullOrEmpty(this.PreviousUrl);

            }
            finally
            {
                UserDialogs.Instance.HideLoading();
                isRefreshing = false;
            }
        }


        private async void GetFavouriteData()
        {
            var pokemon = await _pokemonService.GetFavourite();
            var lst = new List<PokemonItem>();
            foreach (var poke in pokemon)
            {
                lst.Add(new PokemonItem()
                    {
                         name=poke.Name,
                         url= "https://pokeapi.co/api/v2/pokemon/"+poke.Id
                    });
            }

            PokemonList = new ObservableCollection<PokemonItem>(lst);
            this.ShowNextButton = false;
            this.ShowPreviousButton = false;

        }

    }
}
