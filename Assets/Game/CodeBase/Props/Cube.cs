using Mirror;
using UnityEngine;

namespace Game.CodeBase.Props
{
    [RequireComponent(typeof(Rigidbody))]
    public class Cube : NetworkBehaviour
    {
        [field: SerializeField] public Rigidbody Rigidbody { get; private set; }

        public override void OnStartClient()
        {
            Rigidbody.isKinematic = !isServer;
        }
    }
}
