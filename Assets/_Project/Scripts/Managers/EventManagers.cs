using System;

namespace Runner.Core
{
    public class EventManagers
    {
        // UI Actions
        public static Action OnStartGame;
        public static Action OnRetryGame;

        // Player Actions
        public static Action OnFall;

        // Difficulty
        public static Action<DifficultyLevelSettings> OnNextLevel;

    }
}
