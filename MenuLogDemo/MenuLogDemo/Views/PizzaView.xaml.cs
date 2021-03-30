using MenuLogDemo.CartModel;
using MenuLogDemo.Helpers;
using MenuLogDemo.Interface;
using MenuLogDemo.PageModel;
using MenuLogDemo.Pages;
using MenuLogDemo.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        public ItemCartModel DataToSend { get; set; }
        private int _quantity;
        public int Quantity 
        {
            get { return _quantity; }
            set { 
                this._quantity = value;
                if (this._quantity < 0)
                    this._quantity = 1;
                if (this._quantity > 500)
                {
                    this._quantity = 500;
                    DisplayAlert("Alert", "Quantity should be number between 1 to 500", "Ok");
                }
                OnPropertyChanged();
                }
        }
        public PizzaView(PizzaModel tappedItem)
        {
            InitializeComponent();
            PizzaDoughTypes = new List<PizzaDough>
            {
                new PizzaDough { Id = 1, PizzaDoughName = "ThickCrust" },
                new PizzaDough { Id = 2, PizzaDoughName = "DeepPan" },
                new PizzaDough { Id = 3, PizzaDoughName = "FlatBread" },
                new PizzaDough { Id = 4, PizzaDoughName = "GlutenFree" }
            };

            PizzaSauceTypes = new List<PizzaSauce>
            {
                new PizzaSauce { Id = 1, PizzaSauceName = "MenuLogDemoSauce" },
                new PizzaSauce { Id = 1, PizzaSauceName = "Barbecue" }
            };
            TappedIconPizza = tappedItem.PizzaIcon;
            TappedPizzaName = tappedItem.Name;
            TappedPizzaPrice = tappedItem.Price;
            _quantity = 1;

            BindingContext = this;
        }
        private void AddToCart(object sender, EventArgs e)
        {
            //Send data to cart page
            var selectedDoughItem = PizzaDoughEntry.SelectedItem as PizzaDough;
            var selectedSauceItem = PizzaSauceEntry.SelectedItem as PizzaSauce;
            var connection = DependencyService.Get<ISQLite>().GetConnection();
            try
            {
                ItemCartModel ItemInCart = new ItemCartModel()
                {
                    ItemName = TappedPizzaName,
                    ItemDough = selectedDoughItem.PizzaDoughName.ToString(),
                    ItemSauce = selectedSauceItem.PizzaSauceName.ToString(),
                    ItemQuantity = Quantity,
                    ItemPrice = TappedPizzaPrice
                };
                //Create cart table
                var createTable = new CreateCartTable();
                createTable.CreateTable();
                //Find item in that table created above
                var item = connection.Table<ItemCartModel>().ToList().FirstOrDefault();
                if (item == null)
                    connection.Insert(ItemInCart);
                connection.Commit();
                connection.Close();
                DisplayAlert("Cart", "Item added to cart.", "Ok");
                Navigation.PopModalAsync();
            }
            catch(Exception ex)
            {
                DisplayAlert("Error", ex.Message, "Ok");
            }
            finally
            {
                connection.Close();
            }
            
        }
    }
}