﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualLibrary.Enums;
using VirtualLibrary.View;

namespace VirtualLibrary.Model
{
    class Book : IBook
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Code { get; set; }
        public int DaysForBorrowing { get; set; }

        public bool IsTaken { get; set; }
        public string TakenByUser { get; set; }
        public DateTime TakenWhen { get; set; }
        public DateTime HasToBeReturned { get; set; }

        //TODO: 
        //public BookGenre Genre { get; set}
    }

    
}
