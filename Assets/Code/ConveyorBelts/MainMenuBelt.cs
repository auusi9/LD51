using System;
using Code.Basic;
using Code.Boxes;
using Code.Menus;
using UnityEngine;

namespace Code.ConveyorBelts
{
    public class MainMenuBelt : MonoBehaviour
    {
        [SerializeField] private GameState _gameState;
        [SerializeField] private MainMenuBox[] _startObjects;
        [SerializeField] private Belt _belt;
        [SerializeField] private Vector3 _offset;
        
        private void Start()
        {
            _gameState.GameStart += OnGameStart;

            for (var i = 0; i < _startObjects.Length; i++)
            {
                var obj = _startObjects[i];
                obj.transform.position = _belt.GetInitPosition().position + (_offset * i);
                obj.SetBelt(_belt);
            }
        }

        private void OnDestroy()
        {
            _gameState.GameStart -= OnGameStart;
        }

        private void OnGameStart()
        {
            foreach (var obj in _startObjects)
            {
                _belt.RemoveObjectFromBelt(obj);
            }
        }
    }
}