using MenuLogDemo.CartModel;
using MenuLogDemo.Interface;
using MenuLogDemo.Models.ViewModel;
using Newtonsoft.Json;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MenuLogDemo.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CartPage : ContentPage
    {
        public ObservableCollection<Cart> CartItem { get; set; }
        public string NameText { get; set; }
        public string PhoneText { get; set; }
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
            var internetAccess = Connectivity.NetworkAccess;
            if(internetAccess != NetworkAccess.Internet)
            {
                await DisplayAlert("No Internet Connection", "", "OK");
                return;
            }
            else
            {
                var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
                var client = new SendGridClient(apiKey);
                var from = new EmailAddress("annoyingthreat@gmail.com", "MenuLogDemo");
                var subject = "New Order";
                var to = new EmailAddress("sanlimbz@yahoo.com.au");
                var content = SendOrderEmailBody();
                var message = MailHelper.CreateSingleEmail(from, to, subject, content, "");
                await Task.Run(() => client.SendEmailAsync(message).ConfigureAwait(false));
            }
            await Application.Current.MainPage.Navigation.PushModalAsync(new MainPage());
            var connection = DependencyService.Get<ISQLite>().GetConnection();
            connection.DeleteAll<ItemCartModel>();
            connection.Commit();
            connection.Close();
        }
        public string SendOrderEmailBody()
        {
            var connection = DependencyService.Get<ISQLite>().GetConnection();
            var orderData = connection.Table<ItemCartModel>().ToList();
            float totalCost = 0f;
            List<PlaceOrderModel> orderModel = new List<PlaceOrderModel>();
            for (int i = 0; i < orderData.Count; i++)
            {
                totalCost += (orderData[i].ItemPrice * orderData[i].ItemQuantity);
                orderModel.Add(new PlaceOrderModel()
                {
                    PlacedOrderItemname = orderData[i].ItemName,
                    PlacedOrderQuantity = orderData[i].ItemQuantity,
                    PlacedOrderItemPrice = orderData[i].ItemPrice,
                    PlacedOrderItemDough = orderData[i].ItemDough,
                    PlacedOrderItemSauce = orderData[i].ItemSauce,
                });
                TotalCost = totalCost;
            }
            Order OrderDetails = new Order
            {
                UserName = NameText,
                UserPhone = PhoneText,
                OrderItems = orderModel,
                TotalCost = totalCost
            };
            var content = JsonConvert.SerializeObject(OrderDetails);
            return content;
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
                    UserItemname = item.ItemName,
                    UserItemQuantity = item.ItemQuantity,
                    UserItemPrice = item.ItemPrice,
                    ItemDough = item.ItemDough,
                    ItemSauce = item.ItemSauce
                });
                TotalCost += (item.ItemPrice * item.ItemQuantity);
            }
        }
    }
}
