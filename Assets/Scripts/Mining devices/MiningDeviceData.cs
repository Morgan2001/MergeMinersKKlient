using System;
using System.Collections.Generic;
using UnityEngine;

public enum MiningDevices
{
    Null,
    Cpu1,
    Ram2,
    Gpu3,
    Hdd4,
    Monitor5,
    SystemUnit6,
    Notebook7,
    Cpu8,
    Ram9,
    Gpu10,
    Hdd11,
    Monitor12,
    SystemUnit13,
    Notebook14,
    Cpu15,
    Ram16,
    Gpu17,
    Hdd18,
    Monitor19,
    SystemUnit20,
    Notebook21,
    Cpu22,
    Ram23,
    Gpu24,
    Hdd25,
    Monitor26,
    SystemUnit27,
    Notebook28,
    Cpu29,
    Ram30,
    Gpu31,
    Hdd32,
    Monitor33,
    SystemUnit34,
    Notebook35,
    Cpu36,
    Ram37,
    Gpu38,
    Hdd39,
    Monitor40,
    SystemUnit41,
    Notebook42,
    Cpu43,
    Ram44,
    Gpu45,
    Hdd46,
    Monitor47,
    SystemUnit48,
    Notebook49
}

[CreateAssetMenu(fileName = "MiningDevice", menuName = "ScriptableObjects/MiningDevice", order = 1)]
public class MiningDeviceData : ScriptableObject
{
    public int id;
    public string Name;
    public MiningDevices Type;
    public int Level;
    public float CoinsPerSecond;
    public float BuyPrice;
    public Sprite Sprite;
    public Sprite SpriteWithOutline;
}
