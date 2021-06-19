using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Akari
{
    public class BaseComponent : GameFrameworkComponent
    {
        protected override void Awake()
        {
            base.Awake();
        }

        private void Start()
        {
            
        }

        private void Update()
        {
            GameFrameworkEntry.Update(Time.deltaTime, Time.unscaledDeltaTime);
        }

        public void Shutdown()
        {
            Destroy(gameObject);
        }
    }
}
