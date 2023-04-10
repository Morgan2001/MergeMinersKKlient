using System.Linq;
using MergeMiner.Core.State.Enums;
using UnityEngine;

namespace UI.Utils
{
    public interface IMinerResourceHelper
    {
        Sprite GetNormalIconByName(string name);
        Sprite GetPoweredIconByName(string name);
        string GetLocationNameByLevel(int level);
        Sprite GetLocationImageByLevel(int level);
        Sprite GetBoxIconByType(MinerSource source);
    }
    
    public class ResourceHelper : IMinerResourceHelper
    {
        private readonly SetOfMiningDeviceDatas _minersConfig;
        private readonly SetOfMiningDeviceBoxes _minerBoxesConfig;
        private readonly SetOfLocations _locationsConfig;

        public ResourceHelper(
            SetOfMiningDeviceDatas minersConfig,
            SetOfMiningDeviceBoxes minerBoxesConfig,
            SetOfLocations locationsConfig)
        {
            _minersConfig = minersConfig;
            _minerBoxesConfig = minerBoxesConfig;
            _locationsConfig = locationsConfig;
        }

        public Sprite GetNormalIconByName(string name)
        {
            return _minersConfig.MiningDeviceDatas.First(x => x.Name == name).Sprite;
        }
        
        public Sprite GetPoweredIconByName(string name)
        {
            return _minersConfig.MiningDeviceDatas.First(x => x.Name == name).SpriteWithOutline;
        }

        public string GetLocationNameByLevel(int level)
        {
            return _locationsConfig.LocationDatas.Find(location => location.Level == level).Name;
        }

        public Sprite GetLocationImageByLevel(int level)
        {
            return _locationsConfig.LocationDatas.Find(location => location.Level == level).Sprite;
        }

        public Sprite GetBoxIconByType(MinerSource source)
        {
            if (source == MinerSource.None) return null;
            return _minerBoxesConfig.MiningDeviceBoxSprites.First(x => x.Type == source).Sprite;
        }
    }
}