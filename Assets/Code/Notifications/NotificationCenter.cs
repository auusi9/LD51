using System;
using Code.Orders;
using Code.Services.Entities;
using UnityEngine;

namespace Code.Notifications
{
    public class NotificationCenter : MonoBehaviour
    {
        [SerializeField] private Notification _notificationPrefab;
        [SerializeField] private Transform _container;
        [SerializeField] private OrderInterface _orderInterface;

        private void Start()
        {
            _orderInterface.NewOrderCreated += CreateNotification;
        }
        
        private void OnDestroy()
        {
            _orderInterface.NewOrderCreated -= CreateNotification;
        }

        public void CreateNotification(Order order)
        {
            Notification notification = Instantiate(_notificationPrefab, _container);
            notification.SetOrder(order);
        }
    }
}