using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commercial_Spoils_App
{
    class Generics<T> where T : IComparable
    {
        public T FirstNumber { get; set; }

        public T LastNumber { get; set; }

        public T SwitchType(T x, T y)
        {
            if (FirstNumber != null)
            {
                return x = FirstNumber;
            }
            else
            {
                return y = LastNumber;
            }
        }
    }
}
