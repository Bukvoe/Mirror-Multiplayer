using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.CodeBase.Character
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerInputService : MonoBehaviour
    {
        private const string MoveActionId = "Move";
        private const string SayActionId = "Say";
        private const string SpawnActionId = "Spawn";

        [SerializeField] private PlayerInput _playerInput;
        private InputAction _moveAction;
        private InputAction _sayAction;
        private InputAction _spawnAction;

        public Vector2 MoveInput => _moveAction.ReadValue<Vector2>();
        public bool SayActionPressed => _sayAction.WasPressedThisFrame();
        public bool SpawnActionPressed => _spawnAction.WasPressedThisFrame();

        private void Awake()
        {
            _moveAction = _playerInput.currentActionMap[MoveActionId];
            _sayAction = _playerInput.currentActionMap[SayActionId];
            _spawnAction = _playerInput.currentActionMap[SpawnActionId];
        }
    }
}
