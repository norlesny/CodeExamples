using System;
using System.Collections.Generic;
using Assets.Scripts.Network.Models;
using ModestTree;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.Network {
    public interface IRequest {
        AsyncOperation Send();
        void StartCallback();
    }

    public abstract class Request<TResult, TBuilder> : IRequest
        where TBuilder : Request<TResult, TBuilder> {
        protected abstract TBuilder Instance { get; }
        protected UnityWebRequest request;
        protected string method;

        protected Action<TResult> successCallback;
        protected Action<Error> errorCallback;

        protected string bodyParameters;
        protected Dictionary<string, string> pathParameters;

        public abstract TBuilder Build();

        public TBuilder SelectMethod(string method) {
            this.method = method;
            return Instance;
        }

        public TBuilder AddSuccesCallback(Action<TResult> callback) {
            this.successCallback = callback;
            return Instance;
        }

        public TBuilder AddErrorCallback(Action<Error> callback) {
            this.errorCallback = callback;
            return Instance;
        }

        public AsyncOperation Send() {
            return request.Send();
        }

        public virtual void StartCallback() {
            //Debug.Log("Response (" + request.responseCode + "):\n" + request.downloadHandler.text);
            if (request.isError) {
                if (errorCallback == null) return;
                errorCallback(new Error {code = request.responseCode, message = request.error});
            }
            else if (request.responseCode != 200) {
                if (errorCallback == null) return;
                errorCallback(new Error {code = request.responseCode, message = request.downloadHandler.text});
            }
            else {
                if (successCallback == null) return;
                TResult result;
                if (request.downloadHandler.text[0] == '[') {
                    string newJson = "{ \"array\": " + request.downloadHandler.text + "}";
                    result = JsonUtility.FromJson<TResult>(newJson);
                }
                else {
                    result = JsonUtility.FromJson<TResult>(request.downloadHandler.text);
                }
                successCallback(result);
            }
        }

        public TBuilder AddPathParameter(string name, string value) {
            if (pathParameters == null) {
                pathParameters = new Dictionary<string, string>();
            }
            pathParameters.Add(name, value);
            return Instance;
        }

        protected string MethodFormat() {
            if (pathParameters == null) {
                pathParameters = new Dictionary<string, string>();
            }
            var methodUrl = method;

            foreach (var pair in pathParameters) {
                methodUrl = methodUrl.Replace(string.Format("{{{0}}}", pair.Key), pair.Value);
            }

            if (methodUrl.Contains("{")) {
                Debug.LogErrorFormat("Not all parameters passed for method ({0})", methodUrl);
            }

            return methodUrl;
        }

        public TBuilder AddBodyParameters(object parameters) {
            bodyParameters = JsonUtility.ToJson(parameters);
            if (bodyParameters.StartsWith("{\"array\":")) {
                bodyParameters = bodyParameters.Substring(9, bodyParameters.Length - 10);
            }

            return Instance;
        }
    }
}