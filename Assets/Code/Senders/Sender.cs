using System;
using Code.Boxes;
using Code.Orders;
using TMPro;
using UnityEngine;

namespace Code.Senders
{
    public class Sender : MonoBehaviour
    {
        [SerializeField] private OrderInterface _orderInterface;
        [SerializeField] private Animator _orderCompleted;
        [SerializeField] private TextMeshProUGUI _orderCompletedText;
        [SerializeField] private Animator _orderUpdated;
        [SerializeField] private ScoreSystem _scoreSystem;

        private void Start()
        {
            _orderInterface.OrderUpdated += OrderUpdated;
            _scoreSystem.LastBoxCompletedScore += OrderCompleted;
        }

        private void OnDestroy()
        {
            _orderInterface.OrderUpdated -= OrderUpdated;
            _scoreSystem.LastBoxCompletedScore -= OrderCompleted;
        }

        private void OrderCompleted(int score)
        {
            _orderCompletedText.text = score.ToString();
            if (_orderCompleted.gameObject.activeSelf)
            {
                _orderCompleted.Rebind();
                return;
            }
            
            _orderCompleted.gameObject.SetActive(true);
        }

        private void OrderUpdated(OrderUpdater order)
        {
            if (_orderUpdated.gameObject.activeSelf)
            {
                _orderUpdated.Rebind();
                return;
            }
            
            _orderUpdated.gameObject.SetActive(true);
        }

        public void Send(MovingBox movingBox)
        {
            //CalculateScore
            _orderInterface.BoxSent(movingBox.Box);
        }
    }
}