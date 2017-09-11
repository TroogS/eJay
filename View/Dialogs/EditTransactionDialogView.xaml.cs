using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DebtMgr.Extensions;

namespace DebtMgr.View.Dialogs
{
    /// <summary>
    /// Interaktionslogik für EditTransactionDialogView.xaml
    /// </summary>
    public partial class EditTransactionDialogView : Window
    {
        public EditTransactionDialogView(Guid transactionId)
        {
            InitializeComponent();
            this.CenterOnParent();

            App.Locator.EditTransactionDialogView.RequestClose += (s, e) => Close();
            App.Locator.EditTransactionDialogView.ClearView();
            App.Locator.EditTransactionDialogView.TransactionId = transactionId;
            App.Locator.EditTransactionDialogView.LoadTransaction();

            DataContext = App.Locator.EditTransactionDialogView;
        }

        #region Window_KeyUp()

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Event handler. Called by Window for key up events. </summary>
        ///
        /// <remarks>   Andre Beging, 12.09.2017. </remarks>
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

        #region TextBox_OnKeyUp()

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Event handler. Called by TextBox for on key up events. </summary>
        ///
        /// <remarks>   Andre Beging, 12.09.2017. </remarks>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Key event information. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        private void TextBox_OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Enter) || e.Key.Equals(Key.Return))
                App.Locator.EditTransactionDialogView.SaveTransactionButtonClickCommand.Execute(null);
        }

        #endregion
    }
}
