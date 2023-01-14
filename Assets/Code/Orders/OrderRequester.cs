using System;
using Code.Menus;
using UnityEngine;

namespace Code.Orders
{
    public class OrderRequester : MonoBehaviour
    {
        [SerializeField] private float _minTimeBetweenOrders = 5f;
        [SerializeField] private float _timeBetweenOrders = 10f;
        [SerializeField] private float _maxTimeBetweenOrders = 20f;
        [SerializeField] private int _slowDownOrders = 6;
        [SerializeField] private int _minimumOrdersToGoToRush = 3;
        [SerializeField] private OrderInterface _orderInterface;
        [SerializeField] private GameState _gameState;

        private float _lastOrder = 0f;

        [SerializeField] private float _currentTimeBetweenOrders = 10f;
        private RushState _currentState = RushState.Normal;

        private enum RushState
        {
            Calm,
            Normal,
            Rush
        }

        private void Start()
        {
            _lastOrder = _timeBetweenOrders - 3;
            _orderInterface.OrderCompleted += OrderCompleted;
            _orderInterface.OrderCancelled += OrderCompleted;
            _currentTimeBetweenOrders = _timeBetweenOrders;
            _orderInterface.Clear();
        }

        private void OnDestroy()
        {
            _orderInterface.OrderCompleted -= OrderCompleted;
            _orderInterface.OrderCancelled -= OrderCompleted;
        }

        private void OrderCompleted(OrderCompleted obj)
        {
            CheckCurrentState();
        }

        private void CheckCurrentState()
        {
            switch (_currentState)
            {
                case RushState.Calm:
                    if (_orderInterface.QueueLength < 2)
                    {
                        _currentState = RushState.Normal;
                        _currentTimeBetweenOrders = _timeBetweenOrders;
                    }
                    break;
                case RushState.Normal:
                    if (_orderInterface.QueueLength == 0)
                    {
                        if (_orderInterface.GameStats.OrdersCreated >= _minimumOrdersToGoToRush)
                        {
                            _currentState = RushState.Rush;
                            _currentTimeBetweenOrders = _minTimeBetweenOrders;
                        }
                        else
                        {
                            if (_lastOrder + 1 < _currentTimeBetweenOrders)
                            {
                                _lastOrder = _currentTimeBetweenOrders - 1;
                            }
                        }
                    }
                    else if (_orderInterface.QueueLength > _slowDownOrders)
                    {
                        _currentState = RushState.Calm;
                        _currentTimeBetweenOrders = _maxTimeBetweenOrders;
                    }
                    break;
                case RushState.Rush:
                    if (_orderInterface.QueueLength > (_slowDownOrders - 1))
                    {
                        _currentState = RushState.Normal;
                        _currentTimeBetweenOrders = _timeBetweenOrders;
                    }
                    else if(_orderInterface.QueueLength == 0)
                    {
                        _orderInterface.NewOrder();
                    }
                    break;
            }
        }

        private void Update()
        {
            if (!_gameState.GameStarted)
            {
                return;
            }
            
            if (_currentTimeBetweenOrders < _lastOrder)
            {
                _lastOrder = 0f;

                if (_orderInterface.QueueLength < 6)
                {
                    _orderInterface.NewOrder();
                }
            }

            _lastOrder += Time.deltaTime;
        }
    }
}