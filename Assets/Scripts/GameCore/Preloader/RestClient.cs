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
using Utils.Reactive;

namespace GameCore.Preloader
{
    public class RestClient
    {
        private const string _baseUrl = "https://mergeminers.fun/";
        
        public static string BaseUrl => _baseUrl; 
        
        private readonly JsonSerializerSettings _settings;

        private ReactiveEvent<Action> _disconnected = new();
        public IReactiveSubscription<Action> Disconnected => _disconnected;

        public RestClient()
        {
            _settings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver()
            };
            _settings.Converters.Add(new GameEventConverter());
        }

        public async Task<T> Get<T>(string path)
        {
            Debug.Log(_baseUrl + path);
            return await SendRequest<T>(UnityWebRequest.Get(_baseUrl + path));
        }
        
        public async Task<T> Post<T>(string path, Dictionary<string, object> data = null)
        {
            var parameters = data != null
                ? new Dictionary<string, string>(data.Select(x => new KeyValuePair<string, string>(x.Key, x.Value.ToString())))
                : new Dictionary<string, string>();
            Debug.Log(_baseUrl + path);
            return await SendRequest<T>(UnityWebRequest.Post(_baseUrl + path, parameters));
        }
        
        public async Task<bool> Get(string path)
        {
            Debug.Log(_baseUrl + path);
            return await SendRequest(UnityWebRequest.Get(_baseUrl + path));
        }
        
        public async Task<bool> Post(string path, Dictionary<string, object> data = null)
        {
            var parameters = data != null
                ? new Dictionary<string, string>(data.Select(x => new KeyValuePair<string, string>(x.Key, x.Value.ToString())))
                : new Dictionary<string, string>();
            Debug.Log(_baseUrl + path);
            return await SendRequest(UnityWebRequest.Post(_baseUrl + path, parameters));
        }

        private async Task<T> SendRequest<T>(UnityWebRequest uwr)
        {
            try
            {
                uwr.timeout = 3;
                var result = await uwr.SendWebRequest().ToUniTask();
                if (result.result != UnityWebRequest.Result.Success)
                {
                    result.Dispose();
                    return default;
                }
                
                var json = result.downloadHandler.text;
                Debug.Log(json);
                result.Dispose();
                return JsonConvert.DeserializeObject<T>(json, _settings);
            }
            catch (Exception)
            {
                _disconnected.Trigger(async () => await SendRequest(uwr));
                return default;
            }
        }
        
        private async Task<bool> SendRequest(UnityWebRequest uwr)
        {
            try
            {
                uwr.timeout = 3;
                var result = await uwr.SendWebRequest().ToUniTask();
                if (result.result != UnityWebRequest.Result.Success)
                {
                    result.Dispose();
                    return default;
                }
                
                result.Dispose();
                return true;
            }
            catch (Exception)
            {
                _disconnected.Trigger(async () => await SendRequest(uwr));
                return default;
            }
        }
    }
}