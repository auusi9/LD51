using System;
using UnityEngine;

namespace Code.Orders
{
    public class ScoreSystem : MonoBehaviour
    {
        [SerializeField] private OrderInterface _orderInterface;
        [SerializeField] private int _scoreTimeBonus = 100;
        [SerializeField] private int _scoreTime = 50;
        [SerializeField] private int _missedOrderPenalization = 100;

        private int _currentScore = 0;

        public Action<int> ScoreUpdated;

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
            _currentScore -= _missedOrderPenalization;
            
            ScoreUpdated?.Invoke(_currentScore);
        }

        private void OrderCompleted(OrderCompleted obj)
        {
            int score = obj.Score;
            float time = 1 - (Time.time - obj.Order.OrderCreatedTime) /
                (obj.Order.OrderExpirationTime - obj.Order.OrderCreatedTime);

            if (time > 0.4f)
            {
                score += _scoreTimeBonus;
            }
            else
            {
                score += _scoreTime;
            }

            _currentScore += score;
            ScoreUpdated?.Invoke(_currentScore);
        }

        private void OrderUpdated(OrderUpdater obj)
        {
            
        }
    }
}