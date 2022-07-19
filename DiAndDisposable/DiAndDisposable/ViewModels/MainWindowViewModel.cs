using Prism.Mvvm;
using Samples;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace DiAndDisposable.ViewModels;

public class MainWindowViewModel : BindableBase
{
    public string Title => "DI and Disposable";

    public MainWindowViewModel(IUnityContainer container)
    {
        var disposable = container.Resolve<CompositeDisposable>("CompositeDisposable");

        disposable.Add(() =>
        {
            Debug.WriteLine($"Called from {nameof(MainWindowViewModel)}.");
        });
    }
}
