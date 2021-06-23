using Pokedex.Models;
using Pokedex.ViewModels;
using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Pokedex.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewPokemon : ContentPage
    {
        public ViewPokemon()
        {

            InitializeComponent();
            MessagingCenter.Subscribe<ViewPokemonViewModel, List<TypeObject>>(this, "TypesUpdated", (sender, arg) =>
            {
                this.Types.Children.Clear();

                foreach(var obj in arg)
                {
                    Frame frm = new Frame()
                    {
                        BackgroundColor = obj.BackgroundColor,
                        HorizontalOptions = LayoutOptions.Center,
                        CornerRadius = 15,
                        Padding = new Thickness(20, 10, 20, 10),
                         Content= new Label()
                         {
                             Text = obj.TypeName,
                             TextColor = Xamarin.Forms.Color.White
                         },
                         HasShadow=false

                    };
                    this.Types.Children.Add(frm);
                    this.Types.Children.Add(new BoxView() { BackgroundColor = Xamarin.Forms.Color.Transparent, WidthRequest = 15 });
                }

            });
        }


    }
}