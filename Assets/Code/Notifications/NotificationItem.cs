using System.Linq;
using Code.Configurations;
using Code.Items;
using Code.Orders;
using Code.Services.Entities;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Notifications
{
    public class NotificationItem : MonoBehaviour
    {
        [SerializeField] private ItemsConfiguration _itemsConfiguration;
        [SerializeField] private Image _icon;
        [SerializeField] private Image _completed;
        
        public void SetItem(ItemEntity itemEntity)
        {
            if (itemEntity.Shape == -1)
            {
                ItemType itemType = _itemsConfiguration.Types[itemEntity.ItemId];

                Item item = _itemsConfiguration.Shapes.FirstOrDefault(x => x.ItemType == itemType);
                if (item != null)
                {
                    _icon.sprite = item._image.sprite;
                    _icon.SetNativeSize();
                }
            }
        }

        public void UpdateNotification(ItemsCompleted itemsCompleted)
        {
            if (itemsCompleted.Completed)
            {
                _completed.gameObject.SetActive(true);
            }
        }
    }
}