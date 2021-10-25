using System;
using System.Collections.Generic;
using System.Text;

namespace sigma_9
{
    sealed class Check
    {
        Buy Purchase { get; set; }
        public Check(Buy purchase)
        {
            Purchase = purchase;
        }

        public override string ToString()
        {
            //  Інформація про товар і покупку у вигляді стрічки
            return string.Format("Product name: {0}\nPrice per item: {1}\nWeight per item: {2}\nNumber of items: {3}\n" +
                "Full price: {4}\nFull weight: {5}", Purchase.Product.Name, Purchase.Product.Price, Purchase.Product.Weight,
                Purchase.Number, Purchase.FullPrice, Purchase.FullWeight);
        }
        public override bool Equals(object obj)
        {
            return obj?.ToString() == ToString();
        }
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

    }
}
