using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public Dictionary<int, int> gamePointsData;

    public uint level = 0;
    public uint experience = 0;
    public uint score = 0;
    public uint multiplier = 1;

    public uint worldPercent = 100;
    
    public uint musicVolume = 100;
    public uint SFXVolume = 100;
}
