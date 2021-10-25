using System;
using System.Collections.Generic;
using System.Text;

namespace sigma_9
{
    class Buy
    {

        private int _number;
        private double _fullPrice;
        private double _fullWeight;

        public Product Product { get; set; }
        public int Number
        {
            get
            {
                return _number;
            }
            set
            {
                if(value>0)
                {
                    _number = value;
                }
                else
                {
                    throw new ArgumentException("Number of elements must be greater than zero");
                }
            }
        }

        public double FullPrice
        {
            get
            {
                return _fullPrice;
            }
            private set     // Оскільки повна ціна залежить від кількості товару та ціну за 1шт., сетер робимо приватним, 
                            // щоб не можна було змінити повну ціну за межами класу
            {
                _fullPrice = value;
            }
        }

        public double FullWeight
        {
            get
            {
                return _fullWeight;
            }
            private set 
            {
                _fullWeight = value;
            }
        }


        public Buy(Product product, int number=1)
        {
            Product = product;
            Number = number;
            FullPrice = product.Price * Number;
            FullWeight = product.Weight * Number;
        }

        public Buy() : this(new Product { Name = "Default", Price = 100, Weight = 0.8 }, 1) { }
    }
}
