using Code.Basic;
using Code.Menus;
using Code.Orders;
using UnityEngine;

namespace Code.Boxes
{
    public class MainMenuBoxActionInvalid : MainMenuBoxAction
    {
        [SerializeField] private string[] _url;
        [SerializeField] private OrderInterface _orderInterface;

        public override void DoAction()
        {
            _orderInterface.MainMenuBoxInvalid();

            foreach (var url in _url)
            {
                Link.Open(url);
            }
        }
    }
}