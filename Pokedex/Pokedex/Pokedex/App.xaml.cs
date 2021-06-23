using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Prism;
using Prism.Ioc;
using DryIoc;
using Prism.DryIoc;
using System.Threading.Tasks;
using Pokedex.Interfaces;
using Pokedex.Services;
using Pokedex.Views;
using Pokedex.ViewModels;

namespace Pokedex
{
    public partial class App : PrismApplication
    {
        public App(IPlatformInitializer initializer = null) : base(initializer)
        {
        }


        protected override async void OnInitialized()
        {
            InitializeComponent();
            Akavache.Registrations.Start("GoRestXamarin");

            if (Current.Resources == null)
            {
                Current.Resources = new ResourceDictionary();
            }



            try
            {
                // https://stackoverflow.com/a/43563054/5752 - Capture DI/Prism Exceptions
                TaskScheduler.UnobservedTaskException += (sender, e) =>
                {
                    Console.WriteLine("DI PRISM EXCEPTION: " + e.Exception.Message);
                };

                await NavigationService.NavigateAsync($"{nameof(BaseNavigationPage)}/{nameof(MainPage)}");
            }
            catch (Exception e)
            {
                Console.WriteLine("INITIALISE EXCEPTION: " + e.Message);
            }
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IAppNavigationService, AppNavigationService>();
            containerRegistry.Register<IRestService, RestService>();
            containerRegistry.Register<IPokemonService, PokemonService>();

            containerRegistry.RegisterForNavigation<BaseNavigationPage, BaseNavigationPageViewModel>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<ViewPokemon, ViewPokemonViewModel>();
        }

        private void LogUnobservedTaskExceptions()
        {
            TaskScheduler.UnobservedTaskException += (sender, e) =>
            {
                //Container.Resolve<ILoggerFacade>().Log(e.Exception.ToString(), Category.Exception, Priority.None);
            };
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {

        }
    }
}