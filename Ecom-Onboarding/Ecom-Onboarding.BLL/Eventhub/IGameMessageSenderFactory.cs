using Microsoft.Extensions.Configuration;

namespace Ecom_Onboarding.BLL.Eventhub
{
    public interface IGameMessageSenderFactory
    {
        IGameMessageSender Create(IConfiguration config, string eventHubName);
    }
}
