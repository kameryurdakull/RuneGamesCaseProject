using DG.Tweening;
using Game.Core;
using Unity.Cinemachine;
using UnityEngine;
using VContainer;

namespace Runner.Core
{
    public class PlayerStateMachine : StateMachine
    {
        #region States
        public IdleState IdleState { get; private set; }
        public MovementState MovementState { get; private set; }
        public FallState FallState { get; private set; }
        #endregion

        [field: SerializeField] public CinemachineFollow CinemachineFollow { get; private set; }
        [field: SerializeField] public Animator Animator { get; private set; }

        public InputManager InputManager { get; private set; }
        public PlayerSettings PlayerSettings { get; private set; }
        public AudioManager AudioManager { get; private set; }
        public CameraController CameraController { get; private set; }
        public VolumeController VolumeController { get; private set; }

        [Inject]
        public void Construct(
            InputManager inputManager,
            PlayerSettings playerSettings,
            AudioManager audioManager,
            CameraController cameraController,
            VolumeController volumeController)
        {
            InputManager = inputManager;
            PlayerSettings = playerSettings;
            AudioManager = audioManager;
            CameraController = cameraController;
            VolumeController = volumeController;
        }

        private void OnEnable()
        {
            EventManagers.OnNextLevel += OnNextLevel;
        }

        private void OnDisable()
        {
            EventManagers.OnNextLevel -= OnNextLevel;
        }

        private void Awake()
        {
            SetState();
        }

        protected override void Start()
        {
            base.Start();
        }

        private void SetState()
        {
            IdleState = new(this);
            MovementState = new(this);
            FallState = new(this);
        }

        protected override BaseState GetInitialState()
        {
            return IdleState;
        }

        protected override void Update()
        {
            base.Update();

        }

        public void OnGameStarted()
        {
            ChangeState(MovementState);
        }

        public void OnFall()
        {
            ChangeState(FallState);
        }

        private void OnNextLevel(DifficultyLevelSettings settings)
        {
            float newMoveSpeed = PlayerSettings.MoveSpeed + settings.PlayerMoveIncreasedValue;
            float currentAnimationSpeed = Animator.GetFloat("MoveSpeed");
            float newAnimationMoveSpeed = (settings.PlayerMoveIncreasedValue / 10) + currentAnimationSpeed;

            // Player hareket etme hýzýný arttýr
            DOTween.To(() => PlayerSettings.MoveSpeed, x => PlayerSettings.MoveSpeed = x, newMoveSpeed, .5f);

            // Player run animasyonu hýzýný arttýr
            DOTween.To(() => currentAnimationSpeed, x => currentAnimationSpeed = x, newAnimationMoveSpeed, .5f)
                .OnUpdate(() =>
                {
                    Animator.SetFloat("MoveSpeed", newAnimationMoveSpeed);
                });
            
        }
    }
}
