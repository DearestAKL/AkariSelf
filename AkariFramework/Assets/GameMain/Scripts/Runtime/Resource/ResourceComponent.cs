﻿using System;
using UnityEngine;
using VEngine;

namespace Akari
{
    public class ResourceComponent : GameFrameworkComponent
    {
        private ResourceManager m_ResourceManager;

        protected override void Awake()
        {
            base.Awake();
            m_ResourceManager = GameFrameworkEntry.GetModule<ResourceManager>();
        }

        /// <summary>
        /// 同步加载资源，此接口不支持直接从服务器下载资源。
        /// </summary>
        /// <param name="path">资源路径，以 “Assets” 开头</param>
        /// <param name="type">资源类型</param>
        /// <returns></returns>
        public Asset Load(string path, Type type)
        {
            return m_ResourceManager.Load(path, type);
        }

        /// <summary>
        /// 同步加载资源，此接口不支持直接从服务器下载资源。
        /// </summary>
        /// <param name="path">资源路径，以 “Assets” 开头</param>
        /// <param name="type">资源类型</param>
        /// <returns></returns>
        public Asset LoadAsync(string path, Type type, Action<Asset> completed = null)
        {
            return m_ResourceManager.LoadAsync(path, type, completed);
        }

        public GameObject InstantiateAsync(string name)
        {
            return m_ResourceManager.InstantiateAsync(name);
        }

        public void UpdateAssets()
        {
            m_ResourceManager.UpdateAssets();
        }
    }
}
