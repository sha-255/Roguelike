using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    public class InputSystem : IEcsInitSystem, IEcsRunSystem, IEcsDestroySystem
    {
        private EcsCustomInject<SceneService> _sceneData;
        private EcsPoolInject<UnitComponent> _characterPool;
        private EcsPoolInject<InputComponent> _inputPool;
        private EcsFilterInject<Inc<CharacterComponent>> _characterFilter;
        private EcsFilterInject<Inc<InputComponent>> _inputFilter;
#if UNITY_STANDALONE_WIN || UNITY_IOS || UNITY_STANDALONE_LINUX
        PlayerInputActions.CharacterPCActions _characterActions;
#elif UNITY_ANDROID || UNITY_IPHONE
        PlayerInputActions.CharacterMobileActions _characterActions;
#endif

        public void Init(IEcsSystems systems)
        {
            var inputActions = _sceneData.Value.InputActions;
            _characterActions
#if UNITY_STANDALONE_WIN || UNITY_IOS || UNITY_STANDALONE_LINUX
                = inputActions.CharacterPC;
#elif UNITY_ANDROID || UNITY_IPHONE
                = inputActions.CharacterMobile;
#endif
            _characterActions.Enable();
            foreach (var entity in _characterFilter.Value)
            {
                _inputPool.Value.Add(entity);
            }
        }

        public void Run(IEcsSystems systems)
        {
            var moveDirection = _characterActions.Move.ReadValue<Vector2>();
            var lookDirection = _characterActions.Look.ReadValue<Vector2>();
            foreach (var entity in _inputFilter.Value)
            {
                ref var input = ref _inputPool.Value.Get(entity);
                input.MoveDirection = moveDirection;
                input.LookDirection = lookDirection;
            }
        }

        public void Destroy(IEcsSystems systems)
        {
            _characterActions.Disable();
        }
    }
}
