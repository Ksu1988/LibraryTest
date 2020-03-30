using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyLibrary.Models
{
    public enum SortState
    {
        NameAsc,    // по имени по возрастанию
        NameDesc,   // по имени по убыванию
        PublishingYearAsc, // по году издания по возрастанию
        PublishingYearDesc   // по году издания по убыванию        
    }
}
