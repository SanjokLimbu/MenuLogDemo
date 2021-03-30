using MenuLogDemo.PageModel;
using MenuLogDemo.Views;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MenuLogDemo.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PizzaPage : ContentPage
    {
        public IList<PizzaModel> PizzaDetails { get; set; }
        public PizzaPage()
        {
            InitializeComponent();
            PizzaDetails = new List<PizzaModel>
            {
                new PizzaModel{PizzaIcon="PizzaPeporini.png", Name="Peporini Pizza", Price = 5.0f},
                new PizzaModel{PizzaIcon="PizzaChicken.png", Name="Chicken Pizza", Price = 6.0f},
                new PizzaModel{PizzaIcon="PizzaPrawn.jpg", Name="Prawn Pizza", Price = 8.0f}
            };
            BindingContext = this;
        }
        private void OnTappedPizza(object sender, ItemTappedEventArgs e)
        {
            if(e.Item == null)
            {
                return;
            }
            var tappedItem = (PizzaModel)e.Item;
            Navigation.PushModalAsync(new PizzaView(tappedItem));
        }
    }
}