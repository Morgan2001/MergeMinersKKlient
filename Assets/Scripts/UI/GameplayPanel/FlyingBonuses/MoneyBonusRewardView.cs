using UnityEngine;
using UnityEngine.UI;
using Utils.MVVM;

namespace UI.GameplayPanel.FlyingBonuses
{
    public class MoneyBonusRewardView : View<MoneyBonusRewardViewModel>
    {
        [SerializeField] private Text _money;
        
        protected override void BindInner(MoneyBonusRewardViewModel vm)
        {
            _money.text = LargeNumberFormatter.FormatNumber(_vm.Money);
        }
    }

    public class MoneyBonusRewardViewModel : ViewModel
    {
        public double Money { get; }

        public MoneyBonusRewardViewModel(double money)
        {
            Money = money;
        }
    }
}