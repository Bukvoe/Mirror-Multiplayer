using Game.CodeBase.Common.StateManagement;

namespace Game.CodeBase.Player.State
{
    public class PlayerMoveState : IEnterState, IExitState, IUpdateState
    {
        private readonly Player _player;
        private readonly PlayerInputService _input;

        public PlayerMoveState(Player player, PlayerInputService input)
        {
            _player = player;
            _input = input;
        }

        public void Enter()
        {
        }

        public void Exit()
        {
        }

        public void Update()
        {
            _player.UpdateHorizontalVelocity(_input.MoveInput);
            _player.UpdateVerticalVelocity();
            _player.MoveByVelocity();
        }
    }
}
