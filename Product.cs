using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace sigma_9
{
    class Product: IEquatable<Product>
    {
        private double _price;
        private double _weight;
        private int _daysToExpire;
        private DateTime _creationDate;
        public string Name { get; set; }   

        public int DaysToExpire
        {
            get
            {
                return _daysToExpire;
            }
            set
            {
                if(value>0)
                {
                    _daysToExpire = value;
                }
                else
                {
                    throw new ArgumentException("Invalid expiration date");
                }
            }
        }

        public DateTime CreationDate
        {
            get
            {
                return _creationDate;
            }
            set
            {
                if (value > DateTime.Now) throw new ArgumentException("Incorrect creation date");
                _creationDate = DateTime.Now;
            }
        }


        public double Price
        {
            get
            {
                return _price;
            }
            set
            {
                if(value>0) 
                {
                    _price = value;
                }
                else
                {
                    throw new ArgumentException("Price must be greater than zero");
                }
            }
        }

        public double Weight
        {
            get
            {
                return _weight;
            }
            set
            {
                if (value > 0)
                {
                    _weight = value;
                }
                else
                {
                    throw new ArgumentException();
                }
            }
        }


        public Product(string name, double price, double weight, DateTime creationDate, int daysToExpire)
        {
            Name = name;
            Price = price;
            Weight = weight;
            CreationDate = creationDate.Date;
            DaysToExpire = daysToExpire;
        }

        public Product() : this("Test product", 1, 1, DateTime.Now-TimeSpan.FromDays(10), 50) { }


        public virtual void ChangePrice(double interest)
        {
            if(interest<=-100)  //  вважаємо, що ціна не може бути <= 0
            {
                throw new ArgumentException("Price must be greater than zero");
            }
            Price += (Price / 100 * interest);

        }


        static public Product Parse(string s)
        {

            try
            {
                string[] data = s.Split(" ");
                Product prod = new Product
                {
                    Name = data[0],
                    Price = double.Parse(data[1]),
                    Weight = double.Parse(data[2]),
                    CreationDate = DateTime.Parse(data[3]),
                    DaysToExpire = int.Parse(data[4]),
                };
                return prod;

            }
            catch(FormatException fe)
            {
                Console.WriteLine(fe.Message);
                
            }
            catch(IndexOutOfRangeException ie)
            {
                Console.WriteLine(ie.Message);
            }
            catch(Exception)
            {
                Console.WriteLine("Unhandled exception occured.");
            }

            return new Product();
        }



        public override string ToString()
        {
            return $"Name: {Name}\nPrice: {Price}\nWeight: {Weight}\nCreation date: {CreationDate}\n";
        }
        public override bool Equals(object obj)
        {
            // Було задано для класу Product порівнювати лише імена продуктів
            return Name == (obj as Product)?.Name;
        }
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public bool Equals(Product other)
        {
            return Name == other?.Name;
        }
    }
}
