using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    public class BulletSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorldInject _world;
        private EcsCustomInject<SceneService> _sceneData;
        private EcsCustomInject<Configuration> _configuration;
        private EcsPoolInject<BulletComponent> _bulletPool;
        private EcsPoolInject<LifetimeComponent> _lifeTimePool;
        private EcsFilterInject<Inc<LifetimeComponent, BulletComponent>> _bulletLifeTimeFilter;
        private EcsFilterInject<Inc<GunComponent>> _gunFilter;
        private EcsPoolInject<GunComponent> _gunPool;
        private float _spawnInterval;

        public void Init(IEcsSystems systems)
        {
            foreach (var gunEntity in _gunFilter.Value)
            {
                ref var gun = ref _gunPool.Value.Get(gunEntity);
                gun.FiringRateInterval = gun.View.FiringRate;
            }
        }

        public void Run(IEcsSystems systems)
        {
            CheckBulletLifetime();

            foreach (var gunEntity in _gunFilter.Value)
            {
                ref var gun = ref _gunPool.Value.Get(gunEntity);
                if ((gun.FiringRateInterval -= Time.fixedDeltaTime) > 0) return;
                gun.FiringRateInterval = gun.View.FiringRate;
                var spawnPoint = gun.View.BulletSpawnPoint;
                CreateBullet(spawnPoint).View.Rigidbody.linearVelocity
                    = SpreadDirection(spawnPoint, 100, 20, 20);
            }
        }

        private BulletComponent CreateBullet(Transform spawnPoint)
        {
            ref var bullet = ref InitializeBullet();
            return SetBulletPosition(ref bullet, spawnPoint);
        }

        private ref BulletComponent InitializeBullet()
        {
            var bulletEntity = _world.Value.NewEntity();
            ref var bullet = ref _bulletPool.Value.Add(bulletEntity);
            ref var bulletLifetime = ref _lifeTimePool.Value.Add(bulletEntity);
            bulletLifetime.Value = _configuration.Value.BulletConfiguration.LifeTime;
            return ref bullet;
        }

        private BulletComponent SetBulletPosition(ref BulletComponent bullet, Transform spawnPoint)
        {
            bullet.View = _sceneData.Value.GetEnemy();
            var transform = bullet.View.transform;
            transform.position = spawnPoint.position;
            transform.rotation = spawnPoint.rotation;
            transform.parent = null;
            return bullet;
        }

        private Vector3 SpreadDirection(
            Transform start,
            float force = 100f,
            float rangeWidth = 0,
            float rangeHeight = 0
            )
        {
            var localSpread = new Vector3();
            localSpread.x = Random.Range(-rangeWidth, rangeWidth);
            localSpread.y = Random.Range(-rangeHeight, rangeHeight);
            localSpread.z = force;
            return start.TransformVector(localSpread);
        }

        private void CheckBulletLifetime()
        {
            foreach (var entity in _bulletLifeTimeFilter.Value)
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
