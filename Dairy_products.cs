using System;
using System.Collections.Generic;
using System.Text;

namespace sigma_9
{
    class Dairy_products: Product
    {

        // відсотки, визначені як сталі нормативи складу
        private const double _highestInterest = 10.0;
        private const double _middleInterest = 5.0;
        private const double _lowestInterest = 2.0;

        public Dairy_products() { }
        public Dairy_products(string name, double price, double weight, DateTime creationDate, int daysToExpire) : base(name, price, weight, creationDate, daysToExpire) { }

        public override string ToString()
        {
            return $"Name: {Name}\nPrice: {Price}\nWeight: {Weight}\nExpires after {DaysToExpire} days\n";
        }
        public override bool Equals(object obj)
        {
            return obj?.ToString() == ToString();
        }
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }


        public override void ChangePrice(double interest)
        {
            if (interest <= -100)
            {
                throw new ArgumentException("Price must be greater than zero");
            }

            switch (DaysToExpire)
            {
                case int n when n > 365:        //  Доступно з C#7.0
                    Price += (Price / 100 * (interest + _highestInterest)); 
                    break;
                case int n when n > 90:
                    Price += (Price / 100 * (interest + _middleInterest));
                    break;
                case int n when n > 14:
                    Price += (Price / 100 * (interest + _lowestInterest));
                    break;
                default:
                    Price += (Price / 100 * interest);
                    break;
            }
        }


    }
}
