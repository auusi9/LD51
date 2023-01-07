using Code.Boxes;
using Code.Services.Entities;

namespace Code.Orders
{
    public class OrderCompleted
    {
        public Order Order { get; }
        public int Score { get; set; }
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

            if (boxInfo.EmptyTiles == 0)
            {
                score += scoreConfiguration.FilledUpBonus;
            }
             
            if(boxInfo.EmptyTiles == 0 && (boxInfo.FillTiles / ((float)boxInfo.ItemTiles + boxInfo.FillTiles)) < 0.3f && Order.Items.Count == boxInfo.ItemCount)
            {
                score += scoreConfiguration.PerfectBoxBonus;
            }

            score += scoreConfiguration.ItemBonus * (boxInfo.ItemCount - 1);
            score += boxInfo.ItemTiles * scoreConfiguration.ScoreXItemTile;

            if (Boxes > 1)
            {
                score /= (Boxes-1 * 2);
            }

            Score += score;
        }
    }
}