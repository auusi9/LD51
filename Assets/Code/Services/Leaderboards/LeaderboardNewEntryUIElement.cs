using UnityEngine;

namespace Code.Services.Leaderboards
{
    public class LeaderboardNewEntryUIElement : LeaderboardUIElement
    {
        [SerializeField] private GameObject _animation;
        
        public void Disable()
        {
            _animation.gameObject.SetActive(false);
        }
    }
}