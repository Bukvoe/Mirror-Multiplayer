using System.Collections.Generic;
using Game.CodeBase.Common.StateManagement;
using Game.CodeBase.Network;
using Game.CodeBase.Player.State;
using Mirror;
using UnityEngine;

namespace Game.CodeBase.Player
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(PlayerInputService))]
    public class Player : NetworkBehaviour
    {
        private const float MovementThreshold = 0.01f;

        [SyncVar(hook = nameof(OnNickNameChange))]
        private string _nickname = string.Empty;

        [SerializeField] private CharacterController _controller;
        [SerializeField] private PlayerInputService _input;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _gravity = -9.81f;

        private StateMachine _stateMachine;
        private Vector3 _velocity;

        public override void OnStartLocalPlayer()
        {
            if (NetworkManager.singleton is CustomNetworkManager customNetworkManager)
            {
                CmdSetNickname(customNetworkManager.Nickname);
            }

            var states = new Dictionary<IEnterState, List<ITransition>>()
            {
                {
                    new PlayerIdleState(this),
                    new List<ITransition>
                    {
                        new Transition<PlayerMoveState>(() =>
                            _input.MoveInput.magnitude > MovementThreshold && _controller.isGrounded),
                    }
                },
                {
                    new PlayerMoveState(this, _input),
                    new List<ITransition>
                    {
                        new Transition<PlayerIdleState>(() =>
                            _input.MoveInput.magnitude <= MovementThreshold || !_controller.isGrounded),
                    }
                },
            };

            _stateMachine = new StateMachine(states);
        }

        private void Update()
        {
            _stateMachine?.Update();
        }

        public void UpdateHorizontalVelocity(Vector2 input)
        {
            var horizontalMove = new Vector3(input.x, 0, input.y);
            horizontalMove = transform.TransformDirection(horizontalMove);

            _velocity.x = horizontalMove.x * _moveSpeed;
            _velocity.z = horizontalMove.z * _moveSpeed;
        }

        public void UpdateVerticalVelocity()
        {
            if (_controller.isGrounded && _velocity.y < 0)
            {
                _velocity.y = _gravity;
            }
            else
            {
                _velocity.y += _gravity * Time.deltaTime;
            }
        }

        public void MoveByVelocity()
        {
            _controller.Move(_velocity * Time.deltaTime);
        }

        [Command]
        private void CmdSetNickname(string nickname)
        {
            if (string.IsNullOrWhiteSpace(nickname))
            {
                nickname = $"Player{Random.Range(1000, 10000)}";
            }

            _nickname = nickname;
        }

        private void OnNickNameChange(string oldNickname, string newNickname)
        {
            _nickname = newNickname;
        }
    }
}
