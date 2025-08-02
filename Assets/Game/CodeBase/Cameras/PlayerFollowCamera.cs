using Cinemachine;
using Game.CodeBase.Character;
using Game.CodeBase.Network;
using UnityEngine;

namespace Game.CodeBase.Cameras
{
    [RequireComponent(typeof(CinemachineVirtualCameraBase))]
    public class PlayerFollowCamera : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera _camera;

        private void Start()
        {
            CustomNetworkManager.Instance.PlayerAdded += OnPlayerAdded;
        }

        private void OnDestroy()
        {
            if (CustomNetworkManager.Instance != null)
            {
                CustomNetworkManager.Instance.PlayerAdded -= OnPlayerAdded;
            }
        }

        private void OnPlayerAdded(Player player)
        {
            if (player.isLocalPlayer)
            {
                _camera.Follow = player.transform;
                _camera.LookAt = player.transform;
            }
        }
    }
}
