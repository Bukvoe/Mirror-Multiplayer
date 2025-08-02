using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.CodeBase.Character
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerInputService : MonoBehaviour
    {
        private const string MoveActionId = "Move";
        private const string SayActionId = "Say";

        [SerializeField] private PlayerInput _playerInput;
        private InputAction _moveAction;
        private InputAction _sayAction;

        public Vector2 MoveInput => _moveAction.ReadValue<Vector2>();
        public bool SayActionPressed => _sayAction.WasPressedThisFrame();

        private void Awake()
        {
            _moveAction = _playerInput.currentActionMap[MoveActionId];
            _sayAction = _playerInput.currentActionMap[SayActionId];
        }
    }
}
