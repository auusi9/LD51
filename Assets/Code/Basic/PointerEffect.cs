using System.Drawing;
using UnityEngine;

namespace Code.Basic
{
    public class PointerEffect : MonoBehaviour
    {
        [SerializeField] ParticleSystem _particleSystem;

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                ActivatePS();
            }
        }

        void ActivatePS()
        {
            Vector3 pos = Input.mousePosition;
            pos = Camera.main.ScreenToWorldPoint(pos);
            pos.z = 0f;
            _particleSystem.transform.position = pos;

            _particleSystem.Play();
        }
    }
}