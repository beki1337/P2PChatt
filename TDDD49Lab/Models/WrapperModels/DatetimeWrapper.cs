using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDDD49Lab.Models.Interfaces;

namespace TDDD49Lab.Models.WrapperModels
{
    internal class DatetimeWrapper : IDateTime
    {

        private DateTime dateTime;
        public DatetimeWrapper(DateTime dateTime) { 
            this.dateTime = dateTime;
        }
        public DateTime Now => dateTime;
    }
}
