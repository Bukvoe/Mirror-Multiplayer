using System.Collections.Generic;
using Game.CodeBase.Common.StateManagement;
using Game.CodeBase.Player.State;
using Mirror;
using UnityEngine;

namespace Game.CodeBase.Player
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(PlayerInputService))]
    public class Player : NetworkBehaviour
    {
        [SerializeField] private CharacterController _controller;
        [SerializeField] private PlayerInputService _input;

        private StateMachine _stateMachine;

        public override void OnStartLocalPlayer()
        {
            var states = new Dictionary<IEnterState, List<ITransition>>()
            {
                {
                    new PlayerIdleState(this),
                    new List<ITransition>()
                },
                {
                    new PlayerMoveState(this),
                    new List<ITransition>()
                },
            };

            _stateMachine = new StateMachine(states);
        }

        private void Update()
        {
            _stateMachine?.Update();
        }
    }
}
