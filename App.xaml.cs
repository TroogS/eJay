using System.Data.SqlClient;
using System.Windows;
using DebtMgr.Data;
using DebtMgr.ViewModel;
using SQLite.Net;

namespace DebtMgr
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : Application
    {
        #region Locator

        private static ViewModelLocator _locator;
        public static ViewModelLocator Locator => _locator ?? (_locator = new ViewModelLocator());

        #endregion

        #region Database

        private static SQLiteConnection _database;
        public static SQLiteConnection Database => _database ?? (_database = new Database(DebtMgr.Properties.Settings.Default.Database));

        #endregion

        #region DatabasePath

        private static string _databasePath;
        public static string DatabasePath
        {
            get => _databasePath;
            set
            {
                _database = null;
                _databasePath = value;
            }
        }

        #endregion
    }
}
