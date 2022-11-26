using UnityEngine;

namespace Code.Burners
{
    public class Burner : MonoBehaviour
    {
        [SerializeField] private AudioSource _burnerAudioSource;
        [SerializeField] private GameObject _burnerParticles;

        public void DestroyThing()
        {
            _burnerAudioSource.Play();
            _burnerParticles.SetActive(false);
            _burnerParticles.SetActive(true);
        }
    }
}