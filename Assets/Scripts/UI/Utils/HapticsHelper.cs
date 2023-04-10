using MoreMountains.NiceVibrations;

namespace UI.Utils
{
    public static class HapticsHelper
    {
        public static void MergeHaptics(bool maxLevelIncreased)
        {
            MMVibrationManager.Haptic(maxLevelIncreased ? HapticTypes.HeavyImpact : HapticTypes.MediumImpact);
        }
    }
}