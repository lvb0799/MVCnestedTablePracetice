using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVCTestForleeandli.Models;
using MVCTestForleeandli.ViewModel;

namespace MVCTestForleeandli.Controllers
{
    public class OrdersController : Controller
    {
        private NorthwindContext db = new NorthwindContext();
        // GET: Orders
        public async Task<ActionResult> Index()
        {
            var master= await db.Orders.ToListAsync();
            var detail = await (from od in db.OrderDetails
                                join p in db.Products
                                on od.ProductID equals p.ProductID
                                select new OrderDetailProductViewModel { OrderDetail = od, Product = p }).ToListAsync();
            List<OrdersViewModel> allData = new List<OrdersViewModel>();
            foreach (var order in master)
            {
                List<OrderDetailProductViewModel> SubDetails = detail.FindAll(item => item.OrderDetail.OrderID == order.OrderID);
                OrdersViewModel ovm = new OrdersViewModel();
                ovm.Orders = order;
                ovm.OrderDetailProductViewModel = SubDetails;
                allData.Add(ovm);
            }
            return View(allData);
        }
        [HttpPost]
        public async Task<ActionResult> CreateData([System.Web.Http.FromBody]OrderDetailProductViewModel orderDetailProductViewModel)
        {
            if (ModelState.IsValid)
            {
                int orderid = orderDetailProductViewModel.OrderDetail.OrderID;
                int productid = orderDetailProductViewModel.Product.ProductID;
                if (await db.OrderDetails.Where(x => x.OrderID == orderid && x.ProductID == productid).CountAsync() == 0)
                {
                    db.OrderDetails.Add(orderDetailProductViewModel.OrderDetail);
                    if (await db.Products.Where(x => x.ProductID == productid).CountAsync() == 0)
                        db.Products.Add(orderDetailProductViewModel.Product);
                    await db.SaveChangesAsync();
                }
                return RedirectToAction("Index");
            }

            return View(orderDetailProductViewModel);
        }
        [HttpPost]
        public async Task<ActionResult> UpdateData([System.Web.Http.FromBody]OrderDetailProductViewModel orderDetailProductViewModel)
        {
            if (ModelState.IsValid)
            {
                int orderid = orderDetailProductViewModel.OrderDetail.OrderID;
                int productid = orderDetailProductViewModel.Product.ProductID;
                db.Entry(orderDetailProductViewModel.OrderDetail).State = EntityState.Modified;
                db.Entry(orderDetailProductViewModel.Product).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(orderDetailProductViewModel);
        }
        [HttpPost]
        public async Task<ActionResult> DeleteData([System.Web.Http.FromBody]int orderid,int productid)
        {
            if (ModelState.IsValid)
            {
                //int orderid = orderDetailProductViewModel.OrderDetail.OrderID;
                //int productid = orderDetailProductViewModel.Product.ProductID;
                OrderDetails orderDetails=await db.OrderDetails.FindAsync(orderid, productid);
                db.OrderDetails.Remove(orderDetails);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(new object());
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
