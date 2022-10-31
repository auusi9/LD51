using System;
using Code.Menus;
using UnityEngine;

namespace Code.Orders
{
    public class OrderRequester : MonoBehaviour
    {
        [SerializeField] private float _timeBetweenOrders = 10f;
        [SerializeField] private OrderInterface _orderInterface;
        [SerializeField] private GameState _gameState;

        private float _lastOrder = 0f;

        private void Update()
        {
            if (!_gameState.GameStarted)
            {
                return;
            }
            
            if (_timeBetweenOrders < _lastOrder)
            {
                _lastOrder = 0f;
                _orderInterface.NewOrder();
                return;
            }

            _lastOrder += Time.deltaTime;
        }
    }
}