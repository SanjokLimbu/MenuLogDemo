using MenuLogDemo.Interface;
using SQLite;
using System.IO;
using Xamarin.Forms;

[assembly: Dependency(typeof(MenuLogDemo.Droid.SQLite.SQLite_Android))]
namespace MenuLogDemo.Droid.SQLite
{
    public class SQLite_Android : ISQLite
    {
        public SQLiteConnection GetConnection()
        {
            var sqliteFileName = "MenuLogDemo.db3";
            string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var path = Path.Combine(documentsPath, sqliteFileName);
            var connection = new SQLiteConnection(path);
            return connection;
        }
    }
}