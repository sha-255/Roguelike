using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    public class GunSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorldInject _world;
        private EcsCustomInject<SceneService> _sceneData;
        private EcsPoolInject<UnitComponent> _unitPool;
        private EcsFilterInject<Inc<UnitComponent>> _unitFilter;
        private EcsPoolInject<GunComponent> _gunPool;
        private EcsFilterInject<Inc<GunComponent>> _gunFilter;
        private EcsPoolInject<BulletComponent> _bulletPool;
        private EcsFilterInject<Inc<BulletComponent>> _bulletFilter;
        private EcsPoolInject<LifetimeComponent> _lifeTimePool;
        private EcsFilterInject<Inc<LifetimeComponent>> _lifeTimeFilter;
        private float _spawnInterval;

        public void Init(IEcsSystems systems)
        {
            foreach (var unitEntity in _unitFilter.Value)
            {
                ref var gun = ref _gunPool.Value.Add(unitEntity);
                ref var unit = ref _unitPool.Value.Get(unitEntity);
                gun.View = unit.View.GunView;
            }
        }

        public void Run(IEcsSystems systems)
        {
            if ((_spawnInterval -= Time.deltaTime) > 0) return;

            foreach (var gunEntity in _gunFilter.Value)
            {
                ref var gun = ref _gunPool.Value.Get(gunEntity);
                var spawnPoint = gun.View.BulletSpawnPoint;

                //to bulletSystem
                var bulletEntity = _world.Value.NewEntity();
                ref var bullet = ref _bulletPool.Value.Add(bulletEntity);
                bullet.View = _sceneData.Value.GetEnemy();
                bullet.View.transform.position = spawnPoint.position;
                bullet.View.transform.parent = null;
                bullet.View.Rigidbody.linearVelocity = SpreadDirection(spawnPoint.position);
            }
        }

        private Vector3 SpreadDirection(Vector3 direction)
        {
            var velocity = new Vector3();               //spread
            velocity.z = direction.z * Random.Range(1, 1.3f);
            velocity.x = direction.x * Random.Range(1, 1.3f);
            return velocity;
        }

        private void CheckBulletLifetime()
        {
            foreach (var entity in _lifeTimeFilter.Value)
            {
                ref var lifetime = ref _lifeTimePool.Value.Get(entity);
                lifetime.Value -= Time.deltaTime;

                if (lifetime.Value > 0) continue;

                ref var bullet = ref _bulletPool.Value.Get(entity);
                _sceneData.Value.ReleaseEnemy(bullet.View);
                _world.Value.DelEntity(entity);
            }
        }
    }
}
