using UnityEngine;

namespace Code.Services.Leaderboards
{
    public class LeaderboardEndPopup : MonoBehaviour
    {
        [SerializeField] private LeaderboardNewEntryUIElement _newEntry;
        [SerializeField] private LeaderboardUIElement[] _uiElements;
        [SerializeField] private LeaderboardService _service;

        private int _score = 0;

        private LeaderboardPosition[] GetEntries()
        {
            return _service.GetPossibleSurroundings(_score);
        }

        public void SetScore(int score)
        {
            _score = score;
            LeaderboardPosition[] entries = GetEntries();
            
            if(entries == null)
                return;
            int fakeEntry = 0;
            for (int i = 0; i < entries.Length; i++)
            {
                if (entries[i].IsFake)
                {
                    _newEntry.SetElement(entries[i].Position, entries[i].Alias, entries[i].Score);
                    _newEntry.transform.SetSiblingIndex(i);
                    fakeEntry = -1;
                }
                else
                {
                    _uiElements[i + fakeEntry].SetElement(entries[i].Position, entries[i].Alias, entries[i].Score);
                    _uiElements[i + fakeEntry].transform.SetSiblingIndex(i);
                }
            }
        }
    }
}