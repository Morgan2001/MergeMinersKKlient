﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeOffer", menuName = "ScriptableObjects/UpgradeOffer", order = 1)]
public class UpgradeOffer : ScriptableObject
{
    public Sprite Sprite;
    public string Description;
    public int Cost;
    public ResourceTypes Currency;
    public Boosts BoostType;
    public int Power;
    public int Duration;
}
