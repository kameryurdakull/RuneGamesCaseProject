using UnityEngine;

namespace Runner.Core
{
    [CreateAssetMenu(fileName = "MapSettings", menuName = "Settings/MapSettings", order = 1)]
    public class MapSettings : ScriptableObject
    {
        [Header("Lane Settings")]
        public int LaneCount;
        public int LaneLength;
        [Space]
        [Header("Coin Settings")]
        public int CoinGroupMaxSize;
        public int CoinGroupMinSize;
        public float CoinDropDelay;
        public int CoinValue;

        [Space]
        [Header("Obstacle Settings")]
        public float ObstacleDropDelay;
        [Space]
        [Header("General Settings")]
        public float DropHeight;
        public float DropDuration;
        [Space]
        [Header("Item Drop Rate Settings")]
        public float RateOfDropObstacle;
        public float RateOfDropCoin;
        public float RateOfDropEmptySpace;
        
        [Space]
        [Header("Current Data")]
        public float CurrentRateOfDropObstacle;
        public float CurrentRateOfDropCoin;
        public float CurrentRateOfDropEmptySpace;
        public int CurrentCoinValue;
    }
}
