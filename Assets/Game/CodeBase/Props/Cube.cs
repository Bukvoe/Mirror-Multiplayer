using DG.Tweening;
using Mirror;
using UnityEngine;

namespace Game.CodeBase.Props
{
    [RequireComponent(typeof(Rigidbody))]
    public class Cube : NetworkBehaviour
    {
        private const float SpawnTweenDuration = 1f;

        [field: SerializeField] public Rigidbody Rigidbody { get; private set; }

        public override void OnStartClient()
        {
            Rigidbody.isKinematic = !isServer;

            transform.localScale = Vector3.zero;

            transform.DOScale(Vector3.one, SpawnTweenDuration)
                .SetEase(Ease.OutBounce);

            transform.DOShakeRotation(SpawnTweenDuration, new Vector3(15f, 30f, 15f))
                .SetEase(Ease.OutQuad);
        }
    }
}
