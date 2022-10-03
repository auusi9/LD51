﻿using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Code.ConveyorBelts;
using Code.Services.Configurations;
using Code.Services.Entities;
using Random = UnityEngine.Random;

namespace Code.Services
{
    public class OrderGenerator
    {
        private readonly OrderConfigurator _orderConfigurator;
        private readonly BeltItemSpawner.RandomNumberGenerator<int> _randomNumberGenerator;
        
        public OrderGenerator(OrderConfigurator orderConfigurator)
        {
            _orderConfigurator = orderConfigurator;
            
            _randomNumberGenerator = new BeltItemSpawner.RandomNumberGenerator<int>();

            for (int i = 0; i < _orderConfigurator.ShapesAmount; i++)
            {
                _randomNumberGenerator.Add(1f, i);
            }
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
                        _randomNumberGenerator.NextItem(),
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