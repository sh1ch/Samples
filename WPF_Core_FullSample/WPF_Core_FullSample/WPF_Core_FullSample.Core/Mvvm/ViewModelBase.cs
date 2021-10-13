using Prism.Mvvm;
using Prism.Navigation;

namespace WPF_Core_FullSample.Core.Mvvm
{
    public abstract class ViewModelBase : BindableBase, IDestructible
    {
        protected ViewModelBase()
        {

        }

        public virtual void Destroy()
        {

        }
    }
}
