using DebtMgr.Model;
using DebtMgr.View.Dialogs;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using SQLiteNetExtensions.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System;
using System.Threading;
using System.IO;
using System.Diagnostics;
using System.Windows.Controls;
using DebtMgr.Helper;
using Microsoft.Win32;
using System.Windows.Markup;
using System.Xml;

namespace DebtMgr.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        #region PersonListViewItemSource (List<Person>) Property

        /// <summary>
        /// Privater Teil von <see cref="PersonListViewItemSource" />
        /// </summary>
        private List<Person> _personListViewItemSource;

        /// <summary>
        /// Comment
        ///</summary>
        public List<Person> PersonListViewItemSource
        {
            get { return _personListViewItemSource; }

            set
            {
                _personListViewItemSource = value;
                RaisePropertyChanged(() => PersonListViewItemSource);
            }
        }

        #endregion
        #region PersonListViewSelectedItem (Person) Property

        /// <summary>
        /// Privater Teil von <see cref="PersonListViewSelectedItem" />
        /// </summary>
        private Person _personListViewSelectedItem;

        /// <summary>
        /// PersonListViewSelectedItem
        ///</summary>
        public Person PersonListViewSelectedItem
        {
            get { return _personListViewSelectedItem; }

            set
            {
                _personListViewSelectedItem = value;
                RaisePropertyChanged(() => PersonListViewSelectedItem);
                DeletePersonContextMenuCommand.RaiseCanExecuteChanged();
                EditPersonContextMenuCommand.RaiseCanExecuteChanged();
                SaveScreenshotCommand.RaiseCanExecuteChanged();
                SendViaTelegramCommand.RaiseCanExecuteChanged();

                ScreenshotPossible = value != null;

                UpdateDetailView();
            }
        }



        #endregion

        #region DetailViewHeaderLabelContent (string) Property

        /// <summary>
        /// Privater Teil von <see cref="DetailViewHeaderLabelContent" />
        /// </summary>
        private string _detailViewHeaderLabelContent;

        /// <summary>
        /// Comment
        ///</summary>
        public string DetailViewHeaderLabelContent
        {
            get { return _detailViewHeaderLabelContent; }

            set
            {
                _detailViewHeaderLabelContent = value;
                RaisePropertyChanged(() => DetailViewHeaderLabelContent);
            }
        }

        #endregion
        #region DetailViewBalanceLabel (string) Property

        /// <summary>
        /// Privater Teil von <see cref="DetailViewBalanceLabel" />
        /// </summary>
        private string _detailViewBalanceLabel;

        /// <summary>
        /// Comment
        ///</summary>
        public string DetailViewBalanceLabel
        {
            get { return _detailViewBalanceLabel; }

            set
            {
                _detailViewBalanceLabel = value;
                RaisePropertyChanged(() => DetailViewBalanceLabel);
            }
        }

        #endregion

        #region OverallBalanceLabel (string) Property

        /// <summary>
        /// Privater Teil von <see cref="OverallBalanceLabel" />
        /// </summary>
        private string _overallBalanceLabel;

        /// <summary>
        /// Comment
        ///</summary>
        public string OverallBalanceLabel
        {
            get { return _overallBalanceLabel; }

            set
            {
                _overallBalanceLabel = value;
                RaisePropertyChanged(() => OverallBalanceLabel);
            }
        }

        #endregion

        #region ScreenshotPossible (bool) Property

        /// <summary>
        /// Privater Teil von <see cref="ScreenshotPossible" />
        /// </summary>
        private bool _screenshotPossible;

        /// <summary>
        /// Comment
        ///</summary>
        public bool ScreenshotPossible
        {
            get { return _screenshotPossible; }

            set
            {
                _screenshotPossible = value;
                RaisePropertyChanged(() => ScreenshotPossible);
            }
        }

        #endregion

        #region SaveScreenshotCommand Command

        /// <summary>
        /// Private member backing variable for <see cref="SaveScreenshotCommand" />
        /// </summary>
        private RelayCommand<Grid> _saveScreenshotCommand = null;

        /// <summary>
        /// Comment
        /// </summary>
        public RelayCommand<Grid> SaveScreenshotCommand => _saveScreenshotCommand ?? (_saveScreenshotCommand = new RelayCommand<Grid>(SaveScreenshotCommand_Execute));

        private void SaveScreenshotCommand_Execute(Grid grid)
        {
            if (grid == null) return;
            if (grid.ActualHeight == 0 || grid.ActualWidth == 0) return;

            var presetFileName = string.Format("{0}_{1}_{2}",
                DateTime.Now.ToString("yyyyMMdd"),
                PersonListViewSelectedItem.FirstName,
                PersonListViewSelectedItem.LastName);

            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "*.png|*.png";
            saveFileDialog.OverwritePrompt = true;
            saveFileDialog.FileName = presetFileName;

            if (saveFileDialog.ShowDialog() == true)
            {
                PrintHelper.SaveUsingEncoder(saveFileDialog.FileName, grid);
                grid.ShowGridLines = false;
            }

            
        }

        #endregion

        #region SendViaTelegramCommand Command

        /// <summary>
        /// Private member backing variable for <see cref="SendViaTelegramCommand" />
        /// </summary>
        private RelayCommand<Grid> _sendViaTelegramCommand = null;

        /// <summary>
        /// Comment
        /// </summary>
        public RelayCommand<Grid> SendViaTelegramCommand => _sendViaTelegramCommand ?? (_sendViaTelegramCommand = new RelayCommand<Grid>(SendViaTelegramCommand_Execute, SendViaTelegramCommand_CanExecute));

        private bool SendViaTelegramCommand_CanExecute(Grid grid)
        {
            if (grid == null) return false;
            if (PersonListViewSelectedItem == null) return false;

            var telegramPath = Properties.Settings.Default.TelegramPath;
            if (string.IsNullOrWhiteSpace(telegramPath)) return false;
            if (!File.Exists(telegramPath)) return false;

            return true;
        }

        private void SendViaTelegramCommand_Execute(Grid grid)
        {
            if (grid == null) return;
            if (grid.ActualHeight == 0 || grid.ActualWidth == 0) return;

            var fileName = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".png");
            var parameter = string.Format("-sendpath \"{0}\"", fileName);

            // Create temp screenshot
            PrintHelper.SaveUsingEncoder(fileName, grid);

            // Create cmd process
            Process process = new Process();

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.FileName = Properties.Settings.Default.TelegramPath;

            startInfo.Arguments = parameter;

            // Attach command to process
            process.StartInfo = startInfo;

            // Start process
            process.Start();
        }

        #endregion

        #region TransactionHistoryListViewItemSource (List<Transaction>) Property

        /// <summary>
        /// Privater Teil von <see cref="TransactionHistoryListViewItemSource" />
        /// </summary>
        private List<Transaction> _transactionHistoryListViewItemSource;

        /// <summary>
        /// Comment
        ///</summary>
        public List<Transaction> TransactionHistoryListViewItemSource
        {
            get { return _transactionHistoryListViewItemSource; }

            set
            {
                _transactionHistoryListViewItemSource = value;
                RaisePropertyChanged(() => TransactionHistoryListViewItemSource);
            }
        }

        #endregion
        #region TransactionHistoryListViewSelectedItem (Transaction) Property

        /// <summary>
        /// Privater Teil von <see cref="TransactionHistoryListViewSelectedItem" />
        /// </summary>
        private Transaction _transactionHistoryListViewSelectedItem;

        /// <summary>
        /// Comment
        ///</summary>
        public Transaction TransactionHistoryListViewSelectedItem
        {
            get { return _transactionHistoryListViewSelectedItem; }

            set
            {
                _transactionHistoryListViewSelectedItem = value;
                RaisePropertyChanged(() => TransactionHistoryListViewSelectedItem);
                DeleteTransactionContextMenuCommand.RaiseCanExecuteChanged();
                EditTransactionContextMenuCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion

        #region SortPersonListViewCommand

        /// <summary>   The sort person list view command. </summary>
        private RelayCommand<string> _sortPersonListViewCommand = null;

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets the sort person list view command. </summary>
        ///
        /// <value> The sort person list view command. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public RelayCommand<string> SortPersonListViewCommand
        {
            get
            {
                if (_sortPersonListViewCommand == null)
                    _sortPersonListViewCommand = new RelayCommand<string>(SortPersonListViewCommand_Execute);

                return _sortPersonListViewCommand;
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Sort person list view command execute. </summary>
        ///
        /// <remarks>   Andre Beging, 08.09.2017. </remarks>
        ///
        /// <param name="columnName">   Name of the column. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        private void SortPersonListViewCommand_Execute(string columnName)
        {
            if (string.IsNullOrWhiteSpace(columnName)) return;

            switch (columnName)
            {
                case "FirstName":
                    PersonListViewItemSource = PersonListViewItemSource.OrderBy(x => x.FirstName).ToList();
                    break;
                case "LastName":
                    PersonListViewItemSource = PersonListViewItemSource.OrderBy(x => x.LastName).ToList();
                    break;
                case "Total":
                    PersonListViewItemSource = PersonListViewItemSource.OrderBy(x => x.Total).ToList();
                    break;
            }
        }

        #endregion

        #region AddChargeContextMenuCommand Command

        /// <summary>
        /// Private member backing variable for <see cref="AddChargeContextMenuCommand" />
        /// </summary>
        private RelayCommand _addChargeContextMenuCommand = null;

        /// <summary>
        /// Comment
        /// </summary>
        public RelayCommand AddChargeContextMenuCommand => _addChargeContextMenuCommand ?? (_addChargeContextMenuCommand = new RelayCommand(AddChargeContextMenuCommand_Execute, AddChargeContextMenuCommand_CanExecute));

        private bool AddChargeContextMenuCommand_CanExecute()
        {
            if (PersonListViewItemSource != null & PersonListViewItemSource.Count > 0)
                return true;
            return false;
        }

        private void AddChargeContextMenuCommand_Execute()
        {
            var window = new AddTransactionView(TransactionType.Charge, PersonListViewSelectedItem);
            window.ShowDialog();
        }

        #endregion
        #region AddDepositContextMenuCommand Command

        /// <summary>
        /// Private member backing variable for <see cref="AddDepositContextMenuCommand" />
        /// </summary>
        private RelayCommand _addDepositContextMenuCommand = null;

        /// <summary>
        /// Comment
        /// </summary>
        public RelayCommand AddDepositContextMenuCommand => _addDepositContextMenuCommand ?? (_addDepositContextMenuCommand = new RelayCommand(AddDepositContextMenuCommand_Execute, AddDepositContextMenuCommand_CanExecute));

        private bool AddDepositContextMenuCommand_CanExecute()
        {
            if (PersonListViewItemSource != null & PersonListViewItemSource.Count > 0)
                return true;
            return false;
        }

        private void AddDepositContextMenuCommand_Execute()
        {
            var window = new AddTransactionView(TransactionType.Deposit, PersonListViewSelectedItem);
            window.ShowDialog();
        }

        #endregion
        #region NewPersonContextMenuCommand Command

        /// <summary>
        /// Private member backing variable for <see cref="NewPersonContextMenuCommand" />
        /// </summary>
        private RelayCommand _newPersonContextMenuCommand = null;

        /// <summary>
        /// Comment
        /// </summary>
        public RelayCommand NewPersonContextMenuCommand => _newPersonContextMenuCommand ?? (_newPersonContextMenuCommand = new RelayCommand(NewPersonContextMenuCommand_Execute));

        private void NewPersonContextMenuCommand_Execute()
        {
            var window = new NewPersonDialogView(PersonDialogMode.New);
            window.ShowDialog();
        }

        #endregion

        #region EditPersonContextMenuCommand Command

        /// <summary>
        /// Private member backing variable for <see cref="EditPersonContextMenuCommand" />
        /// </summary>
        private RelayCommand _editPersonContextMenuCommand = null;

        /// <summary>
        /// Comment
        /// </summary>
        public RelayCommand EditPersonContextMenuCommand => _editPersonContextMenuCommand ?? (_editPersonContextMenuCommand = new RelayCommand(EditPersonContextMenuCommand_Execute, EditPersonContextMenuCommand_CanExecute));

        private bool EditPersonContextMenuCommand_CanExecute()
        {
            if (PersonListViewSelectedItem != null)
                return true;
            return false;
        }

        private void EditPersonContextMenuCommand_Execute()
        {
            if (PersonListViewSelectedItem != null)
            {
                var window = new NewPersonDialogView(PersonDialogMode.Edit, PersonListViewSelectedItem.Id);
                window.ShowDialog();
            }
                
        }

        #endregion
        #region DeletePersonContextMenuCommand Command

        /// <summary>
        /// Private member backing variable for <see cref="DeletePersonContextMenuCommand" />
        /// </summary>
        private RelayCommand _deletePersonContextMenuCommand = null;

        /// <summary>
        /// Comment
        /// </summary>
        public RelayCommand DeletePersonContextMenuCommand => _deletePersonContextMenuCommand ?? (_deletePersonContextMenuCommand = new RelayCommand(DeletePersonContextMenuCommand_Execute, DeletePersonContextMenuCommand_CanExecute));

        private bool DeletePersonContextMenuCommand_CanExecute()
        {
            if (PersonListViewSelectedItem != null)
                return true;
            return false;
        }

        private void DeletePersonContextMenuCommand_Execute()
        {
            if (PersonListViewSelectedItem == null) return;

            var result = MessageBox.Show(
                string.Format(
                    "Are you sure to delete this person including all transactions?\n\n{0} {1}",
                    PersonListViewSelectedItem.FirstName,
                    PersonListViewSelectedItem.LastName),
                "Delete Person",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                App.Database.Delete(PersonListViewSelectedItem, true);

                UpdatePersonsList();
            }
        }

        #endregion

        #region DeleteTransactionContextMenuCommand Command

        /// <summary>
        /// Private member backing variable for <see cref="DeleteTransactionContextMenuCommand" />
        /// </summary>
        private RelayCommand _deleteTransactionContextMenuCommand = null;

        /// <summary>
        /// Comment
        /// </summary>
        public RelayCommand DeleteTransactionContextMenuCommand => _deleteTransactionContextMenuCommand ?? (_deleteTransactionContextMenuCommand = new RelayCommand(DeleteTransactionContextMenuCommand_Execute, DeleteTransactionContextMenuCommand_CanExecute));

        private bool DeleteTransactionContextMenuCommand_CanExecute()
        {
            if (TransactionHistoryListViewSelectedItem != null)
                return true;
            return false;
        }

        private void DeleteTransactionContextMenuCommand_Execute()
        {
            if (TransactionHistoryListViewSelectedItem == null) return;

            var result = MessageBox.Show(
                string.Format(
                    "Are you sure to delete this {0}?\n\n{1} {2}\nAmount: {3} �\n{4}",
                    TransactionHistoryListViewSelectedItem.Type,
                    TransactionHistoryListViewSelectedItem.Person.FirstName,
                    TransactionHistoryListViewSelectedItem.Person.LastName,
                    TransactionHistoryListViewSelectedItem.Amount,
                    TransactionHistoryListViewSelectedItem.Description),
                string.Format("Delete {0}", TransactionHistoryListViewSelectedItem.Type),
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                App.Database.Delete<Transaction>(TransactionHistoryListViewSelectedItem.Id);
                UpdatePersonsList();
            }
        }

        #endregion
        #region EditTransactionContextMenuCommand Command

        /// <summary>
        /// Private member backing variable for <see cref="EditTransactionContextMenuCommand" />
        /// </summary>
        private RelayCommand _editTransactionContextMenuCommand = null;

        /// <summary>
        /// Comment
        /// </summary>
        public RelayCommand EditTransactionContextMenuCommand => _editTransactionContextMenuCommand ?? (_editTransactionContextMenuCommand = new RelayCommand(EditTransactionContextMenuCommand_Execute, EditTransactionContextMenuCommand_CanExecute));

        private bool EditTransactionContextMenuCommand_CanExecute()
        {
            if (TransactionHistoryListViewSelectedItem != null)
                return true;
            return false;
        }

        private void EditTransactionContextMenuCommand_Execute()
        {
            if (TransactionHistoryListViewSelectedItem == null) return;

            var window = new EditTransactionDialogView(TransactionHistoryListViewSelectedItem.Id);
            window.ShowDialog();
        }

        #endregion

        #region SwitchDatabaseMenuCommand Command

        /// <summary>
        /// Private member backing variable for <see cref="SwitchDatabaseMenuCommand" />
        /// </summary>
        private RelayCommand _PrivateCommandName = null;

        /// <summary>
        /// Comment
        /// </summary>
        public RelayCommand SwitchDatabaseMenuCommand => _PrivateCommandName ?? (_PrivateCommandName = new RelayCommand(SwitchDatabaseMenuCommand_Execute));

        private void SwitchDatabaseMenuCommand_Execute()
        {
            Properties.Settings.Default["Database"] = string.Empty;
            Properties.Settings.Default.Save();

            Thread.Sleep(100);


            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }

        #endregion
        #region OpenDatabaseLocationMenuCommand Command

        /// <summary>
        /// Private member backing variable for <see cref="OpenDatabaseLocationMenuCommand" />
        /// </summary>
        private RelayCommand _openDatabaseLocationMenuCommand = null;

        /// <summary>
        /// Comment
        /// </summary>
        public RelayCommand OpenDatabaseLocationMenuCommand => _openDatabaseLocationMenuCommand ?? (_openDatabaseLocationMenuCommand = new RelayCommand(OpenDatabaseLocationMenuCommand_Execute));

        private void OpenDatabaseLocationMenuCommand_Execute()
        {
            if (File.Exists(Properties.Settings.Default.Database))
            {
                Process.Start("explorer.exe", "/select, " + Properties.Settings.Default.Database);
            }
        }

        #endregion

        #region SetTelegramLocationCommand Command

        /// <summary>
        /// Private member backing variable for <see cref="SetTelegramLocationCommand" />
        /// </summary>
        private RelayCommand _setTelegramLocationCommand = null;

        /// <summary>
        /// Comment
        /// </summary>
        public RelayCommand SetTelegramLocationCommand => _setTelegramLocationCommand ?? (_setTelegramLocationCommand = new RelayCommand(SetTelegramLocationCommand_Execute));

        private void SetTelegramLocationCommand_Execute()
        {
            var checkTelegramPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "Telegram Desktop");

            var openFileDialog = new OpenFileDialog();

            if (Directory.Exists(checkTelegramPath))
                openFileDialog.InitialDirectory = checkTelegramPath;
            else
                openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == true)
            {
                if (Path.GetExtension(openFileDialog.FileName) == "exe" || Path.GetExtension(openFileDialog.FileName) == ".exe")
                {
                    Properties.Settings.Default.TelegramPath = openFileDialog.FileName;
                    Properties.Settings.Default.Save();
                }
            }


        }

        #endregion

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Initializes a new instance of the MainViewModel class. </summary>
        ///
        /// <remarks>   Andre Beging, 10.09.2017. </remarks>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public MainViewModel()
        {
            CheckDatabase();    

            UpdatePersonsList();
            UpdateDetailView();
        }

        #region UpdatePersonsList()

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Updates the persons list. </summary>
        ///
        /// <remarks>   Andre Beging, 08.09.2017. </remarks>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public void UpdatePersonsList()
        {
            // Remember selection
            var rememberSelection = PersonListViewSelectedItem?.Id;

            var personList = App.Database.GetAllWithChildren<Person>().OrderBy(x => x.FirstName).ThenBy(x => x.LastName).ToList();
            PersonListViewItemSource = personList;

            var overallBalance = personList.Sum(x => x.Total);
            OverallBalanceLabel = string.Format("{0:0.00}", overallBalance);
            
            AddChargeContextMenuCommand.RaiseCanExecuteChanged();
            AddDepositContextMenuCommand.RaiseCanExecuteChanged();

            // Restore selection
            if (rememberSelection != null && PersonListViewItemSource.Any(x => x.Id == rememberSelection))
                PersonListViewSelectedItem = PersonListViewItemSource.First(x => x.Id == rememberSelection);

            UpdateDetailView();
        }

        #endregion
        #region UpdateDetailView()

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Updates the detail view. </summary>
        ///
        /// <remarks>   Andre Beging, 10.09.2017. </remarks>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        private void UpdateDetailView()
        {
            if (PersonListViewSelectedItem == null)
            {
                DetailViewHeaderLabelContent = string.Empty;
                DetailViewBalanceLabel = string.Empty;
                TransactionHistoryListViewItemSource = null;
                return;
            };

            DetailViewHeaderLabelContent = string.Format("{0} {1}", PersonListViewSelectedItem.FirstName,
                PersonListViewSelectedItem.LastName);
            DetailViewBalanceLabel = string.Format("{0:0.00}", PersonListViewSelectedItem.Total);

            TransactionHistoryListViewItemSource = PersonListViewSelectedItem.Transactions.OrderByDescending(x => x.Time).ToList();
        }

        #endregion

        #region CheckDatabase()

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Check database. </summary>
        ///
        /// <remarks>   Andre Beging, 10.09.2017. </remarks>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        private void CheckDatabase()
        {
            var databasePath = Properties.Settings.Default.Database;
            if (string.IsNullOrWhiteSpace(databasePath))
            {
                var window = new DatabaseSelectorDialogView();
                var result = window.ShowDialog();
            }

            // Check if provided file path is a valid database
            try
            {
                App.Database.Table<Person>();
            }
            catch (Exception)
            {
                Properties.Settings.Default["Database"] = string.Empty;
                CheckDatabase();
            }
        }

        #endregion
    }
}