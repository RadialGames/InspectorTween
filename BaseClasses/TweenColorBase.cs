// #Generic

using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
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
//Compiler warns new isn't needed, but unity complains if it isn't there.
#if UNITY_EDITOR
        protected new Light light;
        protected new Camera camera;        
#else
	    protected Light light;
	    protected Camera camera;
#endif     
        
        protected Material mat;
        public Material material{set{mat = value; propID = Shader.PropertyToID(materialProperty); type = objectType.Material; } get{return mat;}}
        public enum objectType {None,Sprite,Graphic,Particle,CanvasGroup,TextMesh,Material, Light, Camera};
        protected objectType type;
        public bool useMaterial;
        public string materialProperty;
        protected int propID;
        public bool dontInstanceMaterial;
        public bool forceSetMaterial;

        protected Color _initialColor = Color.white;

        public Color initialColor {
            get { return _initialColor; }
            set { _initialColor = value; }
        }

        /// <summary>
        /// Explicitly set the target type if self configuring tween from code. Use at own risk.
        /// </summary>
        /// <param name="newType">type of object to tween</param>
        public void SetTweenTargetType(objectType newType) {
            this.type = newType;
        }

        protected void Initialize() {
           if(useMaterial && (materialProperty != null) ){
               type = objectType.Material;
               
               propID = Shader.PropertyToID(materialProperty);
               
               if ( renderer != null ) {
                   if(!dontInstanceMaterial)
                   {
                       mat =  new Material(renderer.sharedMaterial);
                       renderer.sharedMaterial = mat;
                   }
                   else{
                       mat = renderer.sharedMaterial;
                   }
                   if(!mat){
                       return;
                   }

                   if ( this is TweenColorRotation && ((TweenColorRotation) this).setMatrix ) {
                       initialColor = mat.color;
                   } else {
                       initialColor = mat.GetColor(propID);
                   }
                   
                   return;
               }else {//could be a canvas object
                   image = target.GetComponent<Graphic>();
                   if ( image != null ) {

                       if ( dontInstanceMaterial ) {
                           mat = image.materialForRendering;
                           //initialColor = mat.GetColor(propID);
                       } else {
                           mat = new Material(image.materialForRendering);
                           image.canvasRenderer.SetMaterial(mat,0);
                           image.SetMaterialDirty();
                       }
                       return;
                   }
               }
               
            } 

            sprite = target.GetComponent<SpriteRenderer>();
            if(sprite != null){
                type = objectType.Sprite;
                initialColor = sprite.color;;
                return;
            }
            image = target.GetComponent<Graphic>();//Image,Text,RawImage...
            if(image != null){
                type = objectType.Graphic;
                updateSettings.pauseOffscreen = VisibilityPause.None; //no renderer on these?
                initialColor = image.color;
                return;
            }
            light = GetComponent<Light>();
            if(light != null) {
                type = objectType.Light;
                initialColor = light.color;
                return;
            }
            psys = target.GetComponent<ParticleSystem>();
            if(psys != null){
                type = objectType.Particle;
#if UNITY_5_5_OR_NEWER
                initialColor = psys.main.startColor.color;
#else
				initialColor = psys.startColor;
#endif
                return;
            }
            canvasGroup = target.GetComponent<CanvasGroup>();
            if(canvasGroup != null){
                type = objectType.CanvasGroup;
                initialColor = new Color(1,1,1,canvasGroup.alpha);
                return;
            }
            textMesh = target.GetComponent<TextMesh>();
            if(textMesh != null){
                type = objectType.TextMesh;
                initialColor = textMesh.color;
                return;
            }

            camera = GetComponent<Camera>();
            if ( camera != null ) {
                type = objectType.Camera;
                initialColor = camera.backgroundColor;
                return;
            }
        }
        
        protected override void Awake()
        {	
            base.Awake();
            if(target == null) {
                target = this.gameObject;
            }
            if ( renderer == null ) {
                renderer = target.GetComponent<Renderer>();
            }
            Initialize();
        } 
        
        public void SetRenderer(Renderer partRenderer) {
            this.renderer = partRenderer;
        }
        
        public void SetInitialMaterialColor(){
            if(mat && mat.HasProperty(propID)){
                initialColor = mat.GetColor(propID);
            }
        }
        
        /// <summary>
        /// Explicitly input initial colour
        /// </summary>
        /// <param name="initial"></param>
        public void SetInitialColor(Color initial){
            initialColor = initial;
        }
        
        /// <summary>
        /// Set the initial value for blending to whatever the current state of target
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void SetInitialColorFromCurrentState() {
            switch ( type ) {
                case objectType.None:
                    break;
                case objectType.Sprite:
                    if ( sprite != null ) {
                        initialColor = sprite.color;
                    }
                    break;
                case objectType.Graphic:
                    if ( image != null ) {
                        initialColor = image.color;
                    }
                    break;
                case objectType.Particle:
                    if ( psys != null ) {
#if UNITY_5_5_OR_NEWER
                        initialColor = psys.main.startColor.color;
#else
				        initialColor = psys.startColor;
#endif
                    }
                    break;
                case objectType.CanvasGroup:
                    if ( canvasGroup != null ) {
                        initialColor = new Color(1,1,1,canvasGroup.alpha); //Canvas group basically only sets alpha
                    }
                    break;
                case objectType.TextMesh:
                    if ( textMesh != null ) {
                        initialColor = textMesh.color;
                    }
                    break;
                case objectType.Material:
                    if ( mat != null && mat.HasProperty(propID) ) {
                        initialColor = mat.GetColor(propID);
                    }
                    break;
                case objectType.Light:
                    if ( light != null ) {
                        initialColor = light.color;
                    }
                    break;
                case objectType.Camera:
                    if ( camera != null ) {
                        initialColor = camera.backgroundColor;
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        protected override bool HasValidParameters()
        {
            return ((type != objectType.None));
        }

        protected abstract Color LerpColor(float lerp,Color initial);
        protected override void LerpParameters(float lerp)
        {
            Color val = LerpColor(lerp,initialColor);
            switch(type){
                case objectType.Material :
                    if(forceSetMaterial){
                        mat.SetColor(propID,val);
                    } else {
                        if ( image != null ) {
                            var iMat = image.material;
                            iMat.SetColor(propID,val);
                            image.SetMaterialDirty();
                            break;
                        }
                        renderer.sharedMaterial.SetColor(propID,val); 
                    }
                    break;
                case objectType.Graphic : 
                    image.color = val; 
                    break;
                case objectType.Sprite : 
                    sprite.color = val; 
                    break;
                case objectType.Particle :
#if UNITY_5_5_OR_NEWER
                    ParticleSystem.MinMaxGradient pMain = psys.main.startColor;
                    pMain.color = val;
#else
	    			psys.startColor = val;
#endif
                    break;
                case objectType.CanvasGroup: 
                    canvasGroup.alpha = val.a;
                    break;
                case objectType.TextMesh : 
                    textMesh.color = val; 
                    break;
                case objectType.Light: 
                    light.color = val;
                    break;
                case objectType.Camera:
                    camera.backgroundColor = val;
                    break;
            }
        }
        public override void SetToLerpPoint(float lerp){
            this.LerpParameters(lerp);
        }
        protected void OnDestroy() {
            if (!dontInstanceMaterial && mat != null) {
                Destroy(mat);
            }
        }
    }

}