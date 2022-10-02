using System;
using System.Collections.Generic;
using Code.Boxes;
using Code.Services;
using Code.Services.Entities;
using UnityEngine;

namespace Code.Orders
{
    public class OrderInterface : ScriptableObject
    {
        private OrderGenerator _orderGenerator;

        private List<Order> _currentOrders = new List<Order>();

        public Action<OrderCompleted> OrderCompleted;
        public Action<OrderUpdated> OrderUpdated;
        public Action<Order> NewOrderCreated;

        public void NewOrder()
        {
            Order order = _orderGenerator.NewOrder();
            _currentOrders.Add(order);
            NewOrderCreated?.Invoke(order);
        }

        public void BoxSent(Box box)
        {
            
        }

        public void OrderExpired(string id)
        {
            
        }
    }
}