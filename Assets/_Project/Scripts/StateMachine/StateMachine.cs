using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core
{
    public class StateMachine : MonoBehaviour
    {
        public BaseState CurrentState;

        protected virtual void Start()
        {
            CurrentState = GetInitialState();

            CurrentState?.Enter();
        }

        protected virtual void Update()
        {
            CurrentState?.UpdateLogic();
        }

        protected virtual void LateUpdate()
        {
            CurrentState?.UpdatePhysics();
        }

        public void ChangeState(BaseState newState)
        {
            CurrentState.Exit();
            CurrentState = newState;
            CurrentState.Enter();
        }

        protected virtual BaseState GetInitialState()
        {
            return null;
        }
    }
}
