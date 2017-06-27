using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JPMtest
{
    class Program
    {
        static List<Sale> storedSales = new List<Sale>();

        static void Main(string[] args)
        {
            Console.WriteLine("Message format: <1>,<2>,<3>,<4>,<5>");
            Console.WriteLine("<1> - Message Type: 1,2,3");
            Console.WriteLine("<2> - Product: 1-apple,2-orange,3-pear");
            Console.WriteLine("<3> - Cost, pence: ");
            Console.WriteLine("<4> - Amount, items: ");
            Console.WriteLine("<5> - Amendment, +/- pence: ");
            Console.WriteLine();

            //test
            // 1,1,10,0,0
            // 1,1,15,0,0
            // 2,1,10,250,0


            Console.WriteLine("Enter E to exit");

            string line;
            bool result = true;

            List<Sale> storedSales = new List<Sale>();

            while ((line = Console.ReadLine()) != "E")
            {
                Sale sale = ParseMessage(line);                

                switch (sale.SaleMessageType)
                {
                    case MessageType.Mes1:
                        storedSales.Add(sale);
                        break;
                    case MessageType.Mes2:
                        AddSaleDetails(sale); 
                        break;
                    case MessageType.Mes3:
                        //UpdateSaleDetails(sale); 
                        break;
                    default:
                        result = false;
                        break;
                }

                Console.WriteLine(result);
                Console.WriteLine("bla bla - enter E to exit");
            }

        }

        static Sale ParseMessage(string line)
        {
            Sale sale = null;

            string[] words = line.Split(',');

            if(words.Count() == 5)
            {
                sale = new Sale();

                int mt; 
                int.TryParse(words[0], out mt);                
                sale.SaleMessageType = (MessageType)mt;

                int product;
                int.TryParse(words[1], out product);
                sale.Product = (ProductType)product;

                int price;
                int.TryParse(words[2], out price);
                sale.Price = price;

                int amount;
                int.TryParse(words[3], out amount);
                sale.Amount = amount;

                int adjustment;
                int.TryParse(words[4], out adjustment);
                sale.Adjustment = adjustment;
                
            }
            else
            {
                //log
            }

            return sale;
        }

        static bool AddSaleDetails(Sale sale)
        {
            bool result = true;

            var detailsList = storedSales.Where(s => s.SaleMessageType == MessageType.Mes1 &&
                                            s.Product == sale.Product &&
                                            s.Price == sale.Price);

            if (detailsList.Count() > 0)
            {
                foreach (var saleDetail in detailsList)
                {
                    saleDetail.Amount = sale.Amount;
                }
            }
            else
            {
                result = false;
            }

            return result;
        }

        static bool UpdateSaleDetails(Sale sale)
        {
            return false;
        }

    }
}
