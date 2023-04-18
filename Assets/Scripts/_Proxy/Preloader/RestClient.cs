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

namespace _Proxy.Preloader
{
    public class RestClient
    {
        private const string _baseUrl = "http://localhost:5208/";
        
        private readonly JsonSerializerSettings _settings;

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

        private async Task<T> SendRequest<T>(UnityWebRequest uwr)
        {
            try
            {
                var result = await uwr.SendWebRequest().ToUniTask();
                if (result.result != UnityWebRequest.Result.Success) return default;
                var json = result.downloadHandler.text;
                Debug.Log(json);
                return JsonConvert.DeserializeObject<T>(json, _settings);
            }
            catch (Exception)
            {
                return default;
            }
        }
    }
}