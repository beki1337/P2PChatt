using System.Threading.Tasks;
using static TDDD49Lab.Models.Chatt;

namespace TDDD49Lab.Models
{
    public interface IChatt
    {

        Task<string> SearchAsync(string username, string ipAddres, int port);
        Task<string> StartListingForUsersAsync(string username, string ipAddres,int port );
        bool IsRunning();
        Task DisconnectAsync();
        Task SendMessageAsync(string message);
        Task DeclineRequest();
        Task ListenForMessagesAsync();
        Task<bool> AcceptRequest();

        //Task SendUserMessageAsync(string username, string message);
        void SubscribeToMessage(MessageRecivedEventHandler method);
        void SubscribeToDisconnec(DisconnecRecivedEventHandler method);
    }
}
