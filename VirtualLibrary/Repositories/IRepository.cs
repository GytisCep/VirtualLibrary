﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualLibrary.Repositories
{
    interface IRepository<T>
    {
        IList<T> GetList();
        void Add(T item);
    }
}