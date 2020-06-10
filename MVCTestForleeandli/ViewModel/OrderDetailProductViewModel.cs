using MVCTestForleeandli.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCTestForleeandli.ViewModel
{
    public class OrderDetailProductViewModel
    {
        public OrderDetails OrderDetail
        {
            get;
            set;
        }
        public Products Product
        {
            get;
            set;
        }
    }
}