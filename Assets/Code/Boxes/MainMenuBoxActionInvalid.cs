using Code.Basic;
using Code.Menus;
using Code.Orders;
using UnityEngine;

namespace Code.Boxes
{
    public class MainMenuBoxActionInvalid : MainMenuBoxAction
    {
        [SerializeField] private OrderInterface _orderInterface;
        [SerializeField] private Box _box;

        public override void DoAction()
        {
            _orderInterface.MainMenuBoxInvalid();

            Box.BoxInfo boxInfo;
            var items = _box.GetItems(out boxInfo);

            foreach (var item in items)
            {
                var itemMainMenu = item.GetComponent<ItemMainMenu>();

                if (itemMainMenu != null)
                {
                    Link.Open(itemMainMenu.URL);
                }
            }
        }
    }
}