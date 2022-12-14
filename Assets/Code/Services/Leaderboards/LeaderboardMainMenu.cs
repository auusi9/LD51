namespace Code.Services.Leaderboards
{
    public class LeaderboardMainMenu : LeaderBoardUI
    {
        protected override LeaderboardPosition[] GetEntries()
        {
            return _service.Top;
        }
    }
}