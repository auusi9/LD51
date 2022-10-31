using System;
using UnityEngine;

namespace Code.Menus
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private GameState _gameState;
        [SerializeField] private GameObject[] _enableOnGameStart;

        private void Start()
        {
            _gameState.GameStart += EnableObjects;
        }

        private void EnableObjects()
        {
            foreach (var go in _enableOnGameStart)
            {
                go.SetActive(true);
            }
        }

        private void OnDestroy()
        {
            _gameState.GameStart -= EnableObjects;
        }
    }
}