using Android.App;
using MenuLogDemo.CartModel;
using MenuLogDemo.Interface;
using MenuLogDemo.Models.ViewModel;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.ObjectModel;
using System.Linq;
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
            var internetAccess = Connectivity.NetworkAccess;
            if(internetAccess != NetworkAccess.Internet)
            {
                await DisplayAlert("No Internet Connection", "", "OK");
                return;
            }
            else
            {
                var apiKey = "*"; //Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
                var client = new SendGridClient(apiKey);
                var from = new EmailAddress("annoyingthreat@gmail.com", "MenuLogDemo");
                var subject = "New Order";
                var to = new EmailAddress("sanlimbz@yahoo.com.au");
                var content = CartItem.ToString();
                var name = NameText;
                var message = MailHelper.CreateSingleEmail(from, to, subject, content, name);
                await Task.Run(() => client.SendEmailAsync(message).ConfigureAwait(false));
            }
            await Xamarin.Forms.Application.Current.MainPage.Navigation.PushModalAsync(new MainPage());
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