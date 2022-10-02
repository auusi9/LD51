﻿using System;
using System.Collections.Generic;
using Code.Configurations;
using Code.Orders;
using Code.Services.Entities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Notifications
{
    public class Notification : MonoBehaviour
    {
        [SerializeField] private NotificationItem _item;
        [SerializeField] private Transform _container;
        [SerializeField] private Image _barFill;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private OrderInterface _orderInterface;

        private Order _order;
        private string _phrase;
        private List<NotificationItem> _notificationItems = new List<NotificationItem>();

        private void Start()
        {
            _orderInterface.OrderUpdated += OrderUpdated;
            _orderInterface.OrderCompleted += OrderCompleted;
            _orderInterface.OrderCancelled += OrderCancelled;
        }

        private void OnDestroy()
        {
            _orderInterface.OrderUpdated -= OrderUpdated;
            _orderInterface.OrderCompleted -= OrderCompleted;
            _orderInterface.OrderCancelled -= OrderCancelled;
        }

        private void OrderUpdated(OrderUpdater obj)
        {
            if (_order != obj.Order)
            {
                return;
            }

            for (var i = 0; i < _notificationItems.Count; i++)
            {
                var notificationItem = _notificationItems[i];
                notificationItem.UpdateNotification(obj.ItemsCompleted[i]);
            }
        }

        private void OrderCompleted(OrderCompleted obj)
        {
            if (_order == obj.Order)
            {
                Destroy(gameObject);
            }
        }

        private void OrderCancelled(OrderCompleted obj)
        {
            if (_order == obj.Order)
            {
                Destroy(gameObject);
            }
        }

        public void SetOrder(Order order)
        {
            _order = order;
            
            if (_audioSource != null)
            {
                _audioSource.Play();
            }

            foreach (var item in order.Items)
            {
                NotificationItem notification = Instantiate(_item, _container);
                notification.SetItem(item);
                _notificationItems.Add(notification);
            }
        }

        private void Update()
        {
            _barFill.fillAmount = (Time.time - _order.OrderCreatedTime) / (_order.OrderExpirationTime - _order.OrderCreatedTime);
            
            if(_order.OrderExpirationTime < Time.time)
            {
                _orderInterface.OrderExpired(_order.Id);
            }
        }
    }
}