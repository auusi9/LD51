using UnityEngine;

namespace Code.Boxes
{
    public class ItemMainMenu : MonoBehaviour
    {
        [SerializeField] private string _url;
        
        public string URL => _url;
    }
}