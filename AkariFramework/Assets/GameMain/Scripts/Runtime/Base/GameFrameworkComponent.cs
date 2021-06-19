﻿using UnityEngine;

namespace Akari
{
    /// <summary>
    /// 游戏框架组件抽象类。
    /// </summary>
    public abstract class GameFrameworkComponent : MonoBehaviour
    {
        /// <summary>
        /// 游戏框架组件初始化。
        /// </summary>
        protected virtual void Awake()
        {
            GameFrameworkComponentEntry.RegisterComponent(this);
        }
    }
}
