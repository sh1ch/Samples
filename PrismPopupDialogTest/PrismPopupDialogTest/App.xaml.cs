using Prism.Ioc;
using PrismPopupDialogTest.ViewModels;
using PrismPopupDialogTest.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace PrismPopupDialogTest
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell() => Container.Resolve<MainWindow>();

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterDialog<Dialog1>();
            containerRegistry.RegisterDialog<Dialog2, Dialog1ViewModel>();
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
        }
    }
}
