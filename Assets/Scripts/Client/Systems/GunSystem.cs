using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client
{
    public class GunSystem : IEcsInitSystem
    {
        private EcsFilterInject<Inc<UnitComponent>> _unitFilter;
        private EcsPoolInject<GunComponent> _gunPool;
        private EcsPoolInject<SpawnIntervalComponent> _spawnInterval;

        public void Init(IEcsSystems systems)
        {
            foreach (var unitEntity in _unitFilter.Value)
            {
                ref var gun = ref _gunPool.Value.Add(unitEntity);
                ref var unit = ref _unitFilter.Pools.Inc1.Get(unitEntity);
                gun.View = unit.View.GunView;
            }
        }
    }
}
