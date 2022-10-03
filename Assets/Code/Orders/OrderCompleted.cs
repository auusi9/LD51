using Code.Boxes;
using Code.Services.Entities;

namespace Code.Orders
{
    public class OrderCompleted
    {
        public Order Order { get; }
        public int Score { get; private set; }
        public int Boxes { get; private set; }

        public OrderCompleted(Order order, int score)
        {
            Order = order;
            Score = score;
        }

        public void UpdateScore(Box.BoxInfo boxInfo, ScoreConfiguration scoreConfiguration)
        {
            Boxes++;
            int score = 0;

            score = boxInfo.EmptyTiles * -scoreConfiguration.ScoreXEmptyTile;

            if (boxInfo.FillTiles == 0 && boxInfo.EmptyTiles == 0)
            {
                score += scoreConfiguration.BonusNoFillNoEmpty;
            }
            else
            {
                score += boxInfo.FillTiles >= boxInfo.ItemTiles ? scoreConfiguration.BonusMoreFillThanItem : scoreConfiguration.BonusLessFillThanItem;
            }

            score += boxInfo.ItemTiles * (scoreConfiguration.ScoreXItemTile / Boxes);

            Score = score;
        }
    }
}