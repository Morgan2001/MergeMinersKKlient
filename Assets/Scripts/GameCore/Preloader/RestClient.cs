using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using MergeMiner.Core.Network.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using UnityEngine;
using UnityEngine.Networking;

namespace GameCore.Preloader
{
    public class RestClient
    {
        private const string _baseUrl = "https://mergeminers.fun/";
        
        public static string BaseUrl => _baseUrl; 
        
        private readonly JsonSerializerSettings _settings;

        public RestClient()
        {
            _settings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver()
            };
            _settings.Converters.Add(new GameEventConverter());
        }

        public async Task<RestResponse<T>> Get<T>(string path)
        {
            Debug.Log(_baseUrl + path);
            return await SendRequest<T>(UnityWebRequest.Get(_baseUrl + path));
        }
        
        public async Task<RestResponse<T>> Post<T>(string path, Dictionary<string, object> data = null)
        {
            var parameters = data != null
                ? new Dictionary<string, string>(data.Select(x => new KeyValuePair<string, string>(x.Key, x.Value.ToString())))
                : new Dictionary<string, string>();
            Debug.Log(_baseUrl + path);
            return await SendRequest<T>(UnityWebRequest.Post(_baseUrl + path, parameters));
        }
        
        public async Task<RestResponse> Get(string path)
        {
            Debug.Log(_baseUrl + path);
            return await SendRequest(UnityWebRequest.Get(_baseUrl + path));
        }
        
        public async Task<RestResponse> Post(string path, Dictionary<string, object> data = null)
        {
            var parameters = data != null
                ? new Dictionary<string, string>(data.Select(x => new KeyValuePair<string, string>(x.Key, x.Value.ToString())))
                : new Dictionary<string, string>();
            Debug.Log(_baseUrl + path);
            return await SendRequest(UnityWebRequest.Post(_baseUrl + path, parameters));
        }

        private async Task<RestResponse<T>> SendRequest<T>(UnityWebRequest uwr)
        {
            try
            {
                uwr.timeout = 3;
                var result = await uwr.SendWebRequest().ToUniTask();
                if (result.result != UnityWebRequest.Result.Success)
                {
                    result.Dispose();
                    return new RestResponse<T>(RestResultType.Error);
                }
                
                var json = result.downloadHandler.text;
                Debug.Log(json);
                result.Dispose();
                return new RestResponse<T>(RestResultType.Success, JsonConvert.DeserializeObject<T>(json, _settings));
            }
            catch (Exception)
            {
                return new RestResponse<T>(RestResultType.Disconnected);
            }
        }
        
        private async Task<RestResponse> SendRequest(UnityWebRequest uwr)
        {
            try
            {
                uwr.timeout = 3;
                var result = await uwr.SendWebRequest().ToUniTask();
                if (result.result != UnityWebRequest.Result.Success)
                {
                    result.Dispose();
                    return new RestResponse(RestResultType.Error);
                }
                
                result.Dispose();
                return new RestResponse(RestResultType.Success);
            }
            catch (Exception)
            {
                return new RestResponse(RestResultType.Disconnected);
            }
        }
    }
    
    public class RestResponse
    {
        public RestResultType ResultType { get; }

        public RestResponse(RestResultType resultType)
        {
            ResultType = resultType;
        }
    }

    public class RestResponse<T> : RestResponse
    {
        public T Result { get; }
        
        public RestResponse(RestResultType resultType, T result) : base(resultType)
        {
            Result = result;
        }
        
        public RestResponse(RestResultType resultType) : base(resultType)
        {
        }
    }

    public enum RestResultType
    {
        Success,
        Error,
        Disconnected,
    }
}