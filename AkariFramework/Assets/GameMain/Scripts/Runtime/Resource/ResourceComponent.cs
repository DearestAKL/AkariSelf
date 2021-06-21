using Akari.Resource;
using System;
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

        public GameObject Instantiate(string path)
        {
            return m_ResourceManager.Instantiate(path);
        }

        /// <summary>
        /// 实例化一个 prefab，底层会根据当前帧的空余时间对并行的实例化进行分帧处理，借以让 fps 更平滑，具体参考 Updater 类
        /// </summary>
        /// <param name="assetPath"></param>
        public GameObject InstantiateAsync(string path)
        {
            return m_ResourceManager.InstantiateAsync(path);
        }

        public void UpdateAssets()
        {
            m_ResourceManager.UpdateAssets();
        }
    }
}
