using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JPMtest
{
    class Program
    {
        static List<Sale> storedSales = new List<Sale>();

        static void Main(string[] args)
        {   
            string path = @"TestData.txt";
            string line;
            int messagesCounter = 0;

            Console.WriteLine("Message format: <1>,<2>,<3>,<4>,<5>,<6>");
            Console.WriteLine("<1> - Message Type: 1,2,3");
            Console.WriteLine("<2> - Product: 1-apple,2-orange,3-pear");
            Console.WriteLine("<3> - Cost, pence: ");
            Console.WriteLine("<4> - Amount, items: ");
            Console.WriteLine("<5> - Action, +/-/* : ");
            Console.WriteLine("<6> - Amendment, pence: ");
            Console.WriteLine("Loading messages from file: /Debug/TestData.txt");

            using (var streamReader = new StreamReader(path, Encoding.UTF8))
            {               
                
                while ((line = streamReader.ReadLine()) != null)
                {
                    Console.WriteLine(String.Format("{0}: {1}", messagesCounter, line));

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
                            UpdateSaleDetails(sale); 
                            break;
                        default:
                            
                            break;
                    }

                    messagesCounter++;
                }

                Console.WriteLine(String.Format("Total messages = {0}", messagesCounter));
            }

            Console.WriteLine("Press a key to finish");
            Console.ReadKey();
            
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

        static string ReadFilePath()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.DefaultExt = ".txt";
            dlg.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";

            string path = System.Environment.CurrentDirectory;
            string fileName = "TestData.txt";

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                path = dlg.FileName;
                fileName = dlg.SafeFileName;
            }
            else
            {
                throw (new Exception("Select file TestData.txt"));
            }

            return path;
        }

        static void PrintSale(Sale sale)
        {

        }

    }

    
}
