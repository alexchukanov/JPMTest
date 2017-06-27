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
            Console.WriteLine("<6> - Amendment, +/-/* pence: ");
            Console.WriteLine();

            //test
            // 1,1,10,0,0,0
            // 1,1,15,0,0,0
            // 2,1,10,250,0,0  mes2
            // 3,1,10,0,+,10   mes3


            Console.WriteLine("Enter E to exit");

            string line;
            bool result = true;
                       
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

            if(words.Count() == 6)
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

                string action = words[4].Trim();
                switch(action)
                {
                    case "-":
                        sale.Action = ActionType.Subtruct;
                        break;
                    case "+":
                        sale.Action = ActionType.Add;
                        break;
                    case "*":
                        sale.Action = ActionType.Multiply;
                        break;
                    default:
                        sale.Action = ActionType.None;
                        //log
                        break;
                }
                                
                int adjustment;
                int.TryParse(words[5], out adjustment);
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
                    saleDetail.SaleMessageType = MessageType.Mes2;
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
            bool result = true;

            var adjList = storedSales.Where(s => s.SaleMessageType == MessageType.Mes2 &&
                                            s.Product == sale.Product 
                                            );

            if (adjList.Count() > 0)
            {
                foreach (var saleDetail in adjList)
                {
                    saleDetail.Action = sale.Action;
                    saleDetail.Adjustment = sale.Adjustment;
                }
            }
            else
            {
                result = false;
            }

            return result;
        }

    }
}
