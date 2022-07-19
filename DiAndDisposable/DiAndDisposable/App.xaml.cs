using Modules.Sample;
using Prism.Ioc;
using Prism.Modularity;
using Samples;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace DiAndDisposable;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App
{
    private CompositeDisposable Disposables = new CompositeDisposable();

    protected override Window CreateShell() => Container.Resolve<Views.MainWindow>();

    protected override void RegisterTypes(IContainerRegistry containerRegistry)
    {
        // Libraries
        containerRegistry.RegisterInstance(Disposables, "CompositeDisposable");
    }

    protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
    {
        moduleCatalog.AddModule<SampleModule>();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        base.OnExit(e);

        Disposables.Dispose();
    }
}

