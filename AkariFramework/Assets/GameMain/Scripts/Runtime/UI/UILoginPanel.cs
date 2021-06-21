using Akari.ObjectPool;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Akari
{
    public class UILoginPanel : UILoginSign
    {
        private int count = 0;
        private List<TestPoolObject> poolObjects = new List<TestPoolObject>();

        private IObjectPool<TestPoolObject> m_InstancePool;

        public override void OnInit(string name, GameObject go, Transform parent, object userData)
        {
            base.OnInit(name, go, parent, userData);
        }

        public override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            btn.onClick.AddListener(TestButton);

            img.sprite = GameEntry.Resource.Load($"Assets/GameMain/Res/UI/NumberIcon/{count}.png", typeof(Sprite)).Get<Sprite>();

            BtnAdd.onClick.AddListener(AddTest);
            BtnMinus.onClick.AddListener(MinusTest);

            m_InstancePool = GameEntry.ObjectPool.CreateMultiSpawnObjectPool<TestPoolObject>();
            m_InstancePool.AutoReleaseInterval = 10;
        }

        private void TestButton()
        {
            if (count == 9)
            {
                count = 0;
            }
            else
            {
                count++;
            }
            Debug.Log($"点击成功,Count={count}");

            img.sprite = GameEntry.Resource.Load($"Assets/GameMain/Res/UI/NumberIcon/{count}.png", typeof(Sprite)).Get<Sprite>();
            m_InstancePool.Register(TestPoolObject.Create("Test", "assetName"), false);
            Debug.Log("创建");
            Debug.Log($"对象数量{m_InstancePool.Count},可释放对象数量{m_InstancePool.CanReleaseCount}");

        }

        private void AddTest()
        {
            Debug.Log("Add");

            poolObjects.Add(m_InstancePool.Spawn("Test"));
            Debug.Log("获取");
            Debug.Log($"对象数量{m_InstancePool.Count},可释放对象数量{m_InstancePool.CanReleaseCount}");

        }

        private void MinusTest()
        {
            Debug.Log("Minus");

            if (poolObjects.Count > 0) 
            {
                m_InstancePool.Unspawn(poolObjects[0]);
                poolObjects.RemoveAt(0);

            }
            Debug.Log($"对象数量：{m_InstancePool.Count},可释放对象数量：{m_InstancePool.CanReleaseCount}");
        }
    }
}
