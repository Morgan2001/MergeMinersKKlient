using UnityEngine;
using Utils.MVVM;

namespace UI.Popups
{
    public class NewMinerPopupViewModel : ViewModel
    {
        public string Name { get; }
        public int Level { get; }
        public double Income { get; }
        public Sprite Icon { get; }
        public Sprite PreviousIcon { get; }

        public NewMinerPopupViewModel(string name, int level, double income, Sprite icon, Sprite previousIcon)
        {
            Name = name;
            Level = level;
            Income = income;
            Icon = icon;
            PreviousIcon = previousIcon;
        }
    }
}