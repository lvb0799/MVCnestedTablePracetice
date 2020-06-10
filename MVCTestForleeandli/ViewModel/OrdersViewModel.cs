using MVCTestForleeandli.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCTestForleeandli.ViewModel
{
    public class OrdersViewModel
    {
        public Orders Orders { get; set; }
        public List<OrderDetailProductViewModel> OrderDetailProductViewModel { get; set; }
    }
}