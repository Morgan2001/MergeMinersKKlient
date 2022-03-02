using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelativePositionsCalculator : MonoBehaviour
{
    public GameObject empty;

    public Vector2 CalcRelativePositions(RectTransform what, RectTransform relativeTo)
    {
        empty.transform.SetParent(what);
        empty.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

        empty.transform.SetParent(relativeTo);
        var pos = empty.GetComponent<RectTransform>().anchoredPosition;

        empty.transform.SetParent(transform);
        empty.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

        return pos;
    }
    public Vector2 CalcRelativePosition(GameObject what, GameObject relativeTo)
    {
        return CalcRelativePositions(what.GetComponent<RectTransform>(), relativeTo.GetComponent<RectTransform>());
    }
}
