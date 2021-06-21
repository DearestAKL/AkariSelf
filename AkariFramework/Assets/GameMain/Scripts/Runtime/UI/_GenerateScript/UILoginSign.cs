// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2021-06-21 09:47:18.251
//------------------------------------------------------------

using Akari.UI;

namespace Akari
{
    public class UILoginSign : UIPanel
    {
		//---UI---
		protected UnityEngine.UI.Button btn = null;
		protected UnityEngine.UI.Image img = null;
		protected UnityEngine.UI.Button BtnMinus = null;
		protected UnityEngine.UI.Button BtnAdd = null;
		protected UnityEngine.UI.Text txtNum = null;
		
		
        public override void OnInit(string name, UnityEngine.GameObject go, UnityEngine.Transform parent, object userData)
        {
            base.OnInit(name, go, parent, userData);
            InitUIData();
        }

		public void InitUIData()
        {

            ReferenceCollector rc = RootGo.GetComponent<ReferenceCollector>();
			
            btn = rc.Get<UnityEngine.UI.Button>("btn");
			img = rc.Get<UnityEngine.UI.Image>("img");
			BtnMinus = rc.Get<UnityEngine.UI.Button>("BtnMinus");
			BtnAdd = rc.Get<UnityEngine.UI.Button>("BtnAdd");
			txtNum = rc.Get<UnityEngine.UI.Text>("txtNum");
			
        }
    }
}
