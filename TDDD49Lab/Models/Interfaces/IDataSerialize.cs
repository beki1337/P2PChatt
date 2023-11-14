﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDDD49Lab.Models.Interfaces
{
    public interface IDataSerialize<T>
    {
        string SerializeToFormat(T obj);

        T DeserializeFromFormat(string data);
    }
}