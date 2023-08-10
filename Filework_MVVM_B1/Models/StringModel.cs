using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filework_MVVM_B1.Models
{
    internal class StringModel
    {
        public int Id { get; set; }
        public DateOnly Date { get; set; }
        public string LatinLetters { get; set; }
        public string KirilicLetters { get; set; }
        public int IntegerNumber { get; set; }
        public double RealNumber { get; set; }
    }
}
