using UnityEngine;
using UnityEngine.UI;

namespace UI.Utils
{
    public class MinerAnimationSetup : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        public Image Icon => _icon;

        [SerializeField] private RectTransform _anchor;
        public RectTransform Anchor => _anchor;

        [SerializeField] private float _rotationStart;
        public float RotationStart => _rotationStart;

        [SerializeField] private float _rotationEnd;
        public float RotationEnd => _rotationEnd;
    }
}