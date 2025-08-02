using System.Collections;
using Game.CodeBase.Cameras;
using Game.CodeBase.Character;
using Game.CodeBase.Props;
using Mirror;
using UnityEngine;

namespace Game.CodeBase.World
{
    [RequireComponent(typeof(Collider))]
    public class OutsideArenaTrigger : MonoBehaviour
    {
        [SerializeField] private float _cubeDestroyDelayInSeconds;
        [SerializeField] private float _respawnDelayInSeconds;
        [SerializeField] private Transform[] _spawnPoints;
        [SerializeField] private PlayerFollowCamera _playerFollowCamera;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Player>(out var player) && player.isLocalPlayer)
            {
                StartCoroutine(RespawnOnArenaAfterDelay(player, _respawnDelayInSeconds));
            }
            else if (other.TryGetComponent<Cube>(out var cube) && cube.isServer)
            {
                StartCoroutine(DestroyAfterDelay(cube.gameObject, _cubeDestroyDelayInSeconds));
            }
        }

        [Server]
        private IEnumerator RespawnOnArenaAfterDelay(Player player, float seconds)
        {
            _playerFollowCamera.Unfollow();

            yield return new WaitForSeconds(seconds);

            var randomSpawnPoint = _spawnPoints[Random.Range(0, _spawnPoints.Length)];
            player.TeleportToPoint(randomSpawnPoint.position);

            _playerFollowCamera.Follow(player);
        }

        [Server]
        private IEnumerator DestroyAfterDelay(GameObject objectToDestroy, float seconds)
        {
            yield return new WaitForSeconds(seconds);

            NetworkServer.Destroy(objectToDestroy);
        }
    }
}
