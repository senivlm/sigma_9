using System;
using System.Collections.Generic;
using System.Text;

namespace sigma_9
{
    class Meat: Product
    {
        // Типи Category та KindOfMeat вкладені в клас Meat
        public enum Category
        { 
            HighestGrade,
            FirstGrade,
            SecondGrade,
            Undefined
        }

        public enum Kind
        {
            Lamb,
            Veal,
            Pork,
            Chicken, 
            Undefined
        }


        // відсотки, визначені як сталі нормативи складу
        private const double _highestInterest = 20.0;
        private const double _middleInterest = 10.0;
        private const double _lowestInterest = 5.0;


        public Category MeatCategory { get; set; } = Category.Undefined;
        public Kind KindOfMeat { get; set; } = Kind.Undefined;

        public Meat() { }
        public Meat(string name, double price, double weight, DateTime creationDate, int daysToExpire) :base(name, price, weight, creationDate, daysToExpire) { }
        public Meat(string name, double price, double weight, Category meatCategory, Kind kindOfMeat, DateTime creationDate, int daysToExpire)
            : base(name, price, weight, creationDate, daysToExpire) 
            {
                MeatCategory = meatCategory;
                KindOfMeat = kindOfMeat;
            }



        public override string ToString()
        {
            return $"Name: {Name}\nPrice: {Price}\nWeight: {Weight}\nCategory: {MeatCategory}\nKind of meat: {KindOfMeat}\n";
        }


        public override bool Equals(object obj)
        {
            // Оскільки ToString() перевантажений таким чином, що стрічки будуть ідентичними лише у випадку,
            // коли всі поля об’єктів ідентичні, ми можемо для простоти порівнювати лише стрічки:
            return obj?.ToString() == ToString();


            // Альтернативний підхід:

            /*if (!(obj is Meat m))
            {
                return false;
            }
            else
            {
                return (MeatCategory == m.MeatCategory)
                    && (KindOfMeat == m.KindOfMeat)
                    && (Price == m.Price)
                    && (Name == m.Name)
                    && (Weight == m.Weight);
            }*/
        }


        // Оскільки по замовчуванню GetHashCode використовує локацію об’єкта для отримання його хешу,
        // при перевантаженні методу Equals потрібно також перевантажити GetHashCode(), щоб для об'єктів з 
        // однаковим значенням, а не адресою, повертався однаковий хеш-код (для однакових стрічок хеш буде ідентичним)
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

            switch(MeatCategory)
            {
                case Category.HighestGrade:
                    Price += (Price / 100 * (interest + _highestInterest));
                    break;
                case Category.FirstGrade:
                    Price += (Price / 100 * (interest + _middleInterest));
                    break;
                case Category.SecondGrade:
                    Price += (Price / 100 * (interest + _lowestInterest));
                    break;
                case Category.Undefined:
                    throw new ArgumentException("You must define meat category first");
            }
        }



    }
}
