using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Client
{
    public class CharacterInitSystem : IEcsInitSystem
    {
        private EcsCustomInject<SceneService> _sceneData;
        private EcsPoolInject<CharacterComponent> _characterPool;
        private EcsPoolInject<UnitComponent> _unitPool;
        private int _characterEntity;

        public void Init(IEcsSystems systems)
        {
            _characterEntity = systems.GetWorld().NewEntity();
            _characterPool.Value.Add(_characterEntity);
            ref var character = ref _unitPool.Value.Add(_characterEntity);
            character.View = _sceneData.Value.CharacterView;
        }
    }
}
