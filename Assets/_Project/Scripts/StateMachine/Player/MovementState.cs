using DG.Tweening;
using Extensions;
using Game.Core;
using UnityEngine;

namespace Runner.Core
{
    public class MovementState : BaseState
    {
        readonly PlayerStateMachine _sm;
        readonly Transform transform;

        private const int _maxSwipeStep = 1;
        private const float _jumpHeight = 1.5f;
        private Vector2 _startTouchPosition;
        private Vector2 _endTouchPosition;
        private bool _isTouch;
        private bool _canJump;
        private bool _canRoll;
        private MovementType _movementType;
        private float _currentXPos;
        private float _maxXPos;
        private float _minXPos;

        public MovementState(StateMachine stateMachine) : base("Movement", stateMachine)
        {
            _sm = (PlayerStateMachine)stateMachine;
            transform = _sm.transform;
        }

        public override void Enter()
        {
            base.Enter();
            _sm.Animator.SetTrigger(CLIPS.RUN);
            _sm.AudioManager.Play(SOUNDS.LETSMOVE);
            // Max & Min gidilecek x pozisyonlarýný, max swipe adýmý ile hareket aralýðýný çarpýp buluyoruz
            _maxXPos = _maxSwipeStep * _sm.PlayerSettings.MoveDistance;
            _minXPos = _maxSwipeStep * -_sm.PlayerSettings.MoveDistance;
        }

        public override void UpdateLogic()
        {
            base.UpdateLogic();

            transform.Translate(_sm.PlayerSettings.MoveSpeed * Time.deltaTime * Vector3.forward);

            if (_sm.InputManager.IsTouch)
            {
                if (!_isTouch)
                {
                    _isTouch = true;
                    _startTouchPosition = _sm.InputManager.TouchStartPosition;
                }
            }
            else if (_isTouch)
            {
                _isTouch = false;
                _endTouchPosition = _sm.InputManager.TouchPosition;
                DetectSwipe();
            }
        }

        private void DetectSwipe()
        {
            Vector2 swipeDelta = _endTouchPosition - _startTouchPosition;

            if (swipeDelta.magnitude > _sm.PlayerSettings.SwipeTreshold)
            {
                if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y)) // Swipe deðeri yüksek olan düzlemi bul
                {
                    if (swipeDelta.x > 0)
                    {
                        _movementType = MovementType.RIGHT;
                    }
                    else
                    {
                        _movementType = MovementType.LEFT;
                    }
                }
                else
                {
                    if (swipeDelta.y > 0)
                    {
                        _movementType = MovementType.UP;
                    }
                    else
                    {
                        _movementType = MovementType.DOWN;
                    }
                }
                OnMovement();
            }
        }

        private void OnMovement()
        {
            //var sound = new AudioSource();

            switch (_movementType)
            {
                // Right move
                case MovementType.RIGHT:
                    if (transform.position.x < _maxXPos)
                    {
                        _currentXPos += _sm.PlayerSettings.MoveDistance;
                        MoveInTheXPlane(_currentXPos);
                    }
                    break;
                // Left move
                case MovementType.LEFT:
                    if (transform.position.x > _minXPos)
                    {
                        _currentXPos -= _sm.PlayerSettings.MoveDistance;
                        MoveInTheXPlane(_currentXPos);
                    }
                    break;
                // Jump Move
                case MovementType.UP:
                    Jump();
                    break;
                // Slide Move
                case MovementType.DOWN:
                    Roll();
                    break;
            }
        }

        private void MoveInTheXPlane(float newPos)
        {
            transform.DOKill();
            transform.DOMoveX(newPos, _sm.PlayerSettings.SwipeDuration);

            // Saða sola swipe hareketi yaparken kamerayý biraz
            // daha ortalamasý için follow offset ayarýný düzenliyoruz.
            float xOffset = _sm.CinemachineFollow.FollowOffset.x;
            DOTween.To(() => xOffset, x => xOffset = x, -newPos / 4, _sm.PlayerSettings.SwipeDuration / 2)
                .OnUpdate(() => _sm.CinemachineFollow.FollowOffset.x = xOffset)
                .SetEase(Ease.Linear);
        }

        private void Jump()
        {
            if (!_canJump)
            {
                _canJump = true;
                _sm.Animator.SetTrigger(CLIPS.JUMP);
                _sm.AudioManager.Play(SOUNDS.JUMP);
                float jumpAnimLength = _sm.Animator.GetAnimationClipDuration(CLIPS.JUMP);

                Sequence jumpSeq = DOTween.Sequence();
                jumpSeq.Append(transform.DOMoveY(_jumpHeight, jumpAnimLength / 2));
                jumpSeq.Append(transform.DOMoveY(0, jumpAnimLength / 2));
                jumpSeq.OnComplete(() =>
                {
                    _canJump = false;
                });
            }
        }

        private void Roll()
        {
            if (!_canRoll)
            {
                _canRoll = true;
                _sm.Animator.SetTrigger(CLIPS.ROLL);
                _sm.AudioManager.Play(SOUNDS.ROLL);
                float rollAnimLength = _sm.Animator.GetAnimationClipDuration(CLIPS.ROLL);
                DOVirtual.DelayedCall(rollAnimLength, () =>
                {
                    _canRoll = false;
                });

            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
