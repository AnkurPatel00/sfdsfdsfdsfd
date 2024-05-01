using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Game.CommonModules.Pooling
{
    public class PoolManager : MonoBehaviour, IPoolManager
    {
        [SerializeField] private Transform poolParent;
        private Dictionary<string, List<PoolableObjectView>> poolableObjectsMap;
        private PoolConfigVO poolConfigVo;
        private IInstantiator instantiator;

        [Inject]
        public void Init(PoolConfigVO poolConfigVo, IInstantiator instantiator)
        {
            this.poolConfigVo = poolConfigVo;
            this.instantiator = instantiator;

            poolableObjectsMap = new Dictionary<string, List<PoolableObjectView>>();
            foreach (var poolableItemConfig in poolConfigVo.PoolableItemConfigs)
            {
                GrowPool(poolableItemConfig);
            }
        }

        public void ReturnToPool(string objectId, PoolableObjectView poolableObjectView)
        {
            if (poolParent)
            {
                poolableObjectView.transform.parent = poolParent;
                poolableObjectView.transform.position = poolParent.position;
            }

            if (poolableObjectsMap.ContainsKey(objectId))
            {
                poolableObjectsMap[objectId].Add(poolableObjectView);
            }
            else
            {
                poolableObjectsMap.Add(objectId, new List<PoolableObjectView> { poolableObjectView });
            }
        }

        public PoolableObjectView GetFromPool(string objectId)
        {
            poolableObjectsMap ??= new Dictionary<string, List<PoolableObjectView>>();

            if (poolableObjectsMap.TryGetValue(objectId, out var poolList))
            {
                if (poolList.Count > 0)
                {
                    var item = poolList[0];
                    poolableObjectsMap[objectId].RemoveAt(0);
                    item.transform.parent = null;
                    return item;
                }

                GrowPool(ObjectIdToPoolConfig(objectId));
                return GetFromPool(objectId);
            }

            return null;
        }

        private PoolableItemConfig ObjectIdToPoolConfig(string objectId)
        {
            return poolConfigVo.PoolableItemConfigs.First(x => x.Id == objectId);
        }

        private void GrowPool(PoolableItemConfig poolableItemConfig)
        {
            for (int i = 0; i < poolableItemConfig.PoolInitialSize; i++)
            {
                var instance = poolParent == null
                    ? instantiator.InstantiatePrefab(poolableItemConfig.Prefab)
                    : instantiator.InstantiatePrefab(poolableItemConfig.Prefab, poolParent);
                var view = instance.GetComponent<PoolableObjectView>();
                if (view)
                {
                    var objectId = poolableItemConfig.Id;
                    if (poolableObjectsMap.ContainsKey(objectId))
                        poolableObjectsMap[objectId].Add(view);
                    else
                        poolableObjectsMap.Add(objectId, new List<PoolableObjectView> { view });
                }
            }
        }
    }
}