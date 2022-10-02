using Code.Boxes;
using Code.Orders;
using UnityEngine;

namespace Code.Senders
{
    public class Sender : MonoBehaviour
    {
        [SerializeField] private OrderInterface _orderInterface;
        
        public void Send(MovingBox movingBox)
        {
            //CalculateScore
            _orderInterface.BoxSent(movingBox.Box);
        }
    }
}