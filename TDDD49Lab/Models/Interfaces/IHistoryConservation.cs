using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDDD49Lab.Models.Interfaces
{
    public interface IHistoryConservation<T>
    {

        Task<List<string>> GetConservationsAsync();

        Task<T> GetConservationAsync(string fileName);


    }
}
