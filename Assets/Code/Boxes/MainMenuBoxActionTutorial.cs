using Code.Orders;
using UnityEngine;

namespace Code.Boxes
{
    public class MainMenuBoxActionTutorial : MainMenuBoxAction
    {
        [SerializeField] private GameObject _tutorial;
        [SerializeField] private OrderInterface _orderInterface;

        public override void DoAction()
        {
            _tutorial.SetActive(true);
            _orderInterface.MainMenuBox();
        }
    }
}