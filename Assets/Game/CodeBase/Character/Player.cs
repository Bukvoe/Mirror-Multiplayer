using System.Collections.Generic;
using Game.CodeBase.Character.State;
using Game.CodeBase.Common.StateManagement;
using Game.CodeBase.Network;
using Game.CodeBase.Props;
using Mirror;
using UnityEngine;

namespace Game.CodeBase.Character
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(PlayerInputService))]
    public class Player : NetworkBehaviour
    {
        public enum Animation
        {
            Idle = 0,
            Run = 1,
        }

        private const float MovementThreshold = 0.01f;

        [SyncVar(hook = nameof(OnNickNameChange))]
        private string _nickname = string.Empty;

        [SerializeField] private CharacterController _controller;
        [SerializeField] private PlayerInputService _input;
        [SerializeField] private NetworkAnimator _networkAnimator;
        [SerializeField] private Transform _modelTransform;
        [SerializeField] private Transform _spawnCubePoint;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _rotationSpeed;
        [SerializeField] private float _pushForce;
        [SerializeField] private float _gravity = -9.81f;

        [Header("Prefabs")]
        [SerializeField] private Cube _cubePrefab;

        private StateMachine _stateMachine;
        private Vector3 _velocity;

        public string Nickname => _nickname;

        public override void OnStartLocalPlayer()
        {
            CmdSetNickname(CustomNetworkManager.Instance.Nickname);

            var states = new Dictionary<IEnterState, List<ITransition>>()
            {
                {
                    new PlayerIdleState(this, _networkAnimator.animator),
                    new List<ITransition>
                    {
                        new Transition<PlayerMoveState>(() =>
                            _input.MoveInput.sqrMagnitude > MovementThreshold && _controller.isGrounded),
                    }
                },
                {
                    new PlayerMoveState(this, _input, _networkAnimator.animator),
                    new List<ITransition>
                    {
                        new Transition<PlayerIdleState>(() =>
                            _input.MoveInput.sqrMagnitude <= MovementThreshold || !_controller.isGrounded),
                    }
                },
            };

            _stateMachine = new StateMachine(states);
        }

        public override void OnStartClient()
        {
            CustomNetworkManager.Instance.RegisterPlayer(this);
        }

        public override void OnStopClient()
        {
            CustomNetworkManager.Instance.UnregisterPlayer(this);
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

        public void UpdateModelRotation(Vector2 input)
        {
            if (input.sqrMagnitude < MovementThreshold)
            {
                return;
            }

            var direction = new Vector3(input.x, 0f, input.y).normalized;
            var targetRotation = Quaternion.LookRotation(direction);
            _modelTransform.rotation = Quaternion.Slerp(_modelTransform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
        }

        public void UpdateSayHello()
        {
            if (_input.SayActionPressed)
            {
                CmdSayHello();
            }
        }

        public void UpdateSpawnCube()
        {
            if (_input.SpawnActionPressed)
            {
                CmdSpawnCube();
            }
        }

        [Command]
        private void CmdSpawnCube()
        {
            var cube = Instantiate(_cubePrefab.gameObject, _spawnCubePoint.position, Quaternion.identity);
            NetworkServer.Spawn(cube, ownerConnection: null);
        }

        [Command]
        private void CmdSayHello()
        {
            RpcReceiveSayHello();
        }

        [ClientRpc(includeOwner = true)]
        private void RpcReceiveSayHello()
        {
            Debug.Log($"Hello from {Nickname}");
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

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (hit.gameObject.TryGetComponent<Cube>(out var cube))
            {
                CmdRequestPush(cube.netIdentity, new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z));
            }
        }

        [Command]
        private void CmdRequestPush(NetworkIdentity cubeIdentity, Vector3 direction)
        {
            if (cubeIdentity.TryGetComponent<Cube>(out var cube))
            {
                cube.Rigidbody.AddForce(direction.normalized * _pushForce, ForceMode.Impulse);
            }
        }

        public void TeleportToPoint(Vector3 position)
        {
            _controller.enabled = false;
            transform.position = position;
            _velocity = Vector3.zero;
            _controller.enabled = true;
        }
    }
}
