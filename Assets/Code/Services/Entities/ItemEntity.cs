using System;

namespace Code.Services.Entities
{
    public class ItemEntity
    {
        public int ItemId { get; }
        public int Shape { get; }

        public ItemEntity(int itemId, int shape)
        {
            ItemId = itemId;
            Shape = shape;
        }

        public override bool Equals(object obj)
        {
            var item = obj as ItemEntity;

            if (item == null)
            {
                return false;
            }
            
            return Equals(item);
        }

        private bool Equals(ItemEntity other)
        {
            return ItemId == other.ItemId && Shape == other.Shape;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ItemId, Shape);
        }
    }
}