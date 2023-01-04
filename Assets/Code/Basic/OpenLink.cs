using System.Runtime.InteropServices;
using UnityEngine;

namespace Code.Basic
{
    public static class Link
    {
        public static void Open(string link)
        {
#if UNITY_EDITOR
                Application.OpenURL(link);
#else 
                OpenLinkJSPlugin(link);
#endif

        }
        public static void OpenLinkJSPlugin(string link) 
        {
#if !UNITY_EDITOR
		openWindow(link);
#endif
        }

        [DllImport("__Internal")]
        private static extern void openWindow(string url);
    }
}