using UnityEngine;

public class GunView : MonoBehaviour
{
    [field: SerializeField] public Transform BulletSpawnPoint { get; private set; }
    [field: SerializeField] public Collider Magazine { get; private set; }
    [field: SerializeField] public float FiringRate { get; private set; } = .2f;
}
