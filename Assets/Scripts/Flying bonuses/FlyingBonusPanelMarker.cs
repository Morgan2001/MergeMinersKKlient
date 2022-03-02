using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FlyingBonusPanelMarker : MonoBehaviour
{
    public float MinPosY;
    public float MaxPosY;

    public FlyingBonusPanelMarker SetToRandomPoint()
    {
        GetComponent<RectTransform>().anchoredPosition = 
            new Vector2(GetComponent<RectTransform>().anchoredPosition.x, UnityEngine.Random.Range(MinPosY, MaxPosY));
        return this;
    }
}
