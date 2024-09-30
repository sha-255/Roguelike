using UnityEngine;

public class BulletView : MonoBehaviour
{
    [field: SerializeField] public Rigidbody Rigidbody { get; private set; }
    [field: SerializeField] public TrailRenderer TrailRenderer { get; private set; }
}
