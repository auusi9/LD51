using System;
using UnityEngine;

namespace Code.Orders
{
    public class ScoreSystem : MonoBehaviour
    {
        [SerializeField] private OrderInterface _orderInterface;

        private void Start()
        {
            _orderInterface.OrderCompleted += OrderCompleted;
            _orderInterface.OrderUpdated += OrderUpdated;
            _orderInterface.OrderCancelled += OrderCancelled;
        }
        
        private void OnDestroy()
        {
            _orderInterface.OrderCompleted -= OrderCompleted;
            _orderInterface.OrderUpdated -= OrderUpdated;
            _orderInterface.OrderCancelled -= OrderCancelled;
        }

        private void OrderCancelled(OrderCompleted obj)
        {
            
        }

        private void OrderCompleted(OrderCompleted obj)
        {
            
        }

        private void OrderUpdated(OrderUpdater obj)
        {
            
        }
    }
}