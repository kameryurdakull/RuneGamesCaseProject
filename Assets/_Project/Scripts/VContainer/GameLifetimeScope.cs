using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Runner.Core
{
    public class GameLifetimeScope : LifetimeScope
    {
        [SerializeField] private PlayerSettings _playerSettings;
        [SerializeField] private MapSettings _mapSettings;
        [SerializeField] private SoundSettings _soundSettings;
        [SerializeField] private DifficultySettings _difficultySettings;
        [SerializeField] private GameData _gameData;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<GameManager>();
            builder.RegisterComponentInHierarchy<InputManager>();
            builder.RegisterComponentInHierarchy<MapGenerator>();
            builder.RegisterComponentInHierarchy<ObjectPool>();
            builder.RegisterComponentInHierarchy<PlayerTriggerController>();
            builder.RegisterComponentInHierarchy<PlayerStepsController>();
            builder.RegisterComponentInHierarchy<PlayerStateMachine>();
            builder.RegisterComponentInHierarchy<AudioManager>();
            builder.RegisterComponentInHierarchy<CameraController>();
            builder.RegisterComponentInHierarchy<UIController>();
            builder.RegisterComponentInHierarchy<VolumeController>();

            // ScriptableObjects
            builder.RegisterComponent(_playerSettings);
            builder.RegisterComponent(_mapSettings);
            builder.RegisterComponent(_soundSettings);
            builder.RegisterComponent(_difficultySettings);
            builder.RegisterComponent(_gameData);
        }
    }
}
