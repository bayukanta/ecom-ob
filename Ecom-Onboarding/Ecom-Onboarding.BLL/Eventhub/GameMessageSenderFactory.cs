using Microsoft.Extensions.Configuration;

namespace Ecom_Onboarding.BLL.Eventhub
{
    public class GameMessageSenderFactory : IGameMessageSenderFactory
    {
        public IGameMessageSender Create(IConfiguration config, string eventHubName)
        {
            return new GameMessageSender(config, eventHubName); ;
        }
    }
}

