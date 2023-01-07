using System;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Orders
{
    public class ScoreSystem : MonoBehaviour
    {
        [SerializeField] private OrderInterface _orderInterface;
        [SerializeField] private int _scoreTimeBonus = 100;
        [SerializeField] private int _scoreTimeNormal = 50;
        [SerializeField] private int _scoreTime = 50;
        [SerializeField] private int _missedOrderPenalization = 100;
        [SerializeField] private Animator _scoreAnimator;
        private int _completedTrigger = Animator.StringToHash("Completed");
        private int _cancelledTrigger = Animator.StringToHash("Cancelled");

        private int _currentScore = 0;

        public int TotalScore => _currentScore;

        public event Action<int> ScoreUpdated;
        public event Action<int> LastBoxCompletedScore;

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
            _scoreAnimator.SetTrigger(_cancelledTrigger);

            ScoreUpdated?.Invoke(_currentScore);
        }

        private void OrderCompleted(OrderCompleted obj)
        {
            int score = obj.Score;
            float time = 1 - (Time.time - obj.Order.OrderCreatedTime) /
                (obj.Order.OrderExpirationTime - obj.Order.OrderCreatedTime);
            
            score += GetTimeBonus(time, obj.Boxes);

            obj.Score = score;
            
            _currentScore += score;
            _scoreAnimator.SetTrigger(_completedTrigger);

            ScoreUpdated?.Invoke(_currentScore);
            LastBoxCompletedScore?.Invoke(score);
        }

        private int GetTimeBonus(float time, int boxes)
        {
            int score = 0;
            if (time > 0.6f)
            {
                score += _scoreTimeBonus;
            }
            else if (time > 0.3f)
            {
                score += _scoreTimeNormal;
            }
            else
            {
                score += _scoreTime;
            }

            if (boxes > 1)
            {
                score /= (boxes-1 * 2);
            }
            
            return score;
        }

        private void OrderUpdated(OrderUpdater obj)
        {
            
        }
    }
}