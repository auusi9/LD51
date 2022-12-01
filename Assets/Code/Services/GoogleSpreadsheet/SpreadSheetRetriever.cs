using System;
using System.Collections;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace Code.Services.GoogleSpreadsheet
{
    public class SpreadSheetRetriever<T> where T : class
    {
        private const string _webServiceUrl = "https://script.google.com/macros/s/AKfycbyEmZSmJHfv_sCVmZcWu7Ji5tcVBwj7uvP0kSv1uUj7wxIChzGJiVfRLMlH3CIQEVFR/exec";
        private const int _maxWaitTime = 10;

        public event Action<T> ResponseSuccessful;
        public event Action ResponseFailed;
        private MonoBehaviour _monoBehaviour;

        public SpreadSheetRetriever(MonoBehaviour monoBehaviour)
        {
            _monoBehaviour = monoBehaviour;
        }

        public void GetData(string spreadSheetId, string workSheetName, string password)
        {
            _monoBehaviour.StartCoroutine(SendRequest(spreadSheetId, workSheetName, password));
        }

        public void SetData(string spreadSheetId, string worksheetName, string[] parameters, string password)
        {
            _monoBehaviour.StartCoroutine(SendRequest(spreadSheetId, worksheetName, password, parameters));
        }

        private IEnumerator SendRequest(string spreadSheetId, string workSheetName, string password, string[] parameters = null)
        {
            StringBuilder stringBuilder = new StringBuilder(_webServiceUrl + "?ssid=" + spreadSheetId + "&sheet=" +
                                                            workSheetName + "&pass=" + password);
            if (parameters != null && parameters.Length > 0)
            {
                for (int i = 0; i < parameters.Length; i++)
                {
                    stringBuilder.Append($"&val{i + 1}={parameters[i]}");
                }
                stringBuilder.Append("&action=SetData");
            }
            else
            {
                stringBuilder.Append("&action=GetData");
            }

            UnityWebRequest webRequest = UnityWebRequest.Get(stringBuilder.ToString());
            webRequest.timeout = _maxWaitTime;
            
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                T result = JsonConvert.DeserializeObject<T>(webRequest.downloadHandler.text);
                ResponseSuccessful?.Invoke(result);
            }
            else
            {
                ResponseFailed?.Invoke();
            }
        }
    }
}