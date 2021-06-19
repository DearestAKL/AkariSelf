using System;
using System.Collections.Generic;
using UnityEngine;

namespace Akari
{
    public class UILoginPanel : UILoginSign
    {
        private int count = 0;

        public override void OnInit(string name, GameObject go, Transform parent, object userData)
        {
            base.OnInit(name, go, parent, userData);
        }

        public override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            btn.onClick.AddListener(TestButton);

            img.sprite = GameEntry.Resource.Load($"Assets/GameMain/Res/UI/NumberIcon/{count}.png", typeof(Sprite)).Get<Sprite>();
        }

        public void TestButton()
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
        }


    }
}
