using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDDD49Lab.Models.Interfaces;
using static TDDD49Lab.Models.Chatt;

namespace TDDD49Lab.Models
{
    public interface IChatt
    {
       
        Task Serach(ShowMessageBoxDelegate showMessageBox, ITcpClient tcpClient);
        Task StartListingForUsers(ShowMessageBoxDelegate showMessageBox, ITcpListener tcpListener);
        bool IsRunning();
        Task Disconnect();
        Task SendMessage(string message);
        Task SendUserMessage(string username, string message);
        void SubscribeToMessage(MessageRecivedEventHandler method);
    }
}
