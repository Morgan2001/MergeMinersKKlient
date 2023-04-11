using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace _Proxy.Config
{
    public class MinersConfigLoader : MonoBehaviour
    {
        [SerializeField] private TextAsset _minersConfig;

        public MinersData Process()
        {
            var json = _minersConfig.text;
            return JsonConvert.DeserializeObject<MinersData>(json);
        }
    }

    public class MinersData : Dictionary<int, MinerData>
    {
    }

    public class MinerData
    {
        public int Level { get; set; }
        public string Item { get; set; }
        public double Earning { get; set; }
        public double Price { get; set; }
        public float PriceMultiplier { get; set; }
        public int PriceInGems { get; set; }
        public double? Bonus { get; set; }
    }
}