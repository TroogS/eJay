using System;
using System.Collections.Generic;
using eJay.Model;
using SQLite.Net;
using SQLite.Net.Interop;
using SQLite.Net.Platform.Generic;

namespace eJay.Data
{
    public class Database : SQLiteConnection
    {
        #region Constructors

        public Database(ISQLitePlatform sqlitePlatform, string databasePath, bool storeDateTimeAsTicks = true, IBlobSerializer serializer = null, IDictionary<string, TableMapping> tableMappings = null, IDictionary<Type, string> extraTypeMappings = null, IContractResolver resolver = null) : base(sqlitePlatform, databasePath, storeDateTimeAsTicks, serializer, tableMappings, extraTypeMappings, resolver)
        {
        }

        public Database(ISQLitePlatform sqlitePlatform, string databasePath, SQLiteOpenFlags openFlags, bool storeDateTimeAsTicks = true, IBlobSerializer serializer = null, IDictionary<string, TableMapping> tableMappings = null, IDictionary<Type, string> extraTypeMappings = null, IContractResolver resolver = null) : base(sqlitePlatform, databasePath, openFlags, storeDateTimeAsTicks, serializer, tableMappings, extraTypeMappings, resolver)
        {
        }

        #endregion

        public Database(string databasePath) : base(new SQLitePlatformGeneric(), databasePath)
        {
            CreateTables();
        }

        #region CreateTables

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Creates the tables. </summary>
        ///
        /// <remarks>   Andre Beging, 08.09.2017. </remarks>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        private void CreateTables()
        {
            CreateTable<Person>();
            CreateTable<Transaction>();
        }

        #endregion
    }
}
