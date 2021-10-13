using Prism.Ioc;
using Prism.Modularity;
using System.Windows;
using WPF_Core_FullSample.Modules.ModuleName;
using WPF_Core_FullSample.Services;
using WPF_Core_FullSample.Services.Interfaces;
using WPF_Core_FullSample.Views;

namespace WPF_Core_FullSample
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IMessageService, MessageService>();
            containerRegistry.RegisterSingleton<IMessageServiceAsync, MessageServiceAsync>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<ModuleNameModule>();
        }
    }
}
