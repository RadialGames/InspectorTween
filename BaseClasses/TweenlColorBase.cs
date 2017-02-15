// #Generic

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
namespace InspectorTween{
[System.Serializable]
public class TweenColorParameters
{
	public Gradient colorOverTime;
	public enum colorFunctions{Normal,Add,Multiply,Overlay};
	public colorFunctions colorFunction;

}
	
public abstract class TweenColorBase : TweenBase {
		public GameObject target;
	protected SpriteRenderer sprite;
	protected Graphic image;
	protected ParticleSystem psys;
	protected CanvasGroup canvasGroup;
	protected TextMesh textMesh;
		protected Material mat;
		public Material material{set{mat = value; propID = Shader.PropertyToID(materialProperty); type = objectType.Material; } get{return mat;}}
	public enum objectType {None,Sprite,Graphic,Particle,CanvasGroup,TextMesh,Material};
	protected objectType type;
		public bool useMaterial;
		public string materialProperty;
		protected int propID;
		public bool dontInstanceMaterial;
		public bool forceSetMaterial;
	[HideInInspector]public Color initialColor = Color.white;

		public void SetTweenTargetType(objectType newType) {
			this.type = newType;
		}
		new void Awake()
		{	
			base.Awake();
			if(target == null) {
				target = this.gameObject;
			}
			renderer = target.GetComponent<Renderer>();
			if(useMaterial && (materialProperty != null) && renderer){
				type = objectType.Material;
				propID = Shader.PropertyToID(materialProperty);
				if(!dontInstanceMaterial)
				{
					mat =  new Material(renderer.sharedMaterial);
					renderer.material = mat;
				}
				else{
					mat = renderer.sharedMaterial;
				}
				if(!mat){
					return;
				}
				initialColor = mat.GetColor(propID);
				return;
			}

			sprite = target.GetComponent<SpriteRenderer>();
			if(sprite){
				type = objectType.Sprite;
				initialColor = sprite.color;;
				return;
			}
			image = target.GetComponent<Graphic>();//Image,Text,RawImage...
			if(image){
				type = objectType.Graphic;
				this.updateSettings.pauseOffscreen = VisibilityPause.None; //no renderer on these?
				initialColor = image.color;
				return;
			}
			psys = target.GetComponent<ParticleSystem>();
			if(psys){
				type = objectType.Particle;
#if UNITY_5_6_OR_NEWER
				initialColor = psys.main.startColor.color;
#else
				initialColor = psys.startColor;
#endif
				return;
			}
			canvasGroup = target.GetComponent<CanvasGroup>();
			if(canvasGroup){
				type = objectType.CanvasGroup;
				initialColor = Color.white;
				initialColor.a = canvasGroup.alpha;
				return;
			}
			textMesh = target.GetComponent<TextMesh>();
			if(textMesh){
				type = objectType.TextMesh;
				initialColor = textMesh.color;
				return;
			}
		} 
		public void SetRenderer(Renderer partRenderer) {
			this.renderer = partRenderer;
		}
	public void SetInitialMaterialColor(){
		if(mat && mat.HasProperty(propID)){
			initialColor = mat.GetColor(propID);
		}
	}
	public void SetInitialColor(Color initial){
		initialColor = initial;
	}

	protected override bool HasValidParameters()
	{
		return ((type != objectType.None));
	}

		protected abstract Color LerpColor(float lerp);
		protected override void LerpParameters(float lerp)
		{
			var val = LerpColor(lerp);
			switch(type){
				case objectType.Material :
					if(forceSetMaterial){
						mat.SetColor(propID,val);
					}else{
						renderer.sharedMaterial.SetColor(propID,val); 
					}
					break;
				case objectType.Graphic : image.color = val; break;
				case objectType.Sprite : sprite.color = val; break;
				case objectType.Particle :
#if UNITY_5_6_OR_NEWER
					var pMain = psys.main.startColor;
					pMain.color = val;
#else
				psys.startColor = val;
#endif

					break;
				case objectType.CanvasGroup: canvasGroup.alpha = val.a;break;
				case objectType.TextMesh : textMesh.color = val; break;
			}
		}
		public override void SetToLerpPoint(float lerp){
			this.LerpParameters(lerp);
		}
		void OnDestroy() {
			if (!dontInstanceMaterial && mat != null) {
				Destroy(mat);
			}
		}
	}

}