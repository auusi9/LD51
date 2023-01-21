using System.Runtime.InteropServices;
using UnityEngine;

namespace Code.Basic
{
    public static class Link
    {
        public static void Open(string link)
        {
            Application.OpenURL(link);
        }
    }
}