using DebtMgr.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using SQLiteNetExtensions.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;

namespace DebtMgr.ViewModel.Dialogs
{
    public class AddTransactionViewModel : ViewModelBase
    {
        #region Public Properties

        public event EventHandler RequestClose;
        public TransactionType DialogMode { get; set; }

        public Person PreselectedPerson { get; set; }

        #endregion

        #region WindowTitle (string) Property

        /// <summary>
        /// Privater Teil von <see cref="WindowTitle" />
        /// </summary>
        private string _windowTitle;

        /// <summary>
        /// Comment
        ///</summary>
        public string WindowTitle
        {
            get { return _windowTitle; }

            set
            {
                _windowTitle = value;
                RaisePropertyChanged(() => WindowTitle);
            }
        }

        #endregion

        #region AmountTextBoxText (string) Property

        /// <summary>
        /// Privater Teil von <see cref="AmountTextBoxText" />
        /// </summary>
        private string _amountTextBoxText;

        /// <summary>
        /// Comment
        ///</summary>
        public string AmountTextBoxText
        {
            get
            {
                return _amountTextBoxText;
            }

            set
            {
                _amountTextBoxText = value;
                RaisePropertyChanged(() => AmountTextBoxText);
                AddTransactionButtonClickCommand.RaiseCanExecuteChanged();
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets the amount text box text as number representation. </summary>
        ///
        /// <value> The amount text box text number representation. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public double AmountTextBoxTextNumberRepresentation
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_amountTextBoxText)) return 0.0;
                return double.Parse(_amountTextBoxText, CultureInfo.InvariantCulture);
            }
        }

        #endregion
        #region DescriptionTextBoxText (string) Property

        /// <summary>
        /// Privater Teil von <see cref="DescriptionTextBoxText" />
        /// </summary>
        private string _descriptionTextBoxText;

        /// <summary>
        /// DescriptionTextBoxText
        ///</summary>
        public string DescriptionTextBoxText
        {
            get { return _descriptionTextBoxText; }

            set
            {
                _descriptionTextBoxText = value;
                RaisePropertyChanged(() => DescriptionTextBoxText);
                AddTransactionButtonClickCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion
        #region DatePickerSelectedDate (DateTime?) Property

        /// <summary>
        /// Privater Teil von <see cref="DatePickerSelectedDate" />
        /// </summary>
        private DateTime? _datePickerSelectedDate;

        /// <summary>
        /// Comment
        ///</summary>
        public DateTime? DatePickerSelectedDate
        {
            get { return _datePickerSelectedDate; }

            set
            {
                _datePickerSelectedDate = value;
                RaisePropertyChanged(() => DatePickerSelectedDate);
                AddTransactionButtonClickCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion

        #region PersonComboBoxItemSource (List<Person>) Property

        /// <summary>
        /// Privater Teil von <see cref="PersonComboBoxItemSource" />
        /// </summary>
        private List<Person> _personComboBoxItemSource;

        /// <summary>
        /// Comment
        ///</summary>
        public List<Person> PersonComboBoxItemSource
        {
            get { return _personComboBoxItemSource; }

            set
            {
                _personComboBoxItemSource = value;
                RaisePropertyChanged(() => PersonComboBoxItemSource);
            }
        }

        #endregion
        #region PersonComboBoxSelectedItem (Person) Property

        /// <summary>
        /// Privater Teil von <see cref="PersonComboBoxSelectedItem" />
        /// </summary>
        private Person _personComboBoxSelectedItem;

        /// <summary>
        /// Comment
        ///</summary>
        public Person PersonComboBoxSelectedItem
        {
            get { return _personComboBoxSelectedItem; }

            set
            {
                _personComboBoxSelectedItem = value;
                RaisePropertyChanged(() => PersonComboBoxSelectedItem);
                AddTransactionButtonClickCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion

        #region AddTransactionButtonClickCommand Command

        /// <summary>
        /// Private member backing variable for <see cref="AddTransactionButtonClickCommand" />
        /// </summary>
        private RelayCommand _addTransactionButtonClickCommand = null;

        /// <summary>
        /// Comment
        /// </summary>
        public RelayCommand AddTransactionButtonClickCommand => _addTransactionButtonClickCommand ?? (_addTransactionButtonClickCommand = new RelayCommand(AddTransactionButtonClickCommand_Execute, AddTransactionButtonClickCommand_CanExecute));

        private bool AddTransactionButtonClickCommand_CanExecute()
        {
            if (AmountTextBoxTextNumberRepresentation < 0.01) return false;
            if (string.IsNullOrWhiteSpace(DescriptionTextBoxText)) return false;
            if (PersonComboBoxSelectedItem == null) return false;
            if (DatePickerSelectedDate == null) return false;

            return true;
        }

        private void AddTransactionButtonClickCommand_Execute()
        {
            if (AddTransactionButtonClickCommand_CanExecute())
            {
                try
                {
                    var person = App.Database.Get<Person>(PersonComboBoxSelectedItem.Id);
                    App.Database.GetChildren(person);

                    if (DatePickerSelectedDate != null)
                        person.Transactions.Add(new Transaction
                        {
                            Amount = AmountTextBoxTextNumberRepresentation,
                            Type = DialogMode,
                            Description = DescriptionTextBoxText,
                            PersonId = person.Id,
                            Time = DatePickerSelectedDate.Value
                        });

                    App.Database.InsertOrReplaceWithChildren(person);
                    RequestClose?.Invoke(null, null);
                    App.Locator.MainView.UpdatePersonsList();
                }
                catch (Exception)
                {
                    MessageBox.Show("Something bad happened", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                

            }
        }

        #endregion

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Andre Beging, 10.09.2017. </remarks>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public AddTransactionViewModel()
        {
            UpdatesPersonsComboBox();
        }

        #region UpdatesPersonsComboBox()

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Updates persons combo box. </summary>
        ///
        /// <remarks>   Andre Beging, 09.09.2017. </remarks>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public void UpdatesPersonsComboBox()
        {
            var personList = App.Database.Table<Person>().OrderBy(x => x.FirstName).ToList();
            PersonComboBoxItemSource = personList;
        }

        #endregion
        #region SetModeSpecificStrings()

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Sets mode specific strings. </summary>
        ///
        /// <remarks>   Andre Beging, 09.09.2017. </remarks>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public void SetModeSpecificStrings()
        {
            if (DialogMode == TransactionType.Deposit)
            {
                WindowTitle = "Add Deposit";
            }

            if (DialogMode == TransactionType.Charge)
            {
                WindowTitle = "Add Charge";
            }

            PersonComboBoxSelectedItem =
                PersonComboBoxItemSource.FirstOrDefault(x => x.Id == PreselectedPerson?.Id);
        }

        #endregion
        #region ClearView()

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Clears the view. </summary>
        ///
        /// <remarks>   Andre Beging, 09.09.2017. </remarks>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public void ClearView()
        {
            AmountTextBoxText = string.Empty;
            DescriptionTextBoxText = string.Empty;
            PersonComboBoxSelectedItem = null;
            DatePickerSelectedDate = DateTime.Now;
        }

        #endregion
    }
}
