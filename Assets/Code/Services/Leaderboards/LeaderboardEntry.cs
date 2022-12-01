using System;

namespace Code.Services.Leaderboards
{
    [Serializable]
    public class LeaderboardEntry
    {
        public string Date { get; set; }
        public string Alias { get; set; }
        public int Score { get; set; }
    }
}