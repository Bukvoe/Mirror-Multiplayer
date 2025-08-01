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
    }
}
