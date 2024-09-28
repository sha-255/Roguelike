using UnityEngine;

[CreateAssetMenu]
public class CharacterConfiguration : ScriptableObject
{
    [field: SerializeField] public float Velocity = 3;
    [field: SerializeField] public float RotarionSpeed = .08f;
}
