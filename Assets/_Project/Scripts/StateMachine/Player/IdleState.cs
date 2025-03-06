using Game.Core;
using UnityEngine;

namespace Runner.Core
{
    public class IdleState : BaseState
    {
        PlayerStateMachine _sm;

        public IdleState(StateMachine stateMachine) : base("Idle", stateMachine)
        {
            _sm = (PlayerStateMachine)stateMachine;
        }

        public override void Enter()
        {
            base.Enter();
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
