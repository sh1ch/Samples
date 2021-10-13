
using System.Threading.Tasks;

namespace WPF_Core_FullSample.Services.Interfaces
{
    public interface IMessageService
    {
        string GetMessage();
    }

    public interface IMessageServiceAsync
    {
        ValueTask<string> GetMessageAsync();
    }
}
