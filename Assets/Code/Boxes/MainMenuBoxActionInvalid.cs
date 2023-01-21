using System.Collections;
using System.Collections.Generic;
using Code.Basic;
using Code.Items;
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

            StartCoroutine(OpenLinks(items));
        }

        private IEnumerator OpenLinks(List<Item> items)
        {
            foreach (var item in items)
            {
                var itemMainMenu = item.GetComponent<ItemMainMenu>();

                if (itemMainMenu != null)
                {
                    Link.Open(itemMainMenu.URL);
                    yield return 0;
                }
            }
        }
    }
}