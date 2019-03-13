// #Generic

using UnityEngine;
using UnityEngine.UI;

	namespace InspectorTween {
	[AddComponentMenu("InspectorTween/TweenMaterialFloat",8)]
	[HelpURL("https://github.com/RadialGames/InspectorTween/wiki/TweenMaterialFloat")]
	public class TweenMaterialFloat : TweenBase {
		public Transform target;
		public string floatName = "_Multiply"; 
		public Vector2 valueStartAndEnd = new Vector2(0,1);
		//public AnimationCurve valueOverTime = new AnimationCurve(new Keyframe[]{new Keyframe(0,0),new Keyframe(1,1)});
		private Material mat;
		public Material material{set{mat = value; propID = Shader.PropertyToID(floatName); } get{return mat;}}
		public bool dontInstanceMaterial;
		private int propID;
		private float initialVal;
		public enum colorFunctions{Normal,Add,Multiply};
		public colorFunctions colorFunction;
		public bool forceSetMaterial;
		
		protected override void LerpParameters(float lerp) {
			float val = initialVal;
			float startVal = timeSettings.reverseValues ? valueStartAndEnd.y : valueStartAndEnd.x;
			float endVal = timeSettings.reverseValues ? valueStartAndEnd.x : valueStartAndEnd.y;
			switch(colorFunction){
			case colorFunctions.Normal:
				val =Mathf.Lerp(startVal,endVal,lerp); break;
			case colorFunctions.Multiply:
				val *=Mathf.Lerp(startVal,endVal,lerp); break;
			case colorFunctions.Add:
				val +=Mathf.Lerp(startVal,endVal,lerp); break;
			}
			if(forceSetMaterial){
				mat.SetFloat(propID,val);
			}else	if(renderer && renderer.sharedMaterial) {
				renderer.sharedMaterial.SetFloat(propID,val);
			}

		}
		protected override bool HasValidParameters()
		{
				return (mat && mat.HasProperty(propID) && interpolationCurve.length>0);
		}
		protected override void Awake()
		{
			if (target == null) {
				target = this.transform;
			}
			renderer = target.GetComponent<Renderer>();
			base.Awake();
			propID= Shader.PropertyToID(floatName);
			var graphic = target.GetComponent<Graphic>();
			if(graphic){
				if(!dontInstanceMaterial)
				{
					mat = new Material(graphic.material);
					graphic.material = mat;
					initialVal = mat.GetFloat(propID);
				}
				else{
					mat = graphic.materialForRendering;
				}
			}
			else{
				Renderer rend = renderer;
				if (!rend) {
						return;
				}
				if(!dontInstanceMaterial)
				{
					mat =  new Material(rend.sharedMaterial);
					rend.material = mat;
				}
				else{
					mat = rend.sharedMaterial;
				}
			}
		
		}

		private void OnDestroy(){
			if(!dontInstanceMaterial && mat != null)
			{
				Destroy(mat);
			}
		}
	}
}