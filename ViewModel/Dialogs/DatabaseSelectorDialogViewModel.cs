﻿using System;
using System.Linq;
using System.Windows;
using DebtMgr.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using SQLite.Net;
using SQLite.Net.Platform.Generic;

namespace DebtMgr.ViewModel.Dialogs
{
    public class DatabaseSelectorDialogViewModel : ViewModelBase
    {
        public event EventHandler RequestClose;
        public bool ProgramRequestedClose;

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
                Filter = "Debt Manager Database|*.dmdb|All files|*.*"
            };

            //Application.Current.Shutdown();

            if (openFileDialog.ShowDialog() == true)
            {
                var x = new SQLiteConnection(new SQLitePlatformGeneric(), openFileDialog.FileName);

                try
                {
                    x.Table<Person>().ToList();

                    Properties.Settings.Default["Database"] = openFileDialog.FileName;
                    Properties.Settings.Default.Save();

                    ProgramRequestedClose = true;
                    RequestClose?.Invoke(null, null);
                }
                catch (Exception)
                {
                    MessageBox.Show(
                        string.Format("File is not a Debt Manager database\n\n{0}", openFileDialog.FileName),
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
                Filter = "Debt Manager Database|*.dmdb|Standard database|*.db",
                CreatePrompt = true
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                Properties.Settings.Default["Database"] = saveFileDialog.FileName;
                Properties.Settings.Default.Save();

                ProgramRequestedClose = true;
                RequestClose?.Invoke(null, null);
            }
        }

        #endregion

        
    }
}
