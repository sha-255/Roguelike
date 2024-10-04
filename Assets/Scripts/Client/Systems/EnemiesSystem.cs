using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Client
{
    public class EnemiesSystem : IEcsRunSystem
    {
        private EcsWorldInject _world;
        private EcsCustomInject<SceneService> _sceneService;
        private EcsPoolInject<EnemyComponent> _enemiesPool;
        private EcsPoolInject<UnitComponent> _unitsPool;
        private EcsFilterInject<Inc<EnemyComponent, UnitComponent>> _enemiesFilter;

        public void Run(IEcsSystems systems)
        {
            CreateEnemy();
        }

        private int CreateEnemy()
        {
            if (!TryGetOutOfScreenPosition(out var position)) return -1;
            var entity = _world.Value.NewEntity();
            var enemy = _enemiesPool.Value.Add(entity);
            _unitsPool.Value.Add(entity);
            enemy.View = CreateSphere(position);
            return entity;
        }

        private GameObject CreateSphere(Vector3 position = new Vector3())
        {
            var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            sphere.transform.position = position;
            sphere.GetComponent<SphereCollider>().enabled = false;
            return sphere;
        }

        private bool TryGetOutOfScreenPosition(out Vector3 position)
        {
            var camera = _sceneService.Value.MainCamera;
            Vector3 local1 = GetRandomBorderPoint(new Vector2(0f, 1f));
            var local2 = local1;
            local1.z = 1;
            local2.z = 2;
            var point1 = camera.ViewportToWorldPoint(local1);
            var point2 = camera.ViewportToWorldPoint(local2);
            if (Physics.Raycast(point1, point2 - point1, out var hit))
            {
                Debug.DrawRay(point1, point2 - point1, Color.green, 999);
                position = hit.point;
                return true;
            }
            Debug.DrawRay(point1, point2 - point1, Color.red, 999);
            position = Vector3.zero;
            return false;
        }

        private Vector2 GetRandomBorderPoint(Vector2 mask, float size = 0)
        {
            bool RandomBool() => Random.Range(0, 2) == 0;
            var border = 1;
            var marginMask = new Vector2(-Mathf.Abs(mask.x - size), Mathf.Abs(mask.y + size));
            var marginSize = Mathf.Abs(border) + Mathf.Abs(size);

            var minInclusive = -marginSize + marginMask.x;
            var maxInclusive = marginSize + marginMask.y;
            var x = Random.Range(minInclusive, maxInclusive);
            float y;

            if (x > marginMask.x && x < marginMask.y)
                y = RandomBool() ? Random.Range(minInclusive, marginMask.x) : Random.Range(marginMask.y, maxInclusive);
            else
                y = Random.Range(minInclusive, maxInclusive);

            return new Vector2(x, y);
        }

        //private Vector2 GetRandomBorderPoint()
        //{
        //    var x = Random.Range(-1, 2f);
        //    var y = x > 0 && x < 1 ? Random.Range(0, 2) == 0 ? Random.Range(-1, 0f) : Random.Range(1, 2f) : Random.Range(-1, 2f);
        //    return new Vector3(x, y);
        //}
    }
}
