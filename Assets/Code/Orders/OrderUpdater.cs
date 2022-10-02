using System.Collections.Generic;
using System.Linq;
using Code.Services.Entities;

namespace Code.Orders
{
    public class OrderUpdater : OrderCompleted
    {
        public List<ItemsCompleted> ItemsCompleted { get; }
        public bool IsComplete => ItemsCompleted.All(x => x.Completed);

        public OrderUpdater(Order order, int score, List<ItemsCompleted> itemsCompleted) : base(order, score)
        {
            ItemsCompleted = itemsCompleted;

            for (int i = 0; i < Order.Items.Count; i++)
            {
                itemsCompleted.Add(new ItemsCompleted(Order.Items[i]));
            }
        }
    }

    public class ItemsCompleted
    {
        public ItemEntity ItemEntity { get; }
        public bool Completed;

        public ItemsCompleted(ItemEntity itemEntity)
        {
            ItemEntity = itemEntity;
        }
    }
}