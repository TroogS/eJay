using System;
using System.Windows;
using System.Windows.Input;
using DebtMgr.Extensions;
using DebtMgr.Model;

namespace DebtMgr.View.Dialogs
{
    /// <summary>
    /// Interaktionslogik für NewPersonDialogView.xaml
    /// </summary>
    public partial class NewPersonDialogView : Window
    {
        public NewPersonDialogView(PersonDialogMode mode, Guid personId = default(Guid))
        {
            InitializeComponent();
            this.CenterOnParent();

            App.Locator.NewPersonDialogView.ClearView();
            App.Locator.NewPersonDialogView.DialogMode = mode;
            App.Locator.NewPersonDialogView.EditPersonId = personId;
            App.Locator.NewPersonDialogView.SetModeSpecifics();

            App.Locator.NewPersonDialogView.RequestClose += (s, e) => Close();
            DataContext = App.Locator.NewPersonDialogView;

            FirstNameTextBox.Focus();
        }

        #region TextBox_OnKeyUp()

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Event handler. Called by TextBox for on key up events. </summary>
        ///
        /// <remarks>   Andre Beging, 09.09.2017. </remarks>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Key event information. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        private void TextBox_OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Enter) || e.Key.Equals(Key.Return))
                App.Locator.NewPersonDialogView.CreatePersonButtonClickCommand.Execute(null);
        }

        #endregion

        #region Window_KeyUp()

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Event handler. Called by Window for key up events. </summary>
        ///
        /// <remarks>   Andre Beging, 09.09.2017. </remarks>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Key event information. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Escape))
                Close();
        }

        #endregion
    }
}
