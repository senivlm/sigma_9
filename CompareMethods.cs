using System;
using System.Collections.Generic;
using System.Text;
using sigma_9;

namespace sigma_9
{
    static class CompareMethods
    {
        public static int CompareByName(object obj1, object obj2) 
        {
            if(obj1 is Product prod1 && obj2 is Product prod2)
            {
                 return string.Compare(prod1.Name, prod2.Name);
            }
            else
            {
                throw new ArgumentException("Unable to cast objects to Product type");
            }
        }
        public static int CompareByPrice(object obj1, object obj2)
        {
            if (obj1 is Product prod1 && obj2 is Product prod2)
            {
                if (prod1.Price > prod2.Price) return 1;
                else if (prod1.Price == prod2.Price) return 0;
                else return -1;
            }
            else
            {
                throw new ArgumentException("Unable to cast objects to Product type");
            }
        }
    }
}
