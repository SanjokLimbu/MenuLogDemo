using MenuLogDemo.Helpers;
using MenuLogDemo.Pages;
using System;
using Xamarin.Forms;

namespace MenuLogDemo
{
    public partial class MainPage : TabbedPage
    {
        public MainPage()
        {
            InitializeComponent();
            //Create cart table
            var createTable = new CreateCartTable();
            createTable.CreateTable();
        }
        private void OnClickPushToCartPage(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new CartPage());
        }
    }
}
