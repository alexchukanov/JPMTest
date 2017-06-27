using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JPMtest
{
    class Model
    {
    }

    enum ProductType
    {
        None,
        Apple,
        Orange,
        Melon,
        Pie,
        Bananas
    }

    class Sale
    {
        int Id { get; set; }
        public ProductType Product { get; set;}
        public int Amount { get; set; }

        int price = 0;
        public int Price
        {
            get { return price; }
            set { if (value < 0) price = 0; }
        }

        public ActionType Action { get; set; }
        public int Adjustment { get; set; }
        public MessageType SaleMessageType { get; set; }

        public bool SaleAdjustment(Sale saleAdj)
        {
            bool result = true;

            if (saleAdj.SaleMessageType == MessageType.Mes3)
            {
                switch(saleAdj.Action)
                {
                    case ActionType.Add:
                        Price += saleAdj.Adjustment;
                        //WriteLog();
                        break;
                    case ActionType.Subtruct:
                        Price -= saleAdj.Adjustment;
                        //if (Cost < 0)  WriteLog();
                            break;
                    case ActionType.Multiply:
                        Price *= saleAdj.Adjustment;
                        //WriteLog();
                        break;
                    default:
                        result = false;
                        //WriteLog();
                        break;
                }
                
            }
            else
            {
                //WriteLog();
                result = false;
            }


            return result;
        }
    }

    enum ActionType
    {
        None,
        Add,
        Subtruct,
        Multiply
    }

    enum MessageType
    {
        None,
        Mes1,
        Mes2,
        Mes3
    }

}
