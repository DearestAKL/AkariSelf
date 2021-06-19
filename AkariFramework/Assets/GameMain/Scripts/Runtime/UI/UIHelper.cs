using System;
using System.Collections.Generic;
using UnityEngine;
using VEngine;

namespace Akari
{
    public class UIHelper : IUIHelper
    {
        private readonly Dictionary<string, Type> uiTypeMap = new Dictionary<string, Type>();
        private readonly Camera uiCamera;

        public UIHelper(Camera camera)
        {
            uiCamera = camera;
            InitUITypeMap();
        }

        public Camera UICamera
        {
            get { return uiCamera; }
        }

        public void InitUITypeMap()
        {
            uiTypeMap.Add(UIType.UILogin,typeof(UILoginPanel));
        }

        public GameObject LoadUIAsset(string name)
        {
            var asset =GameEntry.Resource.Load(name, typeof(GameObject));

            return GameObject.Instantiate(asset.Get<GameObject>());
        }

        //public GameObject LoadAsynUIAsset(string path, Action<Asset> completed = null)
        //{
        //    var asset = GameEntry.Resource.LoadAsync(path, typeof(GameObject), completed);
        //    return asset.Get<GameObject>();
        //}

        public Type GetUIPanelType(string name)
        {
            uiTypeMap.TryGetValue(name, out Type type);
            return type != null ? type : null;
        }
    }
}
