using Code.Menus;
using UnityEngine;

namespace Code.Boxes
{
    public class MainMenuBoxActionStart : MainMenuBoxAction
    {
        [SerializeField] private MainMenu _mainMenu;
        [SerializeField] private GameState _gameState;
        
        public override void DoAction()
        {
            _gameState.StartGame();
            _mainMenu.gameObject.SetActive(false);
        }
    }
}