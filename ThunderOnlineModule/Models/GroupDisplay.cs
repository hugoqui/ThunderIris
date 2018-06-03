using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThunderOnlineModule.Models
{
    public class GroupDisplay
    {
        ZionEntities db = new ZionEntities();
        public string Name { get; set; }
        public GroupDisplay(int id)
        {
            Name = GetName(id);
        }

        string GetName(int id)
        {
            tcGroup group = db.tcGroups.Find(id);
            return group.Name;
        }
    }

    public class ClientDisplay
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class Sale
    {
        public int ProductId { get; set; }
        public int ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public int BillId { get; set; }
    }

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }

}