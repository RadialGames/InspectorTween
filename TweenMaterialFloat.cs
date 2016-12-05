// #Generic

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
	namespace InspectorTween {
	[AddComponentMenu("InspectorTween/TweenMaterialFloat",8)]
	public class TweenMaterialFloat : TweenBase {
	
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
		protected override void LerpParameters(float lerp)
		{
			float val = initialVal;
			switch(colorFunction){
			case colorFunctions.Normal:
				val =Mathf.Lerp(valueStartAndEnd.x,valueStartAndEnd.y,lerp); break;
			case colorFunctions.Multiply:
				val *=Mathf.Lerp(valueStartAndEnd.x,valueStartAndEnd.y,lerp); break;
			case colorFunctions.Add:
				val +=Mathf.Lerp(valueStartAndEnd.x,valueStartAndEnd.y,lerp); break;
			}
				if(forceSetMaterial){
					mat.SetFloat(propID,val);
				}else	if(renderer){
				renderer.sharedMaterial.SetFloat(propID,val);
			}

		}
		protected override bool HasValidParameters()
		{
				return (mat && mat.HasProperty(propID) && interpolation.interpolation.length>0);
		}
		new void Awake()
		{
			renderer = GetComponent<Renderer>();
			base.Awake();
			propID= Shader.PropertyToID(floatName);
			var graphic = this.GetComponent<Graphic>();
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
				var rend = GetComponent<Renderer>();
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
		void OnDestroy(){
			if(!dontInstanceMaterial && mat != null)
			{
				Destroy(mat);
			}
		}
	}
}