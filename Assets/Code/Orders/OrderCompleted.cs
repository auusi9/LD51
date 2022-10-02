using Code.Services.Entities;

namespace Code.Orders
{
    public class OrderCompleted
    {
        public Order Order { get; }
        public int Score { get; private set; }

        public OrderCompleted(Order order, int score)
        {
            Order = order;
            Score = score;
        }
    }
}