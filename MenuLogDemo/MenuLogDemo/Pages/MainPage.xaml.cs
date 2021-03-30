using MenuLogDemo.Pages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MenuLogDemo
{
    public partial class MainPage : TabbedPage
    {
        public MainPage()
        {
            InitializeComponent(); 
        }
        private void OnClickPushToCartPage(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new CartPage());
        }
    }
}
