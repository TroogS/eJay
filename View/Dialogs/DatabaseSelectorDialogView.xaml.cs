using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using DebtMgr.Extensions;

namespace DebtMgr.View.Dialogs
{
    /// <summary>
    /// Interaktionslogik für DatabaseSelectorDialogView.xaml
    /// </summary>
    public partial class DatabaseSelectorDialogView : Window
    {
        public DatabaseSelectorDialogView()
        {
            InitializeComponent();

            this.CenterOnParent();

            App.Locator.DatabaseSelectorDialogView.RequestClose += (s, e) => Close();
            DataContext = App.Locator.DatabaseSelectorDialogView;
        }

        private void Window_OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Escape))
                Close();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            if (!App.Locator.DatabaseSelectorDialogView.ProgramRequestedClose)
                Application.Current.Shutdown();
        }
    }
}
