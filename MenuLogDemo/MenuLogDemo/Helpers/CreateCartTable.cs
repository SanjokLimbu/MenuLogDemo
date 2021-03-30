using MenuLogDemo.CartModel;
using MenuLogDemo.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MenuLogDemo.Helpers
{
    public class CreateCartTable
    {
        public bool CreateTable()
        {
            try
            {
                var connection = DependencyService.Get<ISQLite>().GetConnection();
                connection.CreateTable<ItemCartModel>();
                connection.Close();
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }
    }
}
