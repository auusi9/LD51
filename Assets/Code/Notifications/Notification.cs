using System;
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
        [SerializeField] private Image _circleFill;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _orderCancelled;
        [SerializeField] private AudioClip _orderAppear;
        [SerializeField] private OrderInterface _orderInterface;
        [SerializeField] private Animator _animator;
        [SerializeField] private Color _color01;
        [SerializeField] private Color _color02;
        [SerializeField] private Color _color03;

        private Order _order;
        private string _phrase;
        private List<NotificationItem> _notificationItems = new List<NotificationItem>();
        private bool _callbackCalled = false;

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
                _animator.SetTrigger("Completed");
            }
        }

        private void OrderCancelled(OrderCompleted obj)
        {
            if (_order == obj.Order)
            {
                _audioSource.clip = _orderCancelled;
                _audioSource.Play();
                _animator.SetTrigger("Failed");
            }
        }

        public void SetOrder(Order order)
        {
            _order = order;
            
            if (_audioSource != null)
            {
                _audioSource.clip = _orderAppear;
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
            _barFill.fillAmount = 1 - (Time.time - _order.OrderCreatedTime) / (_order.OrderExpirationTime - _order.OrderCreatedTime);
            
            if(_order.OrderExpirationTime < Time.time && !_callbackCalled)
            {
                _callbackCalled = true;
                _orderInterface.OrderExpired(_order.Id);
            }

            UpdateColor();
        }

        private void UpdateColor()
        {
            Color newColor = Color.white;
            if(_barFill.fillAmount < 0.3)
            {
                newColor = _color03;
            }
            else if (_barFill.fillAmount < 0.6)
            {
                newColor = _color02;
            }
            else
            {
                newColor = _color01;
            }
            _barFill.color = newColor;
            _circleFill.color = newColor;
        }
    }
}