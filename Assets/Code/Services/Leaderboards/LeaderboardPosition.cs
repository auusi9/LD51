namespace Code.Services.Leaderboards
{
    public class LeaderboardPosition : LeaderboardEntry
    {
        public int Position { get; set; }

        public LeaderboardPosition(int position, LeaderboardEntry entry)
        {
            Position = position;
            Date = entry.Date;
            Alias = entry.Alias;
            Score = entry.Score;
        }
    }
}