using SQLite;

namespace ServerSyncMobileApp.Persistence
{
    public interface ISQLiteDb
    {
        SQLiteAsyncConnection GetConnection();
    }
}