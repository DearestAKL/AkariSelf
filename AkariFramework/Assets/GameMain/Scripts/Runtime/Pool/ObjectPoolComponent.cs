using Akari.ObjectPool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Akari
{
    public class ObjectPoolComponent : GameFrameworkComponent
    {
        private IObjectPoolManager m_ObjectPoolManager;

        protected override void Awake()
        {
            base.Awake();

            m_ObjectPoolManager = GameFrameworkEntry.GetModule<ObjectPoolManager>();
        }

        /// <summary>
        /// 检查是否存在对象池。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <returns>是否存在对象池。</returns>
        public bool HasObjectPool<T>() where T : ObjectBase
        {
            return m_ObjectPoolManager.HasObjectPool<T>();
        }

        /// <summary>
        /// 获取对象池。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <returns>要获取的对象池。</returns>
        public IObjectPool<T> GetObjectPool<T>() where T : ObjectBase
        {
            return m_ObjectPoolManager.GetObjectPool<T>();
        }

        /// <summary>
        /// 创建允许单次获取的对象池。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <param name="name">对象池名称。</param>
        /// <returns>要创建的允许单次获取的对象池。</returns>
        public IObjectPool<T> CreateSingleSpawnObjectPool<T>() where T : ObjectBase
        {
            return m_ObjectPoolManager.CreateSingleSpawnObjectPool<T>();
        }

        /// <summary>
        /// 创建允许多次获取的对象池。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <returns>要创建的允许多次获取的对象池。</returns>
        public IObjectPool<T> CreateMultiSpawnObjectPool<T>() where T : ObjectBase
        {
            return m_ObjectPoolManager.CreateMultiSpawnObjectPool<T>();
        }

        /// <summary>
        /// 销毁对象池。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <returns>是否销毁对象池成功。</returns>
        public bool DestroyObjectPool<T>() where T : ObjectBase
        {
            return m_ObjectPoolManager.DestroyObjectPool<T>();
        }

        /// <summary>
        /// 释放对象池中的可释放对象。
        /// </summary>
        public void Release()
        {
            m_ObjectPoolManager.Release();
        }

        /// <summary>
        /// 释放对象池中的所有未使用对象。
        /// </summary>
        public void ReleaseAllUnused()
        {
            m_ObjectPoolManager.ReleaseAllUnused();
        }
    }
    
}
