using DebtMgr.Extensions;
using System.Windows;
using System.Windows.Input;
using DebtMgr.Model;

namespace DebtMgr.View.Dialogs
{
    /// <summary>
    /// Interaktionslogik für AddTransactionView.xaml
    /// </summary>
    public partial class AddTransactionView : Window
    {
        public AddTransactionView(TransactionType transactionType, Person preselectedPerson)
        {
            InitializeComponent();
            this.CenterOnParent();

            App.Locator.AddTransactionView.ClearView();

            App.Locator.AddTransactionView.DialogMode = transactionType;
            App.Locator.AddTransactionView.PreselectedPerson = preselectedPerson;
            App.Locator.AddTransactionView.SetModeSpecificStrings();

            App.Locator.AddTransactionView.RequestClose += (s, e) => Close();

            DataContext = App.Locator.AddTransactionView;
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
                App.Locator.AddTransactionView.AddTransactionButtonClickCommand.Execute(null);
        }

        #endregion
        #region Window_OnKeyUp()

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Event handler. Called by Window for on key up events. </summary>
        ///
        /// <remarks>   Andre Beging, 09.09.2017. </remarks>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Key event information. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        private void Window_OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Escape))
                Close();
        }

        #endregion
    }
}
