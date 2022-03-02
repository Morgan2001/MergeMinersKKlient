using System;
using System.Collections.Generic;
using UnityEngine;

public enum MiningDevices
{
    Null,
    Ram1,
    Ram2,
    Ram3,
    Ram4,
    Ram5,
    Ram6,
    Ram7,
    Cpu8,
    Cpu9,
    Cpu10,
    Cpu11,
    Cpu12,
    Gpu13,
    Gpu14,
    Gpu15,
    Gpu16,
    Gpu17,
    Monitor18,
    Monitor19,
    Monitor20,
    Monitor21,
    Monitor22,
    Monitor23,
    Monitor24,
    Hdd26,
    Hdd27,
    Hdd28,
    Hdd29,
    Notebook30,
    Notebook31,
    Notebook32,
    Notebook33,
    Notebook34,
    SystemUnit35,
    SystemUnit36,
    SystemUnit37,
    SystemUnit38,
    SystemUnit39,
    SystemUnit40,
    SystemUnit41
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
