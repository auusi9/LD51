using Code.Menus;
using Code.Orders;
using UnityEngine;

namespace Code.Boxes
{
    public class MainMenuBoxActionStart : MainMenuBoxAction
    {
        [SerializeField] private MainMenu _mainMenu;
        [SerializeField] private GameState _gameState;
        [SerializeField] private OrderInterface _orderInterface;
        
        public override void DoAction()
        {
            _gameState.StartGame();
            _mainMenu.gameObject.SetActive(false);
            _orderInterface.MainMenuBox();
        }
    }
}