using System.Linq;
using MergeMiner.Core.State.Config;
using MergeMiner.Core.State.Enums;
using UnityEngine;

namespace UI.Utils
{
    public interface IResourceHelper
    {
        Sprite GetNormalIconByLevel(int level);
        Sprite GetPoweredIconByLevel(int level);
        string GetLocationNameByLevel(int level);
        Sprite GetLocationImageByLevel(int level);
        Sprite GetBoxIconByType(MinerSource source);
        Sprite GetBonusIconByType(BonusType bonusType);
        FlyingBonusData GetBonusDataByType(BonusType bonusType);
    }
    
    public class ResourceHelper : IResourceHelper
    {
        private readonly SetOfMiningDeviceDatas _minersConfig;
        private readonly SetOfMiningDeviceBoxes _minerBoxesConfig;
        private readonly SetOfLocations _locationsConfig;
        private readonly SetOfFlyingBonuses _flyingBonusesConfig;

        public ResourceHelper(
            SetOfMiningDeviceDatas minersConfig,
            SetOfMiningDeviceBoxes minerBoxesConfig,
            SetOfLocations locationsConfig,
            SetOfFlyingBonuses flyingBonusesConfig)
        {
            _minersConfig = minersConfig;
            _minerBoxesConfig = minerBoxesConfig;
            _locationsConfig = locationsConfig;
            _flyingBonusesConfig = flyingBonusesConfig;
        }

        public Sprite GetNormalIconByLevel(int level)
        {
            return _minersConfig.MiningDeviceDatas.First(x => x.Level == level).Sprite;
        }
        
        public Sprite GetPoweredIconByLevel(int level)
        {
            return _minersConfig.MiningDeviceDatas.First(x => x.Level == level).SpriteWithOutline;
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
            if (source == MinerSource.RandomOpened) source = MinerSource.Random;
            return _minerBoxesConfig.MiningDeviceBoxSprites.First(x => x.Type == source).Sprite;
        }

        public Sprite GetBonusIconByType(BonusType bonusType)
        {
            return _flyingBonusesConfig.FlyingBonuses.First(x => x.Type == bonusType).Sprite;
        }

        public FlyingBonusData GetBonusDataByType(BonusType bonusType)
        {
            return _flyingBonusesConfig.FlyingBonuses.First(x => x.Type == bonusType);
        }
    }
}