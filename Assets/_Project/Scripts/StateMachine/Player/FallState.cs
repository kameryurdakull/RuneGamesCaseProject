using Game.Core;
using UnityEngine;
using DG.Tweening;

namespace Runner.Core
{
    public class FallState : BaseState
    {
        PlayerStateMachine _sm;
        Transform transform;

        public FallState(StateMachine stateMachine) : base("Fall", stateMachine)
        {
            _sm = (PlayerStateMachine)stateMachine;
            transform = _sm.transform;
        }

        public override void Enter()
        {
            base.Enter();

            _sm.Animator.SetTrigger(CLIPS.FALL);
            _sm.AudioManager.Play(SOUNDS.FALL);
            _sm.CameraController.Shake(2.5f, .4f);
            _sm.VolumeController.VignetteEffect(.4f, .4f);
            transform.DOKill();
            transform.DOMoveZ(-2f, .75f).SetRelative(true);
            EventManagers.OnFall?.Invoke();
        }

        public override void UpdateLogic()
        {
            base.UpdateLogic();
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
