using UnityEngine;

public class RelativePositionsCalculator : MonoBehaviour
{
    [SerializeField] private RectTransform _empty;

    public Vector2 CalcRelativePositions(RectTransform what, RectTransform relativeTo)
    {
        _empty.SetParent(what);
        _empty.anchoredPosition = Vector2.zero;

        _empty.SetParent(relativeTo);
        var pos = _empty.anchoredPosition;

        _empty.SetParent(transform);
        _empty.anchoredPosition = Vector2.zero;

        return pos;
    }
    
    public Vector2 CalcRelativePosition(RectTransform what, RectTransform relativeTo)
    {
        return CalcRelativePositions(what, relativeTo);
    }
}
