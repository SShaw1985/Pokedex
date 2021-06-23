using Pokedex.Models;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pokedex.Interfaces
{
    public interface IAppNavigationService : INavigationService
    {
        Task PopToRootAsync(NavigationParameters parameters);
    }

}
