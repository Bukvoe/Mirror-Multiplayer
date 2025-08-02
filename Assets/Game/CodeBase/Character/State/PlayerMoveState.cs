using Game.CodeBase.Common;
using Game.CodeBase.Common.StateManagement;
using UnityEngine;

namespace Game.CodeBase.Character.State
{
    public class PlayerMoveState : IEnterState, IExitState, IUpdateState
    {
        private readonly Animator _animator;
        private readonly Player _player;
        private readonly PlayerInputService _input;

        public PlayerMoveState(Player player, PlayerInputService input, Animator animator)
        {
            _player = player;
            _input = input;
            _animator = animator;
        }

        public void Enter()
        {
            _animator.SetState(Player.Animation.Run);
        }

        public void Exit()
        {
        }

        public void Update()
        {
            _player.UpdateHorizontalVelocity(_input.MoveInput);
            _player.UpdateVerticalVelocity();
            _player.MoveByVelocity();

            _player.UpdateModelRotation(_input.MoveInput);

            _player.UpdateSayHello();
            _player.UpdateSpawnCube();
        }
    }
}
