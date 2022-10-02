using System.Collections.Generic;
using Code.Services.Entities;

namespace Code.Orders
{
    public class OrderUpdated : OrderCompleted
    {
        

    }

    public class ItemsCompleted
    {
        public ItemEntity ItemEntity { get; }
        public bool Completed;
    }
}