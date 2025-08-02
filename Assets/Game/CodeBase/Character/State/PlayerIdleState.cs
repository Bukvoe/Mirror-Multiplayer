using Game.CodeBase.Common;
using Game.CodeBase.Common.StateManagement;
using UnityEngine;

namespace Game.CodeBase.Character.State
{
    public class PlayerIdleState : IEnterState, IExitState, IUpdateState
    {
        private readonly Animator _animator;
        private readonly Player _player;

        public PlayerIdleState(Player player, Animator animator)
        {
            _player = player;
            _animator = animator;
        }

        public void Enter()
        {
            _player.UpdateHorizontalVelocity(Vector2.zero);
            _animator.SetState(Player.Animation.Idle);
        }

        public void Exit()
        {
        }

        public void Update()
        {
            _player.UpdateVerticalVelocity();
            _player.MoveByVelocity();

            _player.UpdateSayHello();
        }
    }
}
