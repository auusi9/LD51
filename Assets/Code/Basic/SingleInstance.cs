using System;
using UnityEngine;

namespace Code.Basic
{
    public class SingleInstance<T> : MonoBehaviour 	where T : Component
    {
        private static T _instance;

        public static T Get() => _instance;
        
        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}