using ModestTree;
using UnityEngine;
using UnityEngine.Networking;

using SimpleJson;

namespace Assets.Scripts.Network {
    public class PostRequest<TResult> : Request<TResult, PostRequest<TResult>> {
        protected override PostRequest<TResult> Instance {
            get { return this; }
        }

        public override PostRequest<TResult> Build() {
            if (method == null || method.IsEmpty()) {
                Debug.LogError("No method specified");
                return null;
            }
            if (bodyParameters == null) {
                bodyParameters = "";
            }

            request = new UnityWebRequest(string.Format("{0}{1}", NetworkConstants.SERVER_URL, MethodFormat()), "POST");
            byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(bodyParameters);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            if (NetworkConstants.API_TOKEN != null) {
                request.SetRequestHeader("Authorization", NetworkConstants.API_TOKEN);
            }

            return this;
        }
    }
}