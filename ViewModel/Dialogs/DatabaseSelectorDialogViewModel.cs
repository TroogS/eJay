using System;
using System.Linq;
using System.Windows;
using eJay.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using SQLite.Net;
using SQLite.Net.Platform.Generic;

namespace eJay.ViewModel.Dialogs
{
    public class DatabaseSelectorDialogViewModel : ViewModelBase
    {
        #region Public Properties

        public event EventHandler RequestClose;
        public bool ProgramRequestedClose;

        #endregion

        #region SelectDatabasePathText (string) Property

        /// <summary>
        /// Privater Teil von <see cref="SelectDatabasePathText" />
        /// </summary>
        private string _selectDatabasePathText;

        /// <summary>
        /// Comment
        ///</summary>
        public string SelectDatabasePathText
        {
            get { return _selectDatabasePathText; }

            set
            {
                _selectDatabasePathText = value;
                RaisePropertyChanged(() => SelectDatabasePathText);
            }
        }

        #endregion

        #region SelectDatabaseButtonClick Command

        /// <summary>
        /// Private member backing variable for <see cref="SelectDatabaseButtonClick" />
        /// </summary>
        private RelayCommand _selectDatabaseButtonClick = null;

        /// <summary>
        /// Comment
        /// </summary>
        public RelayCommand SelectDatabaseButtonClick => _selectDatabaseButtonClick ?? (_selectDatabaseButtonClick = new RelayCommand(SelectDatabaseButtonClick_Execute));

        private void SelectDatabaseButtonClick_Execute()
        {
            var openFileDialog = new OpenFileDialog
            {
                CheckFileExists = true,
                Filter = "eJay Database|*.dmdb|All files|*.*"
            };

            //Application.Current.Shutdown();

            if (openFileDialog.ShowDialog() == true)
            {
                var x = new SQLiteConnection(new SQLitePlatformGeneric(), openFileDialog.FileName);

                try
                {
                    x.Table<Person>().ToList();

                    App.Settings.Database = openFileDialog.FileName;
                    App.SaveSettings();

                    ProgramRequestedClose = true;
                    RequestClose?.Invoke(null, null);
                }
                catch (Exception)
                {
                    MessageBox.Show(
                        string.Format("File is not an eJay database\n\n{0}", openFileDialog.FileName),
                        "File invalid",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
                finally
                {
                    x.Close();
                }
            }

        }

        #endregion

        #region CreateDatabaseButtonClick Command

        /// <summary>
        /// Private member backing variable for <see cref="CreateDatabaseButtonClick" />
        /// </summary>
        private RelayCommand _createDatabaseButtonClick = null;

        /// <summary>
        /// Comment
        /// </summary>
        public RelayCommand CreateDatabaseButtonClick => _createDatabaseButtonClick ?? (_createDatabaseButtonClick = new RelayCommand(CreateDatabaseButtonClick_Execute));

        private void CreateDatabaseButtonClick_Execute()
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "eJay Database|*.dmdb|Standard database|*.db",
                OverwritePrompt = true
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                App.Settings.Database = saveFileDialog.FileName;
                App.SaveSettings();

                ProgramRequestedClose = true;
                RequestClose?.Invoke(null, null);
            }
        }

        #endregion
    }
}
