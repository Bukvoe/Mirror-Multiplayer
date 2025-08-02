using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.CodeBase.Character
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerInputService : MonoBehaviour
    {
        private const string MoveActionId = "Move";

        [SerializeField] private PlayerInput _playerInput;
        private InputAction _moveAction;

        public Vector2 MoveInput => _moveAction.ReadValue<Vector2>();

        private void Awake()
        {
            _moveAction = _playerInput.currentActionMap[MoveActionId];
        }
    }
}
