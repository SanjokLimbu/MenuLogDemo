using MenuLogDemo.CartModel;
using MenuLogDemo.Helpers;
using MenuLogDemo.Interface;
using MenuLogDemo.Models.ViewModel;
using Newtonsoft.Json;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
                var htmlContent = SendHtmlContent();
                var message = MailHelper.CreateSingleEmail(from, to, subject, "", htmlContent);
                await Task.Run(() => client.SendEmailAsync(message).ConfigureAwait(false));
            }
            await DisplayAlert("Order Sent", "Thank you for your patience.", "OK");
            await Navigation.PopModalAsync();
            var connection = DependencyService.Get<ISQLite>().GetConnection();
            connection.DeleteAll<ItemCartModel>();
            connection.Commit();
            connection.Close();
        }
        public string SendHtmlContent()
        {
            var connection = DependencyService.Get<ISQLite>().GetConnection();
            var orderData = connection.Table<ItemCartModel>().ToList();
            float totalCost = 0f;
            for (int i = 0; i < orderData.Count; i++)
            {
                totalCost += (orderData[i].ItemPrice * orderData[i].ItemQuantity);
                TotalCost = totalCost;
            }
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("Name\t{0}\tPhone Number\t{1}\tTotal Cost\t{2}\n", NameText, PhoneText, TotalCost);
            stringBuilder.AppendFormat("\t\t<th>\n");
            stringBuilder.AppendFormat("\t\t\tName");
            stringBuilder.AppendFormat("\t\t\tQuantity");
            stringBuilder.AppendFormat("\t\t\tPrice");
            stringBuilder.AppendFormat("\t\t\tDough");
            stringBuilder.AppendFormat("\t\t\tSauce");
            stringBuilder.AppendFormat("\t\t</th>\n");
            using (HtmlContent.Table table = new HtmlContent.Table(stringBuilder))
            {
                foreach(var items in orderData)
                {
                    using(HtmlContent.Row row = table.AddRow())
                    {
                        row.AddCell(items.ItemName);
                        row.AddCell(items.ItemQuantity.ToString());
                        row.AddCell(items.ItemPrice.ToString());
                        row.AddCell(items.ItemDough);
                        row.AddCell(items.ItemSauce);
                    }
                }
            }
            return stringBuilder.ToString();
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
