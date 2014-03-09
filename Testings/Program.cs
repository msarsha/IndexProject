using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DatabaseIndex.Entities;
using DatabaseIndex.Logic;
using System;

namespace Testings
{
    class Program
    {
        static XmlDataAccess<OrderItem> _dataAccess;
        private static IndexService<OrderItem> _service;

        static void Main(string[] args)
        {
            _dataAccess = new XmlDataAccess<OrderItem>(Directory.GetCurrentDirectory() + "\\MyTableData.xml");
            _service = new IndexService<OrderItem>(_dataAccess.GetData());

            _service.CreateIndex("Quantity", item => item.Quantity);


            List<OrderItem> search = _service.RetrieveData("Quantity", 12, 20).ToList();

            _service.CreateIndex("ProductID", item => item.ProductID);

            //search = _service.RetrieveData("UnitPrice", 5m, 10m).ToList();

            _service.CreateIndex("OrderID", item => item.OrderID);

            var row = _service.RetrieveData("OrderID", 10000, 10500);

           // Console.WriteLine(_service.DropIndex("OrderID"));

            var row2 = _dataAccess.GetData().First();

            _dataAccess.Delete(row2);

            OrderItem newrow = new OrderItem
            {
                Discount = 10,
                OrderID = 10,
                ProductID = 10,
                Quantity = 0,
                UnitPrice = 100
            };

            _dataAccess.Insert(newrow);

            var newRowSearch = _service.RetrieveData("Quantity", 0);

            var orQuery = _service.RetrieveData("ProductID", "OrderID", 10, SearchOperation.Or);
            Console.Read();
        }
    }
}
