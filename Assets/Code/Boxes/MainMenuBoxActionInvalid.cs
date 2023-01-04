using Code.Menus;
using Code.Orders;
using UnityEngine;

namespace Code.Boxes
{
    public class MainMenuBoxActionInvalid : MainMenuBoxAction
    {
        [SerializeField] private OrderInterface _orderInterface;

        public override void DoAction()
        {
            _orderInterface.MainMenuBoxInvalid();
        }
    }
}