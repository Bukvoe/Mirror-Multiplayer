using System.Collections.Generic;
using Game.CodeBase.Common.StateManagement;
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
            var states = new Dictionary<IEnterState, List<ITransition>>();

            _stateMachine = new StateMachine(states);
        }
    }
}
