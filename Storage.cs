using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace sigma_9
{
    class Storage
    {
        private List<Product> _data = new List<Product>();
        public event EventHandler<IncorrectInputEventArgs> IncorrectInput;
        public event EventHandler OutstandingSearch;

        public Storage(int size=0)
        {
            if (size < 0) throw new ArgumentOutOfRangeException("You must enter correct array size");

            for(int i = 0; i < size; i++)
            {
                _data.Add(InitializeProduct());
            }
            IncorrectInput += AddIncorrectDataToFile;
            IncorrectInput += ReplaceIncorrectData;
            OutstandingSearch += DeleteOutstanding;
            OutstandingSearch += PrintOutstanding;

        }

        private void Storage_OutstandingSearch(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public Storage(IEnumerable<Product> source)
        {
            _data = source.ToList();
            IncorrectInput += AddIncorrectDataToFile;
            IncorrectInput += ReplaceIncorrectData;
        }

        public Storage(string filename)
        {
            IncorrectInput += AddIncorrectDataToFile;
            IncorrectInput += ReplaceIncorrectData;
            ReadDataFromFile(filename);

        }

        // метод зчитування інформації з файлу з врахуванням винятків.
        public void ReadDataFromFile(string filename)
        {

            try
            {
                using(StreamReader sr = new StreamReader(filename))
                {
                    while(!sr.EndOfStream)
                    {
                        string choice = sr.ReadLine();   // Product||Meat||Dairy_Products
                        string name = sr.ReadLine();
                        double price = double.Parse(sr.ReadLine());
                        double weight = double.Parse(sr.ReadLine());
                        DateTime creationDate = DateTime.Parse(sr.ReadLine());
                        int daysToExpire = int.Parse(sr.ReadLine());

                        switch (choice)
                        {
                            case "Product":
                                _data.Add(new Product(name, price, weight, creationDate, daysToExpire));
                                break;

                            case "Meat":
                                Meat.Category category = (Meat.Category)Enum.Parse(typeof(Meat.Category), sr.ReadLine());
                                Meat.Kind kind = (Meat.Kind)Enum.Parse(typeof(Meat.Kind), sr.ReadLine());
                                _data.Add(new Meat(name, price, weight, category, kind, creationDate, daysToExpire));
                                break;

                            case "Dairy_Products":
                                _data.Add(new Dairy_products(name, price, weight, creationDate, daysToExpire));
                                break;
                        }

                        
                    }
                         
                }
            }
            catch(FileNotFoundException)
            {
                Console.WriteLine($"File {filename} was not found.");
                Console.Read();
                Environment.Exit(-1);
            }
            catch(FormatException)
            {
                Console.WriteLine("Incorrect data in file detected.");
                Console.Read();
                Environment.Exit(-1);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.Read();
                Environment.Exit(-1);
            }
            
        }


  
        // наповнення інформацією даних у режимі діалогу з користувачем,
        // викликається в конструкторі задану кількість разів
        private Product InitializeProduct()
        {
            IncorrectInputEventArgs args = new IncorrectInputEventArgs();
            Console.WriteLine("Press 1 to add new product, press 2 to add new meat, press 3 to add new dairy product.");
            int choice = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter product name:");
            args.Name = Console.ReadLine();

            Console.WriteLine("Enter product price:");
            args.IsPriceCorrect = double.TryParse(Console.ReadLine(), out args.Price);
            if (!args.IsPriceCorrect)
            {
                OnIncorrectInput(args);
                return null;
            }


            Console.WriteLine("Enter product weight:");
            args.IsWeightCorrect = double.TryParse(Console.ReadLine(), out args.Weight);
            if (!args.IsWeightCorrect)
            {
                OnIncorrectInput(args);
                return null;
            }

            Console.WriteLine("Enter creation date:");
            args.IsCreationDateCorrect = DateTime.TryParse(Console.ReadLine(), out args.CreationDate);
            if (!args.IsCreationDateCorrect)
            {
                OnIncorrectInput(args);
                return null;
            }

            Console.WriteLine("Enter days to expire:");
            args.IsDaysToExpireCorrect = int.TryParse(Console.ReadLine(), out args.DaysToExpire);
            if (!args.IsCreationDateCorrect)
            {
                OnIncorrectInput(args);
                return null;
            }



            try
            {
                switch (choice)
                {
                    case 1:
                        return new Product(args.Name, args.Price, args.Weight, args.CreationDate,  args.DaysToExpire);
                    case 2:
                        Console.WriteLine("Enter meat category");
                        Meat.Category category = (Meat.Category)Enum.Parse(typeof(Meat.Category), Console.ReadLine());

                        // Значення чутливі до регістру

                        Console.WriteLine("Enter kind of meat");
                        Meat.Kind kind = (Meat.Kind)Enum.Parse(typeof(Meat.Kind), Console.ReadLine());

                        return new Meat(args.Name, args.Price, args.Weight, category, kind, args.CreationDate, args.DaysToExpire);
                    case 3:

                        return new Dairy_products(args.Name, args.Price, args.Weight, args.CreationDate, args.DaysToExpire);
                    default:
                        throw new ArgumentException("Your choice is incorrect");
                }
                
            }
            catch(FormatException fe)
            {
                Console.WriteLine(fe.Message);
                Console.Read();
                Environment.Exit(-1);
                return null;
            }

        }


        public override string ToString()
        {
            EventArgs args = new EventArgs();
            OnOutstandingSearch(args);
            string res = "";
            foreach (Product prod in _data)
            {
                res+=prod;
            }
            return res;
        }


        public IEnumerable<Meat> FindAllMeat()
        {
            foreach (Product prod in _data)
            {
                if(prod is Meat meat)
                {
                    yield return meat;
                }
            }
        }


        public void ChangePrice(double interest)
        {
            if (_data == null) throw new ArgumentNullException("No list was specified");
            foreach (Product product in _data)
            {
                product.ChangePrice(interest);
            }    
        }


        // метод, який для біжучої дати вилучає молочні продукти, термін придатності яких вичерпався
        public void DeleteOutstanding()
        { 
             List<Product> expired = (List<Product>)
                from prod in _data
                where (prod.CreationDate + TimeSpan.FromDays(prod.DaysToExpire) < DateTime.Now && prod is Dairy_products)
                select prod;

            _data = (List<Product>)
                from prod in _data
                where (prod.CreationDate + TimeSpan.FromDays(prod.DaysToExpire) > DateTime.Now && prod is Dairy_products)
                select prod;


            Console.WriteLine("Choose file name");
            string fileName = Console.ReadLine();

            using(StreamWriter sw = new StreamWriter(fileName))
            {
                 foreach (Product prod in expired)
                 {
                     sw.WriteLine(prod);
                 }
            }
        }


        public Product this[int i]
        {
            get 
            {
                if (i < 0 || i > _data.Count - 1) throw new IndexOutOfRangeException("Element with specified index doesn't exist");
                return _data[i];
            }
            set
            {
                if (i < 0 || i > _data.Count - 1) throw new IndexOutOfRangeException("Element with specified index doesn't exist");
                _data[i] = value;
            }
        }


        public static IEnumerable<Product> Intersect(Storage st1, Storage st2)
        {
            return Enumerable.Intersect(st1._data, st2._data);
        }

        public static IEnumerable<Product> Difference(Storage st1, Storage st2)
        {
            return st1._data.Except(st2._data);
        }

        public static IEnumerable<Product> SymmetricDifference(Storage st1, Storage st2)
        {
            return st1._data.Except(st2._data).Union(st2._data.Except(st1._data));
        }

        public void Add(Product p)
        {
            _data.Add(p);
        }

        public int Remove(string name)
        {
            return _data.RemoveAll(p => p.Name == name);
        }
             
        public IEnumerable<Product> FindByName(string name)
        {
            return _data.FindAll(p => p.Name == name);
        }
        public IEnumerable<Product> FindByPrice(double price)
        {
            return _data.FindAll(p => p.Price == price);
        }
        public IEnumerable<Product> FindByWeight(double weight)
        {
            return _data.FindAll(p => p.Weight == weight);
        }

        protected virtual void OnIncorrectInput(IncorrectInputEventArgs e)
        {
            EventHandler<IncorrectInputEventArgs> handler = IncorrectInput;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        protected virtual void OnOutstandingSearch(EventArgs e)
        {
            EventHandler handler = OutstandingSearch;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        void AddIncorrectDataToFile(object sender, IncorrectInputEventArgs args)
         {
            using (StreamWriter sw = new StreamWriter("log.txt", true))
            {
                if (!args.IsPriceCorrect)
                {
                    sw.WriteLine(args.Price);
                }

                if (!args.IsWeightCorrect)
                {
                    sw.WriteLine(args.Weight);
                }

                if (!args.IsCreationDateCorrect)
                {
                    sw.WriteLine(args.CreationDate);
                }


                if (!args.IsDaysToExpireCorrect)
                {
                    sw.WriteLine(args.DaysToExpire);
                }
                sw.WriteLine(DateTime.Now);
            }
            

         }
         void ReplaceIncorrectData(object sender, IncorrectInputEventArgs args)
        {
            if (!args.IsPriceCorrect)
            {
                Console.WriteLine("Enter product price:");
                args.IsPriceCorrect = double.TryParse(Console.ReadLine(), out args.Price);
            }

            if (!args.IsWeightCorrect)
            {
                Console.WriteLine("Enter product weight:");
                args.IsWeightCorrect = double.TryParse(Console.ReadLine(), out args.Weight);
            }

            if (!args.IsCreationDateCorrect)
            {
                Console.WriteLine("Enter creation date:");
                args.IsCreationDateCorrect = DateTime.TryParse(Console.ReadLine(), out args.CreationDate);
            }


            if (!args.IsDaysToExpireCorrect)
            {
                Console.WriteLine("Enter days to expire:");
                args.IsDaysToExpireCorrect = int.TryParse(Console.ReadLine(), out args.DaysToExpire);
            }
            _data.Add(new Product(args.Name, args.Price, args.Weight, args.CreationDate, args.DaysToExpire));
        }



        private void DeleteOutstanding(object sender, EventArgs e)
        {
            foreach (Product p in _data)
            {
                if(p.CreationDate+TimeSpan.FromDays(p.DaysToExpire)<DateTime.Now)
                {
                    _data.Remove(p);
                }
            }
        }

        void PrintOutstanding(object sender, EventArgs e)
        {
            using (StreamWriter sw = new StreamWriter("log.txt", true))
            {
                foreach (Product p in _data)
                {
                    if (p.CreationDate + TimeSpan.FromDays(p.DaysToExpire) < DateTime.Now)
                    {
                        sw.WriteLine(p);
                    }
                }
            }
        }


        // Хендлери подій
    }
}
