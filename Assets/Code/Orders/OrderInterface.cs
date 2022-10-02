using System;
using System.Collections.Generic;
using System.Linq;
using Code.Boxes;
using Code.Configurations;
using Code.Items;
using Code.Services;
using Code.Services.Configurations;
using Code.Services.Entities;
using UnityEngine;

namespace Code.Orders
{
    [CreateAssetMenu(menuName = "Orders/OrderInterface", fileName = "OrderInterface", order = 0)]
    public class OrderInterface : ScriptableObject
    {
        [SerializeField] private ItemsConfiguration _itemsConfiguration;
        [SerializeField] private int _maximumItemsXOrder = 4;
        [SerializeField] private int _minimumItemsXOrder = 1;
        [SerializeField] private bool _itemsHaveDifferentShapes = false;
        [SerializeField] private float _orderExpirationTime = 30f;
        private OrderGenerator _orderGenerator;

        private List<OrderUpdater> _currentOrders = new List<OrderUpdater>();

        public Action<OrderCompleted> OrderCompleted;
        public Action<OrderCompleted> OrderCancelled;
        public Action<OrderUpdater> OrderUpdated;
        public Action<Order> NewOrderCreated;

        private void OnEnable()
        {
            _orderGenerator = new OrderGenerator(new OrderConfigurator()
            {
                ItemAmount = _itemsConfiguration.TypeAmount,
                ShapesAmount = _itemsConfiguration.ShapesAmount,
                MaximumItemsXOrder = _maximumItemsXOrder,
                MinimumItemsXOrder = _minimumItemsXOrder,
                ItemsHaveDifferentShapes = _itemsHaveDifferentShapes,
                OrderExpirationTime = _orderExpirationTime
            });
        }

        public void NewOrder()
        {
            Order order = _orderGenerator.NewOrder();
            _currentOrders.Add(new OrderUpdater(order, 0, new List<ItemsCompleted>()));
            NewOrderCreated?.Invoke(order);
        }

        public void BoxSent(Box box)
        {
            List<Item> items = box.GetItems();

            if (items.Count == 0 || _currentOrders.Count == 0)
            {
                Destroy(box.gameObject);
                return;
            }

            List<int> itemsIds = items.Select(x => Array.IndexOf(_itemsConfiguration.Types, x.ItemType)).ToList();
            List<int> shapes = new List<int>();

            if (_itemsHaveDifferentShapes)
            {
                shapes = items.Select(x => Array.IndexOf(_itemsConfiguration.Shapes,  _itemsConfiguration.Shapes.FirstOrDefault(y => y._image.sprite == x._image.sprite))).ToList();
            }

            List<ItemEntity> entities = new List<ItemEntity>();
            for (var i = 0; i < items.Count; i++)
            {
                entities.Add(new ItemEntity (itemsIds[i], _itemsHaveDifferentShapes ? shapes[i] : -1));
            }

            OrderUpdater orderUpdater = _currentOrders.FirstOrDefault(x => x.Order.Items.Intersect(entities).Count() == entities.Count);

            if (orderUpdater == null)
            {
                Destroy(box.gameObject);
                return;
            }

            foreach (var entity in entities)
            {
                ItemsCompleted itemsCompleted = orderUpdater.ItemsCompleted.FirstOrDefault(x => Equals(x.ItemEntity, entity));
                itemsCompleted.Completed = true;
            }

            if (orderUpdater.IsComplete)
            {
                Destroy(box.gameObject);
                OrderCompleted?.Invoke(new OrderCompleted(orderUpdater.Order, orderUpdater.Score));
                _currentOrders.Remove(orderUpdater);
            }
            else
            {
                OrderUpdated?.Invoke(orderUpdater);
            }
        }

        public void OrderExpired(string id)
        {
            OrderUpdater order = _currentOrders.FirstOrDefault(x => x.Order.Id == id);
            _currentOrders.Remove(order);

            OrderCancelled?.Invoke(order);
        }
    }
}