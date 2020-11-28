using System;
using System.Linq;
using Models;
using SqlSugar;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {

            Order od = new Order();
            string ddNo = "20201128";
            decimal count = 0;
            int sumc = 0;
            OrderDetail detail = new OrderDetail() { OrderNo = ddNo };
            var shops = DB.DBcontext.Queryable<Shop, Goods>((sp, gd) => sp.ID == gd.ShopID)
                .Where((sp, gd) => gd.ShopID == "dpid1")
                .Select((sp, gd) => new
                {
                    ShopID = sp.ID,
                    GoodsID = gd.ID,
                    sp.ShopName,
                    sp.LinkName,
                    gd.GoodsName,
                    gd.GoodsNo,
                    gd.GoodsPrice
                }).ToList();

            foreach (var shop in shops)
            {
                detail.GoodsID = shop.GoodsID;
                detail.ID = Guid.NewGuid().ToString();
                detail.OrderNumber = 2;
                detail.Price = shop.GoodsPrice;
                DB.DBcontext.Insertable(detail).ExecuteCommand();
                count += shop.GoodsPrice.Value;
                sumc += detail.OrderNumber.Value;
            }
            od.UserID = "123";
            od.ShopID = shops[0].ShopID;
            od.ID = Guid.NewGuid().ToString();
            od.OrderMoney = count;
            od.OrderNumber = sumc.ToString();
            od.OrderNo = ddNo;
            DB.DBcontext.Insertable(od).ExecuteCommand();
            //DB.DBcontext.DbFirst.IsCreateAttribute().CreateClassFile("E:\\m\\Solution1\\ConsoleApp1\\Models", "Models");
        }
    }
}
