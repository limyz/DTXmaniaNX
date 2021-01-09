using DTXMania.Configuration.Settings;
using System;
using System.Data.SQLite;

namespace DTXMania.Configuration
{
    /// <summary>
    /// A database-backed store of <see cref="ISetting{T}"/> values.
    /// </summary>
    public class CConfigurationStore : IDisposable
    {
        /// <summary>
        /// The open connection to this store's database.
        /// </summary>
        private SQLiteConnection connection;

        public CConfigurationStore()
        {
            connection = new SQLiteConnection(@"Data Source=config.db;Version=3;");
            connection.Open();
            tInitialise();
        }

        public void Dispose()
        {
            connection?.Close();
            connection?.Dispose();
            connection = null;
        }

        /// <summary>
        /// Get the current value of the given <see cref="ISetting{T}"/> within this store.
        /// </summary>
        /// <typeparam name="T">The type of the value to get.</typeparam>
        /// <param name="setting">The <see cref="ISetting{T}"/> to get the value of.</param>
        /// <returns>The <typeparamref name="T"/> value of <paramref name="setting"/>, within this store.</returns>
        public T tGet<T>(ISetting<T> setting)
        {
            using (var command = connection.CreateCommand())
            {
                // select the first value for the given setting
                // as the key and category columns are unique constrained there can only be one value per setting
                // and as defaults are set on construction a setting can never not be set
                command.CommandText = @"SELECT Value, Category, Key FROM settings WHERE Category = ($category) AND Key = ($key) LIMIT 1;";
                command.Parameters.AddWithValue(@"$category", setting.eCategory.ToString());
                command.Parameters.AddWithValue(@"$key", setting.strKey);

                using (var reader = command.ExecuteReader())
                {
                    // this should never happen, but throw an exception just in case
                    if (!reader.Read())
                        throw new CMissingSettingRowException(setting.eCategory, setting.strKey);

                    string strValue = reader.GetString(0);
                    return setting.tDecode(strValue);
                }
            }
        }

        /// <summary>
        /// Set the value of the given <see cref="ISetting{T}"/> within this store to the given value.
        /// </summary>
        /// <typeparam name="T">The type of the value to set.</typeparam>
        /// <param name="setting">The <see cref="ISetting{T}"/> to set the value of.</param>
        /// <param name="value">The <typeparamref name="T"/> value to set.</param>
        /// <param name="bReplaceValue">Whether an existing value for <paramref name="setting"/> should be replaced by <paramref name="value"/>, or left as is.</param>
        public void tSet<T>(ISetting<T> setting, T value, bool bReplaceValue = true)
        {
            using (var command = connection.CreateCommand())
            {
                // insert the given value for the given setting
                // the unique constraint on the category and key columns causes a conflict when setting a new value for a setting
                // if the existing value should be replaced, then replace the existing row when this conflict occurs
                // but if it should not, then ignore the insertion
                string strCommand;
                if (bReplaceValue)
                    strCommand = @"INSERT OR REPLACE";
                else
                    strCommand = @"INSERT OR IGNORE";

                command.CommandText = $@"{strCommand} INTO Settings(Category, Key, Value) VALUES (($category), ($key), ($value))";
                command.Parameters.AddWithValue(@"$category", setting.eCategory.ToString());
                command.Parameters.AddWithValue(@"$key", setting.strKey);
                command.Parameters.AddWithValue(@"$value", setting.tEncode(value));
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Initialise the parts of this store's database that have not yet been initialised.
        /// </summary>
        private void tInitialise()
        {
            // create tables
            // settings
            using (var command = connection.CreateCommand())
            {
                // create a unique constraint for the category and key columns to only allow one value per-setting
                command.CommandText = @"CREATE TABLE IF NOT EXISTS Settings
                (
                    Category TEXT NOT NULL,
                    Key TEXT NOT NULL,
                    Value TEXT NOT NULL,
                    UNIQUE(category, key)
                );";

                command.ExecuteNonQuery();
            }

            // initialise default settings
            tInitialiseDefaults();
        }

        /// <summary>
        /// Set the value of each available <see cref="ISetting{T}"/> within <see cref="CSetting"/> that has not yet been set within this store to its default value.
        /// </summary>
        private void tInitialiseDefaults()
        {
            // use a transaction to bulk insert
            using (var transaction = connection.BeginTransaction())
            {
                // ...

                transaction.Commit();
            }
        }

        #region Exceptions

        private class CMissingSettingRowException : Exception
        {
            public CMissingSettingRowException(ESettingCategory eCategory, string strKey)
                : base($"Unable to select setting row for category and key. ({eCategory}, {strKey})")
            {
            }
        }

        #endregion
    }
}
