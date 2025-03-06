using UnityEngine;

namespace Runner.Core
{
    [CreateAssetMenu(fileName = "DifficultySettings", menuName = "Settings/DifficultySettings", order = 3)]
    public class DifficultySettings : ScriptableObject
    {
        public DifficultyLevelSettings[] DifficultyLevelSettings;
    }
}
