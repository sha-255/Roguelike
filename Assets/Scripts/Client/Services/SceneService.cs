using UnityEngine;
using UnityEngine.Pool;

namespace Client
{
    public class SceneService : MonoBehaviour
    {
        [field: SerializeField] public UnitView CharacterView { get; private set; }
        [field: SerializeField] public BulletView BulletViewPrefab { get; private set; }

        public Camera MainCamera => Camera.main;

        public PlayerInputActions InputActions
        { get; private set; }

        private ObjectPool<BulletView> _bulletsPool;

        private void Awake()
        {
            InputActions = new();
            //TODO: normal pool
            _bulletsPool = new ObjectPool<BulletView>(() => Instantiate(BulletViewPrefab));
        }

        public BulletView GetEnemy()
        {
            var view = _bulletsPool.Get();
            view.gameObject.SetActive(true);
            return view;
        }

        public void ReleaseEnemy(BulletView view)
        {
            view.gameObject.SetActive(false);
            _bulletsPool.Release(view);
        }
    }
}
