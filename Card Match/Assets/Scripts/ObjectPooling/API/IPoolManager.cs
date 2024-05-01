using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.CommonModules.Pooling
{
    public interface IPoolManager
    {
        /// <summary>
        /// Adds the given object to the pool map
        /// </summary>
        /// <param name="objectId">unique object id</param>
        /// <param name="poolableObjectView">poolable object</param>
        void ReturnToPool(string objectId,PoolableObjectView poolableObjectView);

        /// <summary>
        /// Returns a poolable object from the map if the key is valid
        /// </summary>
        /// <param name="objectId">unique object id</param>
        /// <returns>poolable object</returns>
        PoolableObjectView GetFromPool(string objectId);
    }

    [Serializable]
    public class PoolConfigVO
    {
        public List<PoolableItemConfig> PoolableItemConfigs;
    }

    [Serializable]
    public struct PoolableItemConfig
    {
        public string Id;
        public GameObject Prefab;
        public int PoolInitialSize;
    }
}