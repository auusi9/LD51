using System;
using System.Collections.Generic;
using Code.Services.Configurations;
using Code.Services.Entities;
using Random = UnityEngine.Random;

namespace Code.Services
{
    public class OrderGenerator
    {
        private readonly OrderConfigurator _orderConfigurator;

        public OrderGenerator(OrderConfigurator orderConfigurator)
        {
            _orderConfigurator = orderConfigurator;
        }

        public Order NewOrder()
        {
            return new Order(Guid.NewGuid().ToString(), GetItems(), _orderConfigurator.OrderExpirationTime);
        }

        private List<ItemEntity> GetItems()
        {
            int quantity = Random.Range(_orderConfigurator.MinimumItemsXOrder, _orderConfigurator.MaximumItemsXOrder);
            List<ItemEntity> newItems = new List<ItemEntity>(quantity);

            for (int i = 0; i < quantity; i++)
            {
                if (!_orderConfigurator.ItemsHaveDifferentShapes)
                {
                    newItems.Add(new ItemEntity(
                        Random.Range(0, _orderConfigurator.ItemAmount),
                        -1 //Same shape for this item type
                    ));
                }
                else
                {
                                    
                    newItems.Add(new ItemEntity(
                        Random.Range(0, _orderConfigurator.ItemAmount),
                        Random.Range(0, _orderConfigurator.ShapesAmount)
                    ));
                }
            }
            return newItems;
        }
    }
}