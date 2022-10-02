using Code.Services.Entities;
using UnityEngine;

namespace Code.Notifications
{
    public class NotificationCenter : MonoBehaviour
    {
        [SerializeField] private Notification _notificationPrefab;
        [SerializeField] private Transform _container;

        public void CreateNotification(Order order)
        {
            Notification notification = Instantiate(_notificationPrefab, _container);
            notification.SetOrder(order);
        }
    }
}