using System;
using System.Collections.Generic;
using System.Text;

namespace sigma_9
{
    class IncorrectInputEventArgs: EventArgs
    {
        public string Name { get; set; }
        public double Price;
        public bool IsPriceCorrect { get; set; } = true;
        public double Weight;
        public bool IsWeightCorrect { get; set; } = true;
        public DateTime CreationDate;
        public bool IsCreationDateCorrect { get; set; } = true;
        public int DaysToExpire;
        public bool IsDaysToExpireCorrect { get; set; } = true;

    }
}
