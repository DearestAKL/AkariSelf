using Akari.ObjectPool;
using UnityEngine;

namespace Akari
{
    class TestPoolObject : ObjectBase
    {
        public TestPoolObject()
        {

        }

        public static TestPoolObject Create(string name,string assetName)
        {
            TestPoolObject instanceObjec = new TestPoolObject();
            assetName = "TestPool";
            var go = GameEntry.Resource.Instantiate(assetName);
            instanceObjec.Initialize(name, go);

            return instanceObjec;
        }

        public override void Clear()
        {
            base.Clear();
        }

        protected internal override void Release(bool isShutdown)
        {
            GameObject.Destroy((Object)Target);
        }
    }
}
