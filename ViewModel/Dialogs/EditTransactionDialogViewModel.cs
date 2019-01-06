using System;
using System.Globalization;
using eJay.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using SQLiteNetExtensions.Extensions;

namespace eJay.ViewModel.Dialogs
{
    public class EditTransactionDialogViewModel : ViewModelBase
    {
        #region Public Properties

        public event EventHandler RequestClose;

        public Guid TransactionId { get; set; }

        #endregion
        #region Fields

        private Transaction _transaction;

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
        #region WindowIcon (string) Property

        /// <summary>
        /// Privater Teil von <see cref="WindowIcon" />
        /// </summary>
        private string _windowIcon;

        /// <summary>
        /// Comment
        ///</summary>
        public string WindowIcon
        {
            get { return _windowIcon; }

            set
            {
                _windowIcon = value;
                RaisePropertyChanged(() => WindowIcon);
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
            get { return _amountTextBoxText; }

            set
            {
                _amountTextBoxText = value;
                RaisePropertyChanged(() => AmountTextBoxText);
                SaveTransactionButtonClickCommand.RaiseCanExecuteChanged();
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
        #region PersonNameLabelContent (string) Property

        /// <summary>
        /// Privater Teil von <see cref="PersonNameLabelContent" />
        /// </summary>
        private string _personNameLabelContent;

        /// <summary>
        /// Comment
        ///</summary>
        public string PersonNameLabelContent
        {
            get { return _personNameLabelContent; }

            set
            {
                _personNameLabelContent = value;
                RaisePropertyChanged(() => PersonNameLabelContent);
            }
        }

        #endregion
        #region DescriptionTextBoxText (string) Property

        /// <summary>
        /// Privater Teil von <see cref="DescriptionTextBoxText" />
        /// </summary>
        private string _descriptionTextBoxText;

        /// <summary>
        /// Comment
        ///</summary>
        public string DescriptionTextBoxText
        {
            get { return _descriptionTextBoxText; }

            set
            {
                _descriptionTextBoxText = value;
                RaisePropertyChanged(() => DescriptionTextBoxText);
                SaveTransactionButtonClickCommand.RaiseCanExecuteChanged();
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
                SaveTransactionButtonClickCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion

        #region SaveTransactionButtonClickCommand Command

        /// <summary>
        /// Private member backing variable for <see cref="SaveTransactionButtonClickCommand" />
        /// </summary>
        private RelayCommand _saveTransactionButtonClickCommand = null;

        /// <summary>
        /// Comment
        /// </summary>
        public RelayCommand SaveTransactionButtonClickCommand => _saveTransactionButtonClickCommand ?? (_saveTransactionButtonClickCommand = new RelayCommand(SaveTransactionButtonClickCommand_Execute, SaveTransactionButtonClickCommand_CanExecute));

        private bool SaveTransactionButtonClickCommand_CanExecute()
        {
            if (_transaction == null) return false;
            if (AmountTextBoxTextNumberRepresentation < 0.01) return false;
            if (string.IsNullOrWhiteSpace(DescriptionTextBoxText)) return false;
            if (DatePickerSelectedDate == null) return false;

            return true;
        }

        private void SaveTransactionButtonClickCommand_Execute()
        {
            if (SaveTransactionButtonClickCommand_CanExecute())
            {
                var transaction = App.Database.Get<Transaction>(TransactionId);

                if (DatePickerSelectedDate != null)
                {
                    transaction.Amount = AmountTextBoxTextNumberRepresentation;
                    transaction.Description = DescriptionTextBoxText;
                    transaction.Time = DatePickerSelectedDate.Value.AddHours(12);
                }

                App.Database.InsertOrReplace(transaction);
                RequestClose?.Invoke(null, null);
                App.Locator.MainView.UpdatePersonsList();
            }
        }

        #endregion

        #region LoadTransaction()

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Loads the transaction. </summary>
        ///
        /// <remarks>   Andre Beging, 12.09.2017. </remarks>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public void LoadTransaction()
        {
            if (TransactionId == Guid.Empty) return;
            _transaction = App.Database.Get<Transaction>(TransactionId);

            if (_transaction == null) return;
            App.Database.GetChildren(_transaction);

            PersonNameLabelContent = string.Format("{0} {1}", _transaction.Person.FirstName,
                _transaction.Person.LastName);
            AmountTextBoxText = _transaction.Amount.ToString(CultureInfo.InvariantCulture);
            DescriptionTextBoxText = _transaction.Description;
            DatePickerSelectedDate = _transaction.Time;

            if (_transaction.Type == TransactionType.Charge)
            {
                WindowTitle = "Edit Charge";
                WindowIcon = "../../Content/money_red.ico";
            }
            else if (_transaction.Type == TransactionType.Deposit)
            { 
                WindowTitle = "Edit Deposit";
                WindowIcon = "../../Content/money_green.ico";
            }
        }

        #endregion
        #region ClearView()

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Clears the view. </summary>
        ///
        /// <remarks>   Andre Beging, 12.09.2017. </remarks>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public void ClearView()
        {
            PersonNameLabelContent = string.Empty;
            AmountTextBoxText = string.Empty;
            DescriptionTextBoxText = string.Empty;
            DatePickerSelectedDate = null;

            TransactionId = Guid.Empty;
        }

        #endregion
    }
}