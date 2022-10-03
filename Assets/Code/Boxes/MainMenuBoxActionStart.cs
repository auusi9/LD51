using Code.Menus;
using UnityEngine;

namespace Code.Boxes
{
    public class MainMenuBoxActionStart : MainMenuBoxAction
    {
        [SerializeField] private MainMenu _mainMenu;
        
        public override void DoAction()
        {
            Time.timeScale = 1f;
            _mainMenu.gameObject.SetActive(false);
        }
    }
}