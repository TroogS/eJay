using System;
using System.Collections.Generic;
using System.ComponentModel;
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
                Application.Current.Shutdown();
        }
    }
}
