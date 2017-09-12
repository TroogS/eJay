using System;
using System.Windows;
using DebtMgr.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace DebtMgr.ViewModel.Dialogs
{
    public class NewPersonDialogViewModel : ViewModelBase
    {
        #region Public Properties

        public event EventHandler RequestClose;
        public PersonDialogMode DialogMode { get; set; }
        public Guid EditPersonId { get; set; }
        public Person EditPerson { get; set; }

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
        #region SaveButtonText (string) Property

        /// <summary>
        /// Privater Teil von <see cref="SaveButtonText" />
        /// </summary>
        private string _saveButtonText;

        /// <summary>
        /// Comment
        ///</summary>
        public string SaveButtonText
        {
            get { return _saveButtonText; }

            set
            {
                _saveButtonText = value;
                RaisePropertyChanged(() => SaveButtonText);
            }
        }

        #endregion

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
                bool success = false;
                if (DialogMode == PersonDialogMode.New)
                {
                    var newPerson = new Person
                    {
                        FirstName = FirstNameTextBoxText,
                        LastName = LastNameTextBoxText
                    };

                    if (App.Database.Insert(newPerson) == 1)
                        success = true;
                }

                if (DialogMode == PersonDialogMode.Edit)
                {
                    EditPerson.FirstName = FirstNameTextBoxText;
                    EditPerson.LastName = LastNameTextBoxText;
                    if(App.Database.InsertOrReplace(EditPerson) == 1)
                        success = true;
                }

                if (success)
                {
                    App.Locator.MainView.UpdatePersonsList();
                    App.Locator.AddTransactionView.UpdatesPersonsComboBox();
                    RequestClose?.Invoke(null, null);
                }
                else
                {
                    MessageBox.Show("Something bad happened", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        #endregion

        #region SetModeSpecifics()

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Sets mode specifics. </summary>
        ///
        /// <remarks>   Andre Beging, 12.09.2017. </remarks>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public void SetModeSpecifics()
        {
            if (DialogMode != PersonDialogMode.Edit || EditPersonId == Guid.Empty)
            {
                ClearView();
                return;
            }

            // edit mode
            EditPerson = App.Database.Get<Person>(EditPersonId);

            if (EditPerson == null)
            {
                ClearView();
                return;
            }

            FirstNameTextBoxText = EditPerson.FirstName;
            LastNameTextBoxText = EditPerson.LastName;
            WindowTitle = "Edit Person";
            SaveButtonText = "Save";
        }

        #endregion

        #region ClearView()

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Clears the view. </summary>
        ///
        /// <remarks>   Andre Beging, 08.09.2017. </remarks>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public void ClearView()
        {
            FirstNameTextBoxText = string.Empty;
            LastNameTextBoxText = string.Empty;
            DialogMode = PersonDialogMode.New;
            WindowTitle = "New Person";
            SaveButtonText = "Create";
            EditPersonId = Guid.Empty;
        }

        #endregion

    }
}
