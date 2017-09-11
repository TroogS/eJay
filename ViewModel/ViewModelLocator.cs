/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:DebtMgr"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using DebtMgr.ViewModel.Dialogs;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace DebtMgr.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            ////if (ViewModelBase.IsInDesignModeStatic)
            ////{
            ////    // Create design time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            ////}
            ////else
            ////{
            ////    // Create run time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DataService>();
            ////}

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<NewPersonDialogViewModel>();
            SimpleIoc.Default.Register<AddTransactionViewModel>();
            SimpleIoc.Default.Register<DatabaseSelectorDialogViewModel>();
        }

        public MainViewModel MainView => ServiceLocator.Current.GetInstance<MainViewModel>();
        public NewPersonDialogViewModel NewPersonDialogView => ServiceLocator.Current.GetInstance<NewPersonDialogViewModel>();
        public AddTransactionViewModel AddTransactionView => ServiceLocator.Current.GetInstance<AddTransactionViewModel>();
        public DatabaseSelectorDialogViewModel DatabaseSelectorDialogView => ServiceLocator.Current.GetInstance<DatabaseSelectorDialogViewModel>();

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}