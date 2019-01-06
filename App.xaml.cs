using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Xml;
using System.Xml.Serialization;
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
        #region Private Properties

        private static string _settingsPath;
        private static string _settingsFileName;

        #endregion

        public App()
        {
            _settingsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "eJay", "config");
            _settingsFileName = "eJay.xml";
        }

        #region Locator

        private static ViewModelLocator _locator;
        public static ViewModelLocator Locator => _locator ?? (_locator = new ViewModelLocator());

        #endregion

        #region Database

        private static SQLiteConnection _database;
        public static SQLiteConnection Database => _database ?? (_database = new Database(Settings.Database));

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

        #region Settings

        private static EJaySettings _settings;

        public static EJaySettings Settings
        {
            get
            {
                if(_settings == null)
                    _settings = LoadSettings();

                return _settings;
            }
        }

        #endregion

        #region SaveSettings()

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Saves the settings. </summary>
        /// <remarks>   Andre Beging, 06.01.2019. </remarks>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public static void SaveSettings()
        {
            Directory.CreateDirectory(_settingsPath);

            if (_settings == null) _settings = new EJaySettings();
            using (var streamWriter = new StreamWriter(Path.Combine(_settingsPath, _settingsFileName)))
            {
                using (var xmlWriter = XmlWriter.Create(streamWriter))
                {
                    var serializer = new XmlSerializer(typeof(EJaySettings));
                    serializer.Serialize(xmlWriter, _settings);
                    streamWriter.Flush();
                }
            }
        }

        #endregion

        #region LoadSettings()

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Loads the settings. </summary>
        /// <remarks>   Andre Beging, 06.01.2019. </remarks>
        /// <returns>   The settings. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        private static EJaySettings LoadSettings()
        {
            var targetFile = Path.Combine(_settingsPath, _settingsFileName);
            if (File.Exists(targetFile))
            {
                using (var stream = File.OpenRead(targetFile))
                {
                    var serializer = new XmlSerializer(typeof(EJaySettings));
                    return serializer.Deserialize(stream) as EJaySettings;
                }
            }

            return new EJaySettings();
        }

        #endregion
    }
}
