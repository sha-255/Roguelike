using Assets.Scripts.Utils;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{

    public class CharacterMovementSystem : IEcsRunSystem
    {
        private EcsCustomInject<Configuration> _configuration;
        private EcsPoolInject<InputComponent> _inputPool;
        private EcsPoolInject<UnitComponent> _unitPool;
        private EcsFilterInject<Inc<CharacterComponent>> _characterFilter;
        private float _currentAttractionCharacter;

        public void Run(IEcsSystems systems)
        {
            foreach (var characterEntity in _characterFilter.Value)
            {
                if (!_unitPool.Value.Has(characterEntity)) return;
                if (!_inputPool.Value.Has(characterEntity))
                {
                    Debug.LogWarning(ExceptionMessages.InputSystemMessage);
                    return;
                }

                Move(characterEntity);
            }
        }

        private void Move(int entity)
        {
            ref var character = ref _unitPool.Value.Get(entity);
            ref var input = ref _inputPool.Value.Get(entity);
            character.Velocity = _configuration.Value.CharacterConfiguration.Velocity;
            var characterController = character.View.CharacterController;
            var direction = CalculateDirection(input.MoveDirection, characterController.isGrounded, character.Velocity);//
            characterController.Move(direction);
        }

        private Vector3 CalculateDirection(
            Vector3 inputDirection,
            bool isGrounded,
            float velocity = 3)
            => new Vector3(
                inputDirection.x,
                GravityHandling(isGrounded),
                inputDirection.y
                ) * Time.fixedDeltaTime * velocity;

        private float GravityHandling(bool isGrounded, float gravityForce = 20)
            => !isGrounded ?
            _currentAttractionCharacter -= gravityForce :
            _currentAttractionCharacter = 0;
    }
}
