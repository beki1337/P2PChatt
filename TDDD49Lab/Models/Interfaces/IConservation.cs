using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDDD49Lab.Models.Interfaces
{
    internal interface IConservation<T>
    {
        Task AddMessage(T message);


    }
}
