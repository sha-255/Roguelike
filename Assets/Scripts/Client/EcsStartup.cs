using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    sealed class EcsStartup : MonoBehaviour
    {
        [SerializeField] Configuration _configuration;
        [SerializeField] SceneService _sceneService;

        EcsWorld _world;
        IEcsSystems _systems;


        void Start()
        {
            _world = new EcsWorld();
            _systems = new EcsSystems(_world);
            _systems
                .Add(new CharacterInitSystem())
                .Add(new CharacterMovementSystem())
                .Add(new CharacterLookSystem())
                .Add(new InputSystem())
                .Add(new GunSystem())
#if UNITY_EDITOR
                .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem())
#endif
                .Inject(_configuration)
                .Inject(_sceneService)
                .Init();
        }

        void Update()
        {
            _systems?.Run();
        }

        void OnDestroy()
        {
            if (_systems != null)
            {
                _systems.Destroy();
                _systems = null;
            }
            if (_world != null)
            {
                _world.Destroy();
                _world = null;
            }
        }
    }
}
