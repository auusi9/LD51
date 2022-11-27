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
        [SerializeField] private GameObject _completed;
        [SerializeField] private Color _completedColor;
        
        public void SetItem(ItemEntity itemEntity)
        {
            if (itemEntity.Shape == -1)
            {
                Item itemType = _itemsConfiguration.Shapes[itemEntity.ItemId];
                if (itemType != null)
                {
                    _icon.sprite = itemType.Icon;
                    _icon.SetNativeSize();
                }
            }
        }

        public void UpdateNotification(ItemsCompleted itemsCompleted)
        {
            if (itemsCompleted.Completed)
            {
                _icon.color = _completedColor;
                _completed.SetActive(true);
            }
        }
    }
}