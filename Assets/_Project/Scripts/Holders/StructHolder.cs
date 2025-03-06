using UnityEngine;
using System;

[Serializable]
public class Sound
{
    public string Name;
    public AudioClip[] Clips;
    public float Volume;
    [HideInInspector] public AudioSource Source;
}

public class RoadItem
{
    public GameObject Item;
    public string Name;

    public RoadItem(GameObject item, string name)
    {
        Item = item;
        Name = name;
    }
}

[Serializable]
public struct DifficultyLevelSettings
{
    public float LevelCountdown;
    public float PlayerMoveIncreasedValue;
    public float EmptySpaceDropRateToBeUpdated;
    public float CoinDropRateToBeUpdated;
    public float ObstacleDropRateToBeUpdated;
    public int CoinValueToBeIncreased;
}