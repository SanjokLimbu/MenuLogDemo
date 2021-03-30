using MenuLogDemo.CartModel;
using MenuLogDemo.Interface;
using MenuLogDemo.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MenuLogDemo.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CartPage : ContentPage
    {
        public ObservableCollection<Cart> CartItem { get; set; }
        public string NameText { get; set; }
        public int PhoneText { get; set; }
        public Command PlaceOrderCommand { get; set; }
        private float _totalCost;
        public float TotalCost
        {
            get { return _totalCost; }
            set
            {
                _totalCost = value;
                OnPropertyChanged();
            }
            
        }
        public CartPage()
        {
            InitializeComponent();
            CartItem = new ObservableCollection<Cart>();
            LoadItems();
            PlaceOrderCommand = new Command(async () => await PlaceOrderAsync());
            BindingContext = this;
        }

        private async Task PlaceOrderAsync()
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(new MainPage());
        }

        private void LoadItems()
        {
            var connection = DependencyService.Get<ISQLite>().GetConnection();
            var items = connection.Table<ItemCartModel>().ToList();
            CartItem.Clear();
            foreach (var item in items)
            {
                CartItem.Add(new Cart()
                {
                    UserName = NameText,
                    UserNumber = PhoneText,
                    UserItemname = item.ItemName,
                    UserItemQuantity = item.ItemQuantity,
                    userItemPrice = item.ItemPrice,
                    ItemDough = item.ItemDough,
                    ItemSauce = item.ItemSauce
                });
                TotalCost += (item.ItemPrice * item.ItemQuantity); 
            }
        }
    }
}