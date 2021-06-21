using System;
using UnityEngine;
using VEngine;

namespace Akari.Resource
{
    /// <summary>
    /// 资源管理器(基于XAsset)
    /// </summary>
    public class ResourceManager : GameFrameworkModule
    {

        public override int Priority
        {
            get
            {
                return 1;
            }
        }

        public override void Shutdown()
        {

        }

        public override void Update(float elapseSeconds, float realElapseSeconds)
        {

        }

        /// <summary>
        /// 同步加载资源，此接口不支持直接从服务器下载资源。
        /// </summary>
        /// <param name="path">资源路径，以 “Assets” 开头</param>
        /// <param name="type">资源类型</param>
        /// <returns></returns>
        public Asset Load(string path, Type type)
        {
            return Asset.Load(path, type);
        }

        /// <summary>
        /// 同步加载资源，此接口不支持直接从服务器下载资源。
        /// </summary>
        /// <param name="path">资源路径，以 “Assets” 开头</param>
        /// <param name="type">资源类型</param>
        /// <returns></returns>
        public Asset LoadAsync(string path, Type type, Action<Asset> completed = null)
        {
            return Asset.LoadAsync(path, type, completed);
        }

        public GameObject Instantiate(string path)
        {
            var asset = Asset.Load(path, typeof(GameObject));
            return GameObject.Instantiate(asset.asset as GameObject);
        }

        /// <summary>
        /// 实例化一个 prefab，底层会根据当前帧的空余时间对并行的实例化进行分帧处理，借以让 fps 更平滑，具体参考 Updater 类
        /// </summary>
        /// <param name="assetPath"></param>
        public GameObject InstantiateAsync(string path)
        {
            return InstantiateObject.InstantiateAsync(path).result;
        }

        public void UpdateAssets()
        {
            Asset.UpdateAssets();
        }
    }
}
