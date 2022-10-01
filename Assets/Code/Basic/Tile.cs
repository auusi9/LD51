using System.Drawing;
using UnityEngine;

namespace Code.Basic
{
    public class Tile : MonoBehaviour
    {
        public RectTransform RectTransform => (RectTransform)transform;
        public Point Position;
    }
}