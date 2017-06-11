using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Network
{
    public class Networking : MonoBehaviour
    {
        public void SendRequest(IRequest request)
        {
            StartCoroutine(Request(request));
        }

        public void GetSprite(string url, Action<Sprite> resultCallback)
        {
            var textureRequest = new TextureRequest().SelectAddress(url)
                .AddSuccesCallback(resultCallback)
                .Build();
            SendRequest(textureRequest);
        }

        private IEnumerator Request(IRequest request)
        {
            yield return request.Send();
            request.StartCallback();
        }
    }
}