using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    sealed class EcsStartup : MonoBehaviour
    {
        private const string CharacterGroupName = "Character";

        [SerializeField] private Configuration _configuration;
        [SerializeField] private SceneService _sceneService;

        private EcsWorld _world;
        private IEcsSystems _update;
        private IEcsSystems _fixedUpdate;

        void Start()
        {
            _world = new EcsWorld();
            _update = new EcsSystems(_world);
            _update
                .Add(new CharacterInitSystem())
                .Add(new CharacterMovementSystem())
                .Add(new CharacterLookSystem())
                .Add(new InputSystem())
                .Add(new GunSystem())
                .Add(new EnemiesSystem())//TODO: complite system
#if UNITY_EDITOR
                .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem())
#endif
                .Inject(_configuration)
                .Inject(_sceneService)
                .Init();
            _fixedUpdate = new EcsSystems(_world);
            _fixedUpdate
                .Add(new BulletSystem())
#if UNITY_EDITOR
                .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem())
#endif
                .Inject(_configuration)
                .Inject(_sceneService)
                .Init();
        }

        void Update()
        {
            _update?.Run();
        }

        void FixedUpdate()
        {
            _fixedUpdate?.Run();
        }

        void OnDestroy()
        {
            if (_update != null)
            {
                _update.Destroy();
                _update = null;
            }
            if (_fixedUpdate != null)
            {
                _fixedUpdate.Destroy();
                _fixedUpdate = null;
            }
            if (_world != null)
            {
                _world.Destroy();
                _world = null;
            }
        }
    }
}
