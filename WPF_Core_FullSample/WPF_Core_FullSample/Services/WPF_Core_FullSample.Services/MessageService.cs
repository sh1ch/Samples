using WPF_Core_FullSample.Services.Interfaces;

namespace WPF_Core_FullSample.Services
{
    public class MessageService : IMessageService
    {
        public string GetMessage()
        {
            return "Hello from the Message Service";
        }
    }
}
