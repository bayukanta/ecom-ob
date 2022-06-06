using System;
using System.Threading.Tasks;

namespace Ecom_Onboarding.BLL.Eventhub
{
    public interface IGameMessageSender : IDisposable
    {
        Task CreateEventBatchAsync();
        bool AddMessage(object data);
        Task SendMessage();
    }
}
