// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2021-06-19 16:47:55.081
//------------------------------------------------------------


namespace Akari
{
    public class UILoginSign : UIPanel
    {
		//---UI---
		protected UnityEngine.UI.Button btn = null;
		protected UnityEngine.UI.Image img = null;

        public override void OnInit(string name, UnityEngine.GameObject go, UnityEngine.Transform parent, object userData)
        {
            base.OnInit(name, go, parent, userData);
            InitUIData();
        }

        public void InitUIData()
        {

            ReferenceCollector rc = RootGo.GetComponent<ReferenceCollector>();

            btn = rc.Get<UnityEngine.UI.Button>("Button");
            img = rc.Get<UnityEngine.UI.Image>("Image");
			
        }
    }
}
