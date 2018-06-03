using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using ThunderOnlineModule.Models;

namespace ThunderOnlineModule.Controllers
{
    [RoutePrefix("api/sales")]
    public class ProductSalesController : ApiController
    {
        private ZionEntities db = new ZionEntities();

        [Route("getclient/{id}")]
        public ClientDisplay GetClient(string id)
        {
            tcClient item = db.tcClients.Find(id.Trim());
            if (clientExists(id))
            {
                ClientDisplay client = new ClientDisplay() { Id = item.Id, Name = item.Name };
                return client;
            }
            return null;
        }

        [Route("getClientByName/{name}")]
        public List<ClientDisplay> GetClients(string name)
        {
            var list = new List<ClientDisplay>();
            foreach (var item in db.tcClients.Where(c => c.Name.Contains(name)).ToList())
            {
                var client = new ClientDisplay() { Id = item.Id, Name = item.Name };
                list.Add(client);
            }
            return list;
        }

        [Route("getProduct/{id}")]
        public tcProduct GetProduct(int id)
        {
            if (!productExists(id)) { return null; }
            var tcProduct = db.tcProducts.Find(id);
            var product = new tcProduct() { Id = tcProduct.Id, Name = tcProduct.Name, Cost = tcProduct.Cost, Price = tcProduct.Price, Stock = tcProduct.Stock };
            return product;
        }

        [Route("getService/{id}")]
        public tcService GetService(int id)
        {
            if (!serviceExists(id)) { return null; }
            var tcService = db.tcServices.Find(id);
            var service = new tcService() { Id = tcService.Id, Name = tcService.Name, Price = tcService.Price };
            return service;
        }

        [Route("postSaleService/{sale}")]
        public string PostSaleService(string sale)
        {
            var billId = NewBill();
            var lines = sale.Split('!');
            foreach (var item in lines)
            {
                var fields = item.Split(',');
                var sl = new tcServiceSale()
                {
                    ServiceId = int.Parse(fields[0]),
                    Quantity = int.Parse(fields[1]),
                    Price = Convert.ToDecimal(fields[2]),
                    BillId = billId,
                };
                db.tcServiceSales.Add(sl);
            }
            db.SaveChanges();
            return billId.ToString();
        }

        [Route("postSale/{sale}")]
        public string PostSale(string sale)
        {
            var billId = NewBill();
            var lines = sale.Split('!');
            foreach (var item in lines)
            {
                var fields = item.Split(',');
                var sl = new tcProductSale()
                {
                    ProductId = int.Parse(fields[0]),
                    Quantity = int.Parse(fields[1]),
                    Price = Convert.ToDecimal(fields[2]),
                    BillId = billId
                };
                db.tcProductSales.Add(sl);
                var product = db.tcProducts.Find(sl.ProductId);
                product.Stock = product.Stock - sl.Quantity;
                db.Entry(product).State = EntityState.Modified;
            }
            db.SaveChanges();
            return billId.ToString();
        }

        [Route("newBill")]
        public int NewBill()
        {
            var bill = new tcBill();
            db.tcBills.Add(bill);
            db.SaveChanges();
            return bill.Id;
        }

        [Route("SetBillDetails/{data}")]
        public int SetBillDetails(string data)
        {
            var values = data.Split(',');
            var id = Convert.ToInt32(values[0]);
            var bill = db.tcBills.Find(id);
            bill.ClientId = values[1];
            bill.PaymentMethod = int.Parse(values[2]);
            if (values[3] == "true")
            {
                bill.IsCredit = true;
            }
            else
            {
                bill.IsCredit = false;
            }
            bill.Date = DateTime.Today;
            bill.User = values[4]; 

            if (bill == null) { return 0; }

            db.Entry(bill).State = EntityState.Modified;
            try { db.SaveChanges(); }
            catch (Exception) { throw; }

            return bill.Id;
        }

        [Route("postClientDetail/{clientDetail}")]
        public string PostClientDetail(string clientDetail)
        {
            var lines = clientDetail.Split(',');
            var detail = new tcClientDetail() { ClientId = lines[0], Credit = Convert.ToDecimal(lines[1]), Payment = Convert.ToDecimal(lines[2]), BillId = int.Parse(lines[3]) };
            db.tcClientDetails.Add(detail);
            db.SaveChanges();
            return "Done";
        }

        [Route("getBillDetails/{id}")]
        public tcBill GetBillDetails(int id)
        {
            var bill = db.tcBills.Find(id);

            var products = new List<tcProductSale>();
            foreach (var item in bill.tcProductSales)
            {
                var p = new tcProductSale() { Id = item.Id, BillId = item.Id, Price = item.Price, ProductId = item.ProductId, Quantity = item.Quantity, tcProduct = new tcProduct() { Name = item.tcProduct.Name } };
                products.Add(p);
            }

            var services = new List<tcServiceSale>();
            foreach (var item in bill.tcServiceSales)
            {
                var s = new tcServiceSale() { Id = item.Id, BillId = item.BillId, Price = item.Price, Quantity = item.Quantity, ServiceId = item.ServiceId, tcService = new tcService() { Name = item.tcService.Name } };
                services.Add(s);
            }

            var payments = new List<tcClientDetail>();
            foreach (var item in bill.tcClientDetails)
            {
                var p = new tcClientDetail()
                {
                    BillId = item.BillId,
                    ClientId = item.ClientId,
                    Id = item.Id,
                    Credit = item.Credit,
                    Payment = item.Payment,
                    PaymentMethod = item.PaymentMethod
                };
                payments.Add(p);
            }
            
            var result = new tcBill()
            {
                Id = bill.Id,
                ClientId = bill.ClientId,
                tcClient = new tcClient() { Id = bill.ClientId, Name = bill.tcClient.Name, Group = bill.tcClient.Group },
                IsCredit = bill.IsCredit,
                PaymentMethod = bill.PaymentMethod,
                User = bill.User,
                tcProductSales = products,
                tcServiceSales = services,
                tcClientDetails = payments
            };
            return result;
        }

        [Route("getClientBalance/{id}")]
        public decimal? GetClientBalance(string id)
        {
            var client = db.tcClients.Find(id);
            decimal? payments = 0;
            decimal? credit = 0;
            foreach (var item in client.tcClientDetails)
            {
                payments = payments + item.Payment;
                credit = credit + item.Credit;
            }
            decimal? balance = credit - payments;

            return balance;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool productExists(int id)
        {
            return db.tcProducts.Count(e => e.Id == id) > 0;
        }

        private bool serviceExists(int id)
        {
            return db.tcServices.Count(e => e.Id == id) > 0;
        }

        private bool clientExists(string id)
        {
            return db.tcClients.Count(e => e.Id.Trim() == id.Trim()) > 0;
        }

        bool IsAllowed()
        {
            try
            {
                var access = System.Web.HttpContext.Current.Session["logged"].ToString();
                if (access != null) return true;
            }
            catch (Exception)
            {
                return false;
            }
            return false;
        }
    }
}