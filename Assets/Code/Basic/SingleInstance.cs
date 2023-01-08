using System;
using UnityEngine;

namespace Code.Basic
{
    public class SingleInstance : MonoBehaviour
    {
        private static SingleInstance _instance;
        
        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}