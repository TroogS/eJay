using System;
using System.Windows;
using DebtMgr.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace DebtMgr.ViewModel.Dialogs
{
    public class NewPersonDialogViewModel : ViewModelBase
    {
        public event EventHandler RequestClose;

        #region FirstNameTextBoxText (string) Property

        /// <summary>
        /// Privater Teil von <see cref="FirstNameTextBoxText" />
        /// </summary>
        private string _firstNameTextBoxText;

        /// <summary>
        /// Comment
        ///</summary>
        public string FirstNameTextBoxText
        {
            get { return _firstNameTextBoxText; }

            set
            {
                _firstNameTextBoxText = value;
                RaisePropertyChanged(() => FirstNameTextBoxText);
                CreatePersonButtonClickCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion
        #region LastNameTextBoxText (string) Property

        /// <summary>
        /// Privater Teil von <see cref="LastNameTextBoxText" />
        /// </summary>
        private string _lastNameTextBoxText;

        /// <summary>
        /// Comment
        ///</summary>
        public string LastNameTextBoxText
        {
            get { return _lastNameTextBoxText; }

            set
            {
                _lastNameTextBoxText = value;
                RaisePropertyChanged(() => LastNameTextBoxText);
                CreatePersonButtonClickCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion

        #region CreatePersonButtonClickCommand Command

        /// <summary>
        /// Private member backing variable for <see cref="CreatePersonButtonClickCommand" />
        /// </summary>
        private RelayCommand _createPersonButtonClickCommand = null;

        /// <summary>
        /// Comment
        /// </summary>
        public RelayCommand CreatePersonButtonClickCommand => _createPersonButtonClickCommand ?? (_createPersonButtonClickCommand = new RelayCommand(CreatePersonButtonClickCommand_Execute, CreatePersonButtonClickCommand_CanExecute));

        private bool CreatePersonButtonClickCommand_CanExecute()
        {
            if (string.IsNullOrWhiteSpace(FirstNameTextBoxText) || string.IsNullOrWhiteSpace(LastNameTextBoxText))
            {
                return false;
            }
            return true;
        }

        private void CreatePersonButtonClickCommand_Execute()
        {
            if (CreatePersonButtonClickCommand_CanExecute())
            {
                var newPerson = new Person
                {
                    FirstName = FirstNameTextBoxText,
                    LastName = LastNameTextBoxText
                };

                var resultId = App.Database.Insert(newPerson);

                if (resultId == 1)
                {
                    App.Locator.MainView.UpdatePersonsList();
                    App.Locator.AddTransactionView.UpdatesPersonsComboBox();
                    RequestClose?.Invoke(null, null);
                    ClearView();
                }
                else
                {
                    MessageBox.Show("Something bad happened", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        #endregion

        #region ClearView()

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Clears the view. </summary>
        ///
        /// <remarks>   Andre Beging, 08.09.2017. </remarks>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        private void ClearView()
        {
            FirstNameTextBoxText = string.Empty;
            LastNameTextBoxText = string.Empty;
        }

        #endregion

    }
}
