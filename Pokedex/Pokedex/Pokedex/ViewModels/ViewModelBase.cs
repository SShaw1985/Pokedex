using Pokedex.Interfaces;
using Prism;
using Prism.AppModel;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pokedex.ViewModels
{

    public class ViewModelBase : BindableBase, IActiveAware, INavigationAware, IDestructible, IConfirmNavigation, IConfirmNavigationAsync, IApplicationLifecycleAware, IPageLifecycleAware
    {
        protected IAppNavigationService _navigationService { get; }

        public ViewModelBase(IAppNavigationService navigationService)
        {
            _navigationService = navigationService;
        }


        public bool IsBusy { get; set; }

        public bool IsNotBusy { get; set; }


        private void OnIsBusyChanged() => IsNotBusy = !IsBusy;

        private void OnIsNotBusyChanged() => IsBusy = !IsNotBusy;

        #region IActiveAware

        public bool IsActive { get; set; }

        public event EventHandler IsActiveChanged;

        private void OnIsActiveChanged()
        {
            IsActiveChanged?.Invoke(this, EventArgs.Empty);

            if (IsActive)
            {
                OnIsActive();
            }
            else
            {
                OnIsNotActive();
            }
        }

        protected virtual void OnIsActive() { }

        protected virtual void OnIsNotActive() { }

        #endregion IActiveAware

        #region INavigationAware

        public virtual void OnNavigatingTo(INavigationParameters parameters) { }

        public virtual void OnNavigatedTo(INavigationParameters parameters) { }

        public virtual void OnNavigatedFrom(INavigationParameters parameters) { }

        #endregion INavigationAware

        #region IDestructible

        public virtual void Destroy() { }

        #endregion IDestructible

        #region IConfirmNavigation

        public virtual bool CanNavigate(INavigationParameters parameters) => true;

        public virtual Task<bool> CanNavigateAsync(INavigationParameters parameters) =>
            Task.FromResult(CanNavigate(parameters));

        #endregion IConfirmNavigation

        #region IApplicationLifecycleAware

        public virtual void OnResume() { }

        public virtual void OnSleep() { }

        #endregion IApplicationLifecycleAware

        #region IPageLifecycleAware

        public virtual void OnAppearing() { }

        public virtual void OnDisappearing() { }

        Task<bool> IConfirmNavigationAsync.CanNavigateAsync(INavigationParameters parameters)=>
            Task.FromResult(CanNavigate(parameters));

        #endregion IPageLifecycleAware
    }
}