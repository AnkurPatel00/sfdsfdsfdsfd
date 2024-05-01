using UnityEngine;
using Zenject;

namespace Game.CommonModules.Pooling
{
    public class PoolableObjectView : MonoBehaviour
    {
        [SerializeField] private string objectId;
        
        private IPoolManager poolManager;

        [Inject]
        private void Init(IPoolManager poolManager)
        {
            this.poolManager = poolManager;
        }
        
        public virtual void SendToPool()
        {
            poolManager.ReturnToPool(objectId,this);
        }
    }
}