using SQLite;

namespace MenuLogDemo.Interface
{
    public interface ISQLite
    {
        SQLiteConnection GetConnection();
    }
}
