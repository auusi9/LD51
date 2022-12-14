using TMPro;
using UnityEngine;

namespace Code.Services.Leaderboards
{
    public class LeaderboardUIElement : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _position;
        [SerializeField] private TextMeshProUGUI _alias;
        [SerializeField] private TextMeshProUGUI _score;

        public void SetElement(int position, string alias, int score)
        {
            _position.text = $"{position}.";
            _alias.text = alias;
            _score.text = score.ToString("N0");
        }
    }
}