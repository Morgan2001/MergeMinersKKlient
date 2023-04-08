using System.Collections.Generic;
using UnityEngine;

namespace _Proxy.Config
{
    [CreateAssetMenu(fileName = "MinerShopConfig", menuName = "ScriptableObjects/MinerShopConfig", order = 1)]
    public class MinerShopConfig : ScriptableObject
    {
        public List<MiningDeviceOffer> ForMoney;
        public List<MiningDeviceOffer> ForAds;
    }
}