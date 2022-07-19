using Modules.Sample.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modules.Sample;

public class SampleModule : IModule
{
    private readonly IRegionManager _regionManager;

    public SampleModule(IRegionManager regionManager)
    {
        _regionManager = regionManager;
    }

    public void OnInitialized(IContainerProvider containerProvider)
    {
        _regionManager.RequestNavigate("Left", "Test1");
        _regionManager.RequestNavigate("Right", "Test2");
    }

    public void RegisterTypes(IContainerRegistry containerRegistry)
    {
        // メニュー
        containerRegistry.RegisterForNavigation<Test1>("Test1");
        containerRegistry.RegisterForNavigation<Test2>("Test2");
    }
}
