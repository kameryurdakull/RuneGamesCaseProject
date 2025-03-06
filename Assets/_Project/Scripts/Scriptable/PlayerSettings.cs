using UnityEngine;

namespace Runner.Core
{
    [CreateAssetMenu(fileName = "PlayerSettings", menuName = "Settings/PlayerSettings", order = 0)]
    public class PlayerSettings : ScriptableObject
    {
        public float MoveSpeed;
        public float SwipeTreshold;
        public float SwipeDuration;
        public float MoveDistance;

        [Space]
        [Header("Current Data")]
        public float CurrentMoveSpeed;
    }
}
