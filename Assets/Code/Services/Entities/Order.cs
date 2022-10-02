using System.Collections.Generic;
using UnityEngine;

namespace Code.Services.Entities
{
    public class Order
    {
        public string Id { get; }
        public float OrderCreatedTime { get; }
        public float OrderExpirationTime { get; }
        public List<ItemEntity> Items { get; }

        public Order(string id, List<ItemEntity> items, float orderExpirationTime)
        {
            Id = id;
            OrderCreatedTime = Time.time;
            OrderExpirationTime = OrderCreatedTime + orderExpirationTime;
            Items = items;
        }
    }
}