using System.Linq;
using MergeMiner.Core.State.Data;
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
        Sprite GetBonusIconByType(string id);
        FlyingBonusData GetBonusDataById(string id);
        Sprite GetWheelRewardIcon(int reward);
        string GetWheelRewardDescription(int reward);
        Sprite GetUpgradeIcon(int index);
        string GetUpgradeDescription(int index);
    }
    
    public class ResourceHelper : IResourceHelper
    {
        private readonly SetOfMiningDeviceDatas _minersConfig;
        private readonly SetOfMiningDeviceBoxes _minerBoxesConfig;
        private readonly SetOfLocations _locationsConfig;
        private readonly SetOfFlyingBonuses _flyingBonusesConfig;
        private readonly SetOfWheelRewards _wheelRewards;
        private readonly SetOfUpgrades _upgrades;

        public ResourceHelper(
            SetOfMiningDeviceDatas minersConfig,
            SetOfMiningDeviceBoxes minerBoxesConfig,
            SetOfLocations locationsConfig,
            SetOfFlyingBonuses flyingBonusesConfig,
            SetOfWheelRewards wheelRewards,
            SetOfUpgrades upgrades)
        {
            _minersConfig = minersConfig;
            _minerBoxesConfig = minerBoxesConfig;
            _locationsConfig = locationsConfig;
            _flyingBonusesConfig = flyingBonusesConfig;
            _wheelRewards = wheelRewards;
            _upgrades = upgrades;
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

        public Sprite GetBonusIconByType(string id)
        {
            return _flyingBonusesConfig[id].Sprite;
        }

        public FlyingBonusData GetBonusDataById(string id)
        {
            return _flyingBonusesConfig[id];
        }

        public Sprite GetWheelRewardIcon(int reward)
        {
            return _wheelRewards.Rewards.Find(x => x.Id == reward).Sprite;
        }

        public string GetWheelRewardDescription(int reward)
        {
            return _wheelRewards.Rewards.Find(x => x.Id == reward).Description;
        }

        public Sprite GetUpgradeIcon(int index)
        {
            return _upgrades.Upgrades[index].Sprite;
        }

        public string GetUpgradeDescription(int index)
        {
            return _upgrades.Upgrades[index].Description;
        }
    }
}