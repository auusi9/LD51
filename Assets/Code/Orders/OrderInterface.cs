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
        [SerializeField] private ScoreConfiguration _scoreConfiguration;
        private OrderGenerator _orderGenerator;

        private List<OrderUpdater> _currentOrders = new List<OrderUpdater>();

        public event Action<OrderCompleted> OrderCompleted;
        public event Action<OrderCompleted> OrderCancelled;
        public event Action<OrderUpdater> OrderUpdated;
        public event Action MainMenuBoxSent;
        public event Action MainMenuBoxInvalidSent;
        public event Action InvalidBox;
        public event Action<Order> NewOrderCreated;

        public int QueueLength => _currentOrders.Count;

        public GameStats GameStats;

        private void OnEnable()
        {
            _orderGenerator = new OrderGenerator(new OrderConfigurator()
            {
                ItemAmount = _itemsConfiguration.TypeAmount,
                ShapesAmount = _itemsConfiguration.ShapesAmount,
                MaximumItemsXOrder = _maximumItemsXOrder,
                MinimumItemsXOrder = _minimumItemsXOrder,
                ItemsHaveDifferentShapes = _itemsHaveDifferentShapes,
                OrderExpirationTime = _orderExpirationTime,
                ShapeChance = _itemsConfiguration.Chances
            });
        }

        public void Clear()
        {
            _currentOrders.Clear();
            GameStats.Clear();
        }

        public void MainMenuBox()
        {
            MainMenuBoxSent?.Invoke();
        }  
        
        public void MainMenuBoxInvalid()
        {
            MainMenuBoxInvalidSent?.Invoke();
        }

        public void NewOrder()
        {
            Order order = _orderGenerator.NewOrder();
            _currentOrders.Add(new OrderUpdater(order, 0, new List<ItemsCompleted>()));
            NewOrderCreated?.Invoke(order);
        }

        public void BoxSent(Box box)
        {
            List<Item> items = box.GetItems(out Box.BoxInfo boxInfo);
            
            Debug.Log("Box  item count " + items.Count + " and box is open " + box.IsOpen);

            if (items.Count == 0 || _currentOrders.Count == 0 || box.IsOpen)
            {
                Destroy(box.gameObject);
                InvalidBox?.Invoke();
                return;
            }

            List<int> itemsIds = items.Select(x => Array.IndexOf(_itemsConfiguration.Shapes,  _itemsConfiguration.Shapes.FirstOrDefault(y => y.Icon == x.Icon))).ToList();
            List<int> shapes = new List<int>();

            if (_itemsHaveDifferentShapes)
            {
                shapes = items.Select(x => Array.IndexOf(_itemsConfiguration.Shapes,  _itemsConfiguration.Shapes.FirstOrDefault(y => y.Icon == x.Icon))).ToList();
            }

            List<ItemEntity> entities = new List<ItemEntity>();
            for (var i = 0; i < items.Count; i++)
            {
                entities.Add(new ItemEntity (itemsIds[i], _itemsHaveDifferentShapes ? shapes[i] : -1));
            }

            List<OrderUpdater> ordersUpdater = new List<OrderUpdater>();
            List<ItemEntity> distinctEntities = entities.Distinct().ToList();

            foreach (var order in _currentOrders)
            {
                bool hasAllItems = true;
                foreach (var entity in distinctEntities)
                {
                    int count = order.ItemsCompleted.Count(x => !x.Completed && x.ItemEntity.Equals(entity));
                    if (count == 0 || count < entities.Count(x => x.Equals(entity)))
                    {
                        hasAllItems = false;
                        break;
                    }
                }

                if (hasAllItems)
                {
                    ordersUpdater.Add(order);
                }
            }

            OrderUpdater orderUpdater = GetBestOrder(ordersUpdater, entities.Count);
            
            if (orderUpdater == null)
            {
                Destroy(box.gameObject);
                Debug.Log("No order found for this items");
                InvalidBox?.Invoke();
                return;
            }

            foreach (var entity in entities)
            {
                ItemsCompleted itemsCompleted = orderUpdater.ItemsCompleted.FirstOrDefault(x => Equals(x.ItemEntity, entity) && !x.Completed);
                if (itemsCompleted != null)
                {
                    itemsCompleted.Completed = true;
                }
            }
            
            if (orderUpdater.UpdateScore(boxInfo, _scoreConfiguration))
            {
                GameStats.PerfectBoxesSent++;
            }
            
            GameStats.BoxesSent++;
            GameStats.ItemsSent += entities.Count;

            if (orderUpdater.IsComplete)
            {
                GameStats.OrdersCompleted++;
                OrderUpdated?.Invoke(orderUpdater);
                Destroy(box.gameObject);
                Debug.Log("Order completed");
                _currentOrders.Remove(orderUpdater);
                OrderCompleted?.Invoke(new OrderCompleted(orderUpdater.Order, orderUpdater.Score));
            }
            else
            {
                OrderUpdated?.Invoke(orderUpdater);
                Debug.Log("Order updated");
                Destroy(box.gameObject);
            }
        }

        private OrderUpdater GetBestOrder(List<OrderUpdater> ordersUpdater, int entitiesCount)
        {
            OrderUpdater orderUpdater =
                ordersUpdater.FirstOrDefault(x => x.ItemsCompleted.Count(y => !y.Completed) == entitiesCount);

            if (orderUpdater != null)
            {
                Debug.Log("Best order found");
                return orderUpdater;
            }

            if (ordersUpdater.Count > 0)
            {
                return ordersUpdater[0];
            }

            return null;
        }

        public void OrderExpired(string id)
        {
            OrderUpdater order = _currentOrders.FirstOrDefault(x => x.Order.Id == id);

            if (order == null)
            {
                return;
            }
            
            _currentOrders.Remove(order);
            GameStats.OrdersLost++;
            OrderCancelled?.Invoke(order);
        }
    }

    [Serializable]
    public class ScoreConfiguration
    {
        public int ScoreXEmptyTile;
        public int ScoreXItemTile;
        public int FilledUpBonus;
        public int PerfectBoxBonus;
        public int ItemBonus;
    }

    public struct GameStats
    {
        public int ItemsSent;
        public int BoxesSent;
        public int PerfectBoxesSent;
        public int OrdersCompleted;
        public int OrdersLost;

        public void Clear()
        {
            ItemsSent = 0;
            BoxesSent = 0;
            PerfectBoxesSent = 0;
            OrdersCompleted = 0;
            OrdersLost = 0;
        }
    }
}