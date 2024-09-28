using UnityEngine;

[CreateAssetMenu]
public class Configuration : ScriptableObject
{
    [field: SerializeField] public CharacterConfiguration CharacterConfiguration;
    [field: SerializeField] public BulletConfiguration BulletConfiguration;
}
