using Game.CodeBase.Common.StateManagement;
using UnityEngine;

namespace Game.CodeBase.Character.State
{
    public class PlayerIdleState : IEnterState, IExitState, IUpdateState
    {
        private readonly Player _player;

        public PlayerIdleState(Player player)
        {
            _player = player;
        }

        public void Enter()
        {
            _player.UpdateHorizontalVelocity(Vector2.zero);
        }

        public void Exit()
        {
        }

        public void Update()
        {
            _player.UpdateVerticalVelocity();
            _player.MoveByVelocity();
        }
    }
}
