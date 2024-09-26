using UnityEngine;

public class UnitView : MonoBehaviour
{
    [field: SerializeField] public CharacterController CharacterController { get; private set; }
    [field: SerializeField] public GameObject RotationTarget { get; private set; }
    [field: SerializeField] public GunView GunView { get; private set; }
}
