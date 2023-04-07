using System.Linq;
using MergeMiner.Core.State.Enums;
using UnityEngine;

namespace UI.Utils
{
    public interface IMinerResourceHelper
    {
        Sprite GetNormalIconByName(string name);
        Sprite GetPoweredIconByName(string name);
        Sprite GetBoxIconByType(MinerSource source);
    }
    
    public class ResourceHelper : IMinerResourceHelper
    {
        private readonly SetOfMiningDeviceDatas _minersConfig;
        private readonly SetOfMiningDeviceBoxes _minerBoxesConfig;

        public ResourceHelper(
            SetOfMiningDeviceDatas minersConfig,
            SetOfMiningDeviceBoxes minerBoxesConfig)
        {
            _minersConfig = minersConfig;
            _minerBoxesConfig = minerBoxesConfig;
        }

        public Sprite GetNormalIconByName(string name)
        {
            return _minersConfig.MiningDeviceDatas.First(x => x.Name == name).Sprite;
        }
        
        public Sprite GetPoweredIconByName(string name)
        {
            return _minersConfig.MiningDeviceDatas.First(x => x.Name == name).SpriteWithOutline;
        }

        public Sprite GetBoxIconByType(MinerSource source)
        {
            if (source == MinerSource.None) return null;
            return _minerBoxesConfig.MiningDeviceBoxSprites.First(x => x.Type == source).Sprite;
        }
    }
}