using MenuLogDemo.PageModel;
using MenuLogDemo.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MenuLogDemo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PizzaView : ContentPage
    {
        public IList<PizzaDough> PizzaDoughTypes { get; set; }
        public IList<PizzaSauce> PizzaSauceTypes { get; set; }
        public string TappedIconPizza { get; set; }
        public string TappedPizzaName { get; set; }
        public float TappedPizzaPrice { get; set; }
        public PizzaView(PizzaModel tappedItem)
        {
            InitializeComponent();
            PizzaDoughTypes = new ObservableCollection<PizzaDough>
            {
                new PizzaDough { Id = 1, PizzaDoughName = "ThickCrust" },
                new PizzaDough { Id = 2, PizzaDoughName = "DeepPan" },
                new PizzaDough { Id = 3, PizzaDoughName = "FlatBread" },
                new PizzaDough { Id = 4, PizzaDoughName = "GlutenFree" }
            };

            PizzaSauceTypes = new ObservableCollection<PizzaSauce>
            {
                new PizzaSauce { Id = 1, PizzaSauceName = "MenuLogDemoSauce" },
                new PizzaSauce { Id = 1, PizzaSauceName = "Barbecue" }
            };
            TappedIconPizza = tappedItem.PizzaIcon;
            TappedPizzaName = tappedItem.Name;
            TappedPizzaPrice = tappedItem.Price;
            BindingContext = this;
        }
        private void AddToCart(object sender, EventArgs e)
        {
            //Send data to cart page
            Navigation.PopModalAsync();
        }
    }
}