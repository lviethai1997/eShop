﻿using eShop.Data.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.Data.Entities
{
    public class OrderDetail
    {
        public int OrderID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public Order Order { get; set; }
        public Product Product { get; set; }
    }
}
