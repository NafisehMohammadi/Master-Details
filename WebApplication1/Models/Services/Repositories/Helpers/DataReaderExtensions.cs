
using System.Data.Common;

namespace MasterDetail.Repositories.Helpers
{
    public static class DataReaderExtensions
    {
        public static Guid GetGuid(
            this DbDataReader reader,
            string columnName)
        {
            return reader.GetGuid(
                reader.GetOrdinal(columnName));
        }

        public static string GetString(
            this DbDataReader reader,
            string columnName)
        {
            return reader.GetString(
                reader.GetOrdinal(columnName));
        }

        public static decimal GetDecimal(
            this DbDataReader reader,
            string columnName)
        {
            return reader.GetDecimal(
                reader.GetOrdinal(columnName));
        }

        public static DateTime GetDateTime(
            this DbDataReader reader,
            string columnName)
        {
            return reader.GetDateTime(
                reader.GetOrdinal(columnName));
        }

        public static string? GetNullableString(
            this DbDataReader reader,
            string columnName)
        {
            int ordinal = reader.GetOrdinal(columnName);

            if (reader.IsDBNull(ordinal))
                return null;

            return reader.GetString(ordinal);
        }
    }
}