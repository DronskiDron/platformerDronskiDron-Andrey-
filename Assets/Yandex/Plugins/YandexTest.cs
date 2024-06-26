using System;
using System.Collections;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Yandex.Plagins
{
    public class YandexTest : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _nameText;
        [SerializeField] RawImage _photo;


        [DllImport("__Internal")]
        private static extern void Hello();

        [DllImport("__Internal")]
        private static extern void GetYandexPlayerData();


        public void HelloButton()
        {
            GetYandexPlayerData();
        }


        public void SetName(string name)
        {
            _nameText.text = name;
        }


        public void SetPhoto(string url)
        {
            StartCoroutine(DownloadImage(url));
        }


        private IEnumerator DownloadImage(string mediaUrl)
        {
            var request = UnityWebRequestTexture.GetTexture(mediaUrl);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
                Debug.Log(request.error);
            else
                _photo.texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
        }
    }
}