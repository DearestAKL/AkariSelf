using System.Collections.Generic;
using UnityEngine;

namespace Akari
{
    public class ObjectPool
    {
        private readonly string m_Name;
        private Transform m_Root;

        private Queue<GameObject> m_PoolObjs = new Queue<GameObject>();
        private int m_Count;
        private int m_UsingCount;
        private int m_Capacity;

        public ObjectPool(string name,Transform parentRoot)
        {
            m_Name = name;
            SetParentRoot(parentRoot);
        }

        public ObjectPool(string name, Transform parentRoot,GameObject obj,int count)
        {
            m_Name = name;
            SetParentRoot(parentRoot);

            for (int i = 0; i < count; i++)
            {
                PushObject(GameObject.Instantiate(obj, m_Root));
            }
        }

        private void SetParentRoot(Transform parentRoot)
        {
            var go = new GameObject();
            go.name = m_Name;
            m_Root = go.transform;
            m_Root.SetParent(parentRoot);
        }

        /// <summary>
        /// 获取对象池名称。
        /// </summary>
        public string Name
        {
            get
            {
                return m_Name;
            }
        }

        /// <summary>
        /// 获取对象池中对象的数量。
        /// </summary>
        public int Count
        {
            get 
            {
                return m_Count; 
            }
        }

        /// <summary>
        /// 获取对象池中能被释放的对象的数量。
        /// </summary>
        public int CanReleaseCount
        {
            get 
            { 
                return m_UsingCount; 
            }
        }

        /// <summary>
        /// 获取或设置对象池的容量。
        /// </summary>
        public int Capacity
        {
            get { return m_Capacity; }
            set { m_Capacity = value; }
        }

        /// <summary>
        /// 释放对象池中的可释放对象。
        /// </summary>
        public void Release()
        {

        }

        /// <summary>
        /// 释放对象池中的可释放对象。
        /// </summary>
        /// <param name="toReleaseCount">尝试释放对象数量。</param>
        public void Release(int toReleaseCount)
        {

        }

        /// <summary>
        /// 从对象池中获取对象
        /// </summary>
        public GameObject GetObject()
        {
            if(m_Count > 0)
            {
                m_Count--;
                var poolObj = m_PoolObjs.Dequeue();
                poolObj.SetActive(true);
                return poolObj;
            }

            return null;
        }

        /// <summary>
        /// 归还对象到对象池
        /// </summary>
        public void PushObject(GameObject poolObj)
        {
            m_Count++;
            m_PoolObjs.Enqueue(poolObj);
            poolObj.SetActive(false);
            poolObj.transform.SetParent(m_Root);
        }
    }
}
