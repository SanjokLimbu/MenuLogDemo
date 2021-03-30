using MenuLogDemo.Interface;
using SQLite;
using System;
using System.IO;
using Xamarin.Forms;

[assembly: Dependency(typeof(MenuLogDemo.iOS.SQLite.SQLite_IOS))]
namespace MenuLogDemo.iOS.SQLite
{
    public class SQLite_IOS : ISQLite
    {
        public SQLiteConnection GetConnection()
        {
            var sqliteFileName = "MenuLogDemo.db3";
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string librarypath = Path.Combine(documentsPath, "..", "Library");
            var path = Path.Combine(librarypath, sqliteFileName);
            var connection = new SQLiteConnection(path);
            return connection;
        }
    }
}