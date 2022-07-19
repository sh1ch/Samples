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

namespace Modules.Sample.ViewModels;

public class Test1ViewModel : BindableBase
{
    public Test1ViewModel(IUnityContainer container)
    {
        var disposable = container.Resolve<CompositeDisposable>("CompositeDisposable");

        disposable.Add(() =>
        {
            Debug.WriteLine($"Called from {nameof(Test1ViewModel)}.");
        });
    }
}