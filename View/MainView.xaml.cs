using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DebtMgr.Helper;

namespace DebtMgr.View
{
    /// <summary>
    /// Interaktionslogik für MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Andre Beging, 10.09.2017. </remarks>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public MainView()
        {
            InitializeComponent();
            DataContext = App.Locator.MainView;

            PersonListView.KeyUp += PersonListViewOnKeyUp;
            TransactionHistoryListView.KeyUp += TransactionHistoryListViewOnKeyUp;
        }

        #region PersonListViewOnKeyUp()

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Person list view on key up. </summary>
        ///
        /// <remarks>   Andre Beging, 10.09.2017. </remarks>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Key event information. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        private void PersonListViewOnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                if (App.Locator.MainView.DeletePersonContextMenuCommand.CanExecute(null))
                    App.Locator.MainView.DeletePersonContextMenuCommand.Execute(null);
            }
        }

        #endregion

        #region TransactionHistoryListViewOnKeyUp()

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Transaction history list view on key up. </summary>
        ///
        /// <remarks>   Andre Beging, 10.09.2017. </remarks>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Key event information. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        private void TransactionHistoryListViewOnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                if (App.Locator.MainView.DeleteTransactionContextMenuCommand.CanExecute(null))
                    App.Locator.MainView.DeleteTransactionContextMenuCommand.Execute(null);
            }
        }

        #endregion

        #region PersonListView_OnMouseDoubleClick()

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Event handler. Called by PersonListView for on mouse double click events.
        /// </summary>
        ///
        /// <remarks>   Andre Beging, 13.09.2017. </remarks>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Mouse button event information. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        private void PersonListView_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            App.Locator.MainView.EditPersonContextMenuCommand.Execute(null);
        }

        #endregion

        #region TransactionHistoryListView_OnMouseDoubleClick()

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Event handler. Called by TransactionHistoryListView for on mouse double click events.
        /// </summary>
        ///
        /// <remarks>   Andre Beging, 13.09.2017. </remarks>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        Mouse button event information. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        private void TransactionHistoryListView_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            App.Locator.MainView.EditTransactionContextMenuCommand.Execute(null);
        }

        #endregion

        private void PrintContextMenu_OnClick(object sender, RoutedEventArgs e)
        {
            var targetElement = RightPanel;

            PrintHelper.SaveUsingEncoder("test.png", targetElement);
            return;
        }
    }
}
