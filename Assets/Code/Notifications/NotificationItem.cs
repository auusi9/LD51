using Code.Configurations;
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