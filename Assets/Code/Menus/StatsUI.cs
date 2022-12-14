using System;
using Code.Orders;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code.Menus
{
    public class StatsUI : MonoBehaviour
    {
        [SerializeField] private OrderInterface _orderInterface;
        [SerializeField] private TextMeshProUGUI _items;
        [SerializeField] private TextMeshProUGUI _boxesSent;
        [SerializeField] private TextMeshProUGUI _perfectBoxes;
        [SerializeField] private TextMeshProUGUI _ordersCompleted;
        [SerializeField] private TextMeshProUGUI _ordersLost;

        private void OnEnable()
        {
            _items.text = _orderInterface.GameStats.ItemsSent.ToString();
            _boxesSent.text = _orderInterface.GameStats.BoxesSent.ToString();
            _perfectBoxes.text = _orderInterface.GameStats.PerfectBoxesSent.ToString();
            _ordersCompleted.text = _orderInterface.GameStats.OrdersCompleted.ToString();
            _ordersLost.text = _orderInterface.GameStats.OrdersLost.ToString();
        }
    }
}