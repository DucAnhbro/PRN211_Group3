﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
namespace DataAccess.Repository
{
    public interface IOrderDetailRepository
    {
        IEnumerable<OrderDetailObject> GetOrderDetail();
        OrderDetailObject GetOrderDetailByOrderID(int orderID);
        void InsertOrderDetail(OrderDetailObject orderDetail);
        void UpdateOrderDetail(OrderDetailObject orderDetail);
        void DeleteOrderDetail(int orderID);

    }


}
