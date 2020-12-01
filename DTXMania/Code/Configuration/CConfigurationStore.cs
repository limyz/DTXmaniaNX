using System;
using System.Data.SQLite;

namespace DTXMania
{
    /// <summary>
    /// A database-backed store of <see cref="ESetting"/> values.
    /// </summary>
    public class CConfigurationStore : IDisposable
    {
        /// <summary>
        /// The open <see cref="SQLiteConnection"/> to this store's database.
        /// </summary>
        private SQLiteConnection connection;

        public CConfigurationStore()
        {
            connection = new SQLiteConnection(@"Data Source=config.db;Version=3;");
            connection.Open();

            tInitialise();
            tSetDefaults();
        }

        public void Dispose()
        {
            connection?.Close();
            connection?.Dispose();
            connection = null;
        }

        /// <summary>
        /// Get the current value of the given <see cref="ESetting"/> within this store.
        /// </summary>
        /// <typeparam name="T">The type of the value to get.</typeparam>
        /// <param name="eSetting">The <see cref="ESetting"/> to get the value of.</param>
        /// <returns>The <typeparamref name="T"/> value of <paramref name="eSetting"/>, within this store.</returns>
        public T tGet<T>(ESetting eSetting) where T : struct, IConvertible
        {
            string strKey = tGetSettingKey(eSetting);
            using (var command = connection.CreateCommand())
            {
                // select the first value for the given setting
                // as the key column is unique constrained there can only be one value per key
                // and as defaults are set on construction a key can never not be set
                command.CommandText = @"SELECT value, key FROM settings WHERE key = ($key) LIMIT 1;";
                command.Parameters.AddWithValue(@"$key", strKey);

                using (var reader = command.ExecuteReader())
                {
                    // this should never happen, but throw an exception just in case
                    if (!reader.Read())
                        throw new CMissingSettingRowException(strKey);

                    string strValue = reader.GetString(0);
                    return (T)Convert.ChangeType(strValue, typeof(T));
                }
            }
        }

        /// <summary>
        /// Set the value of the given <see cref="ESetting"/> within this store to the given value.
        /// </summary>
        /// <typeparam name="T">The type of the value to set.</typeparam>
        /// <param name="eSetting">The <see cref="ESetting"/> to set the value of.</param>
        /// <param name="value">The <typeparamref name="T"/> to set.</param>
        /// <param name="bReplace">Whether an existing value for <paramref name="eSetting"/> should be replaced by <paramref name="value"/>, or left as is.</param>
        public void tSet<T>(ESetting eSetting, T value, bool bReplace = true) where T : struct, IConvertible
        {
            string strCommand;
            if (bReplace)
                strCommand = @"INSERT OR REPLACE";
            else
                strCommand = @"INSERT OR IGNORE";

            string strKey = tGetSettingKey(eSetting);
            string strValue = (string)Convert.ChangeType(value, typeof(string));
            using (var command = connection.CreateCommand())
            {
                // insert the given value for the given setting, possibly replacing an existing row
                command.CommandText = $@"{strCommand} INTO settings(key, value) VALUES (($key), ($value))";
                command.Parameters.AddWithValue(@"$key", strKey);
                command.Parameters.AddWithValue(@"$value", strValue);
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Get the unique <see cref="string"/> key used to identify the given <see cref="ESetting"/>.
        /// </summary>
        /// <param name="eSetting">The <see cref="ESetting"/> to get the key of.</param>
        /// <returns>The unique <see cref="string"/> key used to identify <paramref name="eSetting"/>.</returns>
        private string tGetSettingKey(ESetting eSetting) => Enum.GetName(typeof(ESetting), eSetting);

        /// <summary>
        /// Initialise the parts of this store's database that have not yet been initialised.
        /// </summary>
        private void tInitialise()
        {
            using (var command = connection.CreateCommand())
            {
                // create the settings table
                command.CommandText = @"CREATE TABLE IF NOT EXISTS settings
                (
                    key TEXT UNIQUE NOT NULL,
                    value TEXT NOT NULL
                );";

                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Set each <see cref="ESetting"/> that has not yet been set within this store to its default value.
        /// </summary>
        private void tSetDefaults()
        {
        }

        #region Exceptions

        private class CMissingSettingRowException : Exception
        {
            public CMissingSettingRowException(string strKey)
                : base($"Unable to select setting row for key. ({strKey})")
            {
            }
        }

        #endregion
    }
}
