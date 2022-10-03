using UnityEngine;

namespace Code.Burners
{
    public class Burner : MonoBehaviour
    {
        [SerializeField] private AudioSource _burnerAudioSource;

        public void DestroyThing()
        {
            _burnerAudioSource.Play();
        }
    }
}