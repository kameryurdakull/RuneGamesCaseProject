using UnityEngine;

namespace Runner.Core
{
    [CreateAssetMenu(fileName = "SoundSettings", menuName = "Settings/SoundSettings", order = 2)]
    public class SoundSettings : ScriptableObject
    {
        public Sound[] Sounds;
    }
}
