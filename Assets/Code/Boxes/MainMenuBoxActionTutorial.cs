using UnityEngine;

namespace Code.Boxes
{
    public class MainMenuBoxActionTutorial : MainMenuBoxAction
    {
        [SerializeField] private GameObject _tutorial;
        
        public override void DoAction()
        {
            _tutorial.SetActive(true);
            GetComponent<MainMenuBox>().Destroy();
        }
    }
}