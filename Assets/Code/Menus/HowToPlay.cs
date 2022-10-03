using UnityEngine;

namespace Code.Menus
{
    public class HowToPlay : MonoBehaviour
    {
        [SerializeField] private GameObject[] _pages;

        private int _currentPage;

        private void Start()
        {
            SetPage();
        }

        public void NextPage()
        {
            _currentPage++;

            if (_currentPage > _pages.Length - 1)
            {
                _currentPage = 0;
            }
            SetPage();
        }

        public void PreviousPage()
        {
            _currentPage--;
            if (_currentPage < 0)
            {
                _currentPage = _pages.Length - 1;
            }
            SetPage();
        }

        private void SetPage()
        {
            for (int i = 0; i < _pages.Length; i++)
            {
                if (i == _currentPage)
                {
                    _pages[i].SetActive(true);
                }
                else
                {
                    _pages[i].SetActive(false);
                }
            }
        }
    }
}