

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace YSH.Framework
{
    public class NetWorkHttpMgr : Singleton<NetWorkHttpMgr>
    {
        public class HttpResponse
        {
            public bool HasError;
            public string ErrorMessage;
            public string Data;
        }

        public void Get(string url, Action<HttpResponse> callback)
        {
            MonoMgr.Instance.StartCoroutine(GetCoroutine(url, callback));
        }

        private IEnumerator GetCoroutine(string url, Action<HttpResponse> callback)
        {
            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                yield return request.SendWebRequest();
                callback?.Invoke(ProcessResponse(request));
            }
        }

        public void Post(string url, string jsonData, Action<HttpResponse> callback)
        {
            MonoMgr.Instance.StartCoroutine(PostCoroutine(url, jsonData, callback));
        }

        private IEnumerator PostCoroutine(string url, string jsonData, Action<HttpResponse> callback)
        {
            using (UnityWebRequest request = UnityWebRequest.Post(url, ""))
            {
                byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");

                yield return request.SendWebRequest();
                callback?.Invoke(ProcessResponse(request));
            }
        }

        private HttpResponse ProcessResponse(UnityWebRequest request)
        {
            HttpResponse response = new HttpResponse();
            if (request.result != UnityWebRequest.Result.Success)
            {
                response.HasError = true;
                response.ErrorMessage = request.error;
            }
            else
            {
                response.HasError = false;
                // ´¦Àí "null" ×Ö·û´®»ò¿Õ×Ö·û´®
                response.Data = string.IsNullOrEmpty(request.downloadHandler.text) || request.downloadHandler.text == "null"
                                ? null
                                : request.downloadHandler.text;
            }
            return response;
        }
    }
}
