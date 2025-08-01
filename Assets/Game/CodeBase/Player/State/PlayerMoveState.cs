using Game.CodeBase.Common.StateManagement;

namespace Game.CodeBase.Player.State
{
    public class PlayerMoveState : IEnterState, IExitState, IUpdateState
    {
        private readonly Player _player;

        public PlayerMoveState(Player player)
        {
            _player = player;
        }

        public void Enter()
        {
        }

        public void Exit()
        {
        }

        public void Update()
        {
        }
    }
}
