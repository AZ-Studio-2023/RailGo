using Microsoft.Data.Sqlite;

namespace RailGo.Core.OfflineQuery;

public static class SqliteExtensions
{
    public static bool FieldExists(this SqliteDataReader reader, string fieldName)
    {
        for (int i = 0; i < reader.FieldCount; i++)
        {
            if (reader.GetName(i).Equals(fieldName, StringComparison.OrdinalIgnoreCase))
                return true;
        }
        return false;
    }
}