using Prism.Regions;
using WPF_Core_FullSample.Core.Mvvm;
using WPF_Core_FullSample.Services.Interfaces;

namespace WPF_Core_FullSample.Modules.ModuleName.ViewModels
{
    public class ViewAViewModel : RegionViewModelBase
    {
        private string _message;
        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }

        private string _messageAsync = "Now Loading...";
        public string MessageAsync
        {
            get { return _messageAsync; }
            set { SetProperty(ref _messageAsync, value); }
        }

        public ViewAViewModel(IRegionManager regionManager, IMessageService messageService, IMessageServiceAsync messageServiceAsync) :
            base(regionManager)
        {
            Message = messageService.GetMessage();
            messageServiceAsync.GetMessageAsync().AsTask().ContinueWith(p =>
            {
                MessageAsync = p.Result;
            });
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            //do something
        }
    }
}
