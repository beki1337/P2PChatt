using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDDD49Lab.Models.Interfaces;

namespace TDDD49Lab.Models.WrapperModels
{
    public class DatetimeWrapper : IDateTime
    {

      
        public DateTime Now => DateTime.Now;
    }
}
