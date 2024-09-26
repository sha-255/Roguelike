using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    public class CharacterLookSystem : IEcsRunSystem
    {
        private EcsFilterInject<Inc<CharacterComponent>> _characterFilter;
        private EcsPoolInject<UnitComponent> _unitPool;
        private EcsPoolInject<InputComponent> _inputPool;
        private EcsCustomInject<Configuration> _configuration;

        public void Run(IEcsSystems systems)
        {
            foreach (var characterEntity in _characterFilter.Value)
            {
                Rotate(characterEntity);
            }
        }

        private void Rotate(int entity)
        {
            if (!_unitPool.Value.Has(entity)) return;

            ref var character = ref _unitPool.Value.Get(entity);
            ref var input = ref _inputPool.Value.Get(entity);
            var target = character.View.RotationTarget;

            if (input.LookDirection.sqrMagnitude < 0.05f) return;
            target.transform.rotation = Quaternion.Lerp(
                target.transform.rotation,
                Quaternion.LookRotation(
                    new Vector3(input.LookDirection.x, 0, input.LookDirection.y)),
                    _configuration.Value.CharacterConfiguration.RotarionSpeed
                    );
        }
    }
}
