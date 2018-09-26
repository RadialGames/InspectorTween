//#Generic
/** Usefull as an event function so enable or disable almost anything through animation events.
*/ 
using UnityEngine;
using System.Collections;
namespace InspectorTween{
public class EnableComponent : MonoBehaviour {
	public enum ActivateState {None,Awake,OnEnable,Start
	}

	public ActivateState activeOn;
	public Component[] component;
	//particle system variables
	[Header("Particle System Settings")]
	[Tooltip("Check if particle systems use Distance Emission")]
	public bool isDistanceEmission;
	private float[] eRate;

	void Awake()
	{
		if(component.Length > 0){
			int count = 0;
			foreach( var obj in component){
				if(obj.GetType().IsSubclassOf(typeof(ParticleSystem))){
					count ++;
				}
			}
			eRate = new float[component.Length];
			for(var i = 0; i < this.component.Length;i++)
			{
				var obj = component[i];
				if(obj.GetType().IsSubclassOf(typeof(ParticleSystem))){
					ParticleSystem ps = (ParticleSystem)obj;
#if UNITY_5_5_OR_NEWER
						eRate[i] = ps.emission.rateOverDistance.constantMax;
#else
				eRate[i] = ps.emission.rate.constantMax;
#endif

					}
				}
		}

		if ( activeOn == ActivateState.Awake ) {
			EnableAll();
		}
	}

	void OnEnable() {
		if ( activeOn == ActivateState.OnEnable ) {
			EnableAll();
		}
	}

	void Start() {
		if ( activeOn == ActivateState.Start ) {
			EnableAll();
		}
	}
	
	
	private bool GetComponentState(Component obj){
		if(obj){
			System.Type oType = obj.GetType();
			if(oType.IsSubclassOf(typeof(MonoBehaviour))){
				return ((MonoBehaviour)obj).enabled;
			}else 
			if(oType.IsSubclassOf(typeof(ParticleSystem))){ //particle System is a renderer, but we want some special cases
				ParticleSystem ps = (ParticleSystem)obj;
				if(!this.isDistanceEmission){
					return ps.isPlaying && ps.IsAlive(true);
				}
				else{
#if UNITY_5_5_OR_NEWER
						return ps.emission.rateOverDistance.constantMax > 0f;
#else
						return ps.emission.rate.constantMax > 0f;
#endif

					}
				} else 
			if(oType.IsSubclassOf(typeof(Renderer))){//handles trail renderers, etc.
				return ((Renderer)obj).enabled;
			} else
				if(oType.IsSubclassOf(typeof(Animator))){
				var amtr = (Animator)obj;
				return amtr.enabled;
			} else 
			if(oType.IsSubclassOf(typeof(Transform)) || (oType.ToString() == "UnityEngine.Transform")){
				//in the case of getting a transform, SetActive on gameobject
				GameObject gob = ((Transform)obj).gameObject;
				return gob.activeSelf;
			}
			else if (oType.IsSubclassOf(typeof(Collider))){
				return ((Collider)obj).enabled;
			}
		}
		return false;
	}
	private void SetComponentState(Component obj,bool state)
	{
		if(obj){
			System.Type oType = obj.GetType();
			if(oType.IsSubclassOf(typeof(MonoBehaviour))){
				((MonoBehaviour)obj).enabled = state;
			}else 
			if(oType.IsSubclassOf(typeof(ParticleSystem))){ //particle System is a renderer, but we want some special cases
				ParticleSystem ps = (ParticleSystem)obj;
				if(!this.isDistanceEmission){
					if(state){
						ps.Play();
					}else{
						ps.Stop();
					}
				}
				else{
					if(state){
						int _i = -1;
						for(int i=0;i<component.Length;i++){
							if(component[i] == obj){
								_i = i;
								break;
							}
						}
						if(_i>=0){//this should always be valid.
							var emm = ps.emission;
#if UNITY_5_5_OR_NEWER
								emm.rateOverDistance = new ParticleSystem.MinMaxCurve(this.eRate[_i]);
#else
						emm.rate = new ParticleSystem.MinMaxCurve(this.eRate[_i]);
#endif

								ps.Simulate(0.5f,false,true);
							ps.Play(false);
						}
					}
					else{
						var emm = ps.emission;
#if UNITY_5_5_OR_NEWER
							emm.rateOverDistance = new ParticleSystem.MinMaxCurve(0f);
#else
							emm.rate = new ParticleSystem.MinMaxCurve( 0f);
#endif

						}
					}
			}else 
			if(oType.IsSubclassOf(typeof(Renderer))){//handles trail renderers, etc.
				((Renderer)obj).enabled = state;
			} else
			if(oType.IsSubclassOf(typeof(Animator))){
				var amtr = (Animator)obj;
				amtr.enabled = state;
			}
			else if(oType.IsSubclassOf(typeof(Transform)) || (oType.ToString() == "UnityEngine.Transform") ){
				//in the case of getting a transform, SetActive on gameobject
				GameObject gob = ((Transform)obj).gameObject;
				gob.SetActive(state);
			}
			else if (oType.IsSubclassOf(typeof(Collider))){
				((Collider)obj).enabled = state;
			}
			else{
				Debug.Log ("No support for " + oType.ToString() + " ask a coder, or email script authour (lindsay@radialgames.com)");
			}
		}
	}
	private bool IsValidIndex(int ind){
		return ind >= 0 && ind < this.component.Length && this.component[ind];
	}
	public void Enable()
	{
		Enable(0);
	}
	public void Enable(int ind){
		if(IsValidIndex(ind)){
			SetComponentState(component[ind],true);
		}
	}
	public void EnableAll(){
		foreach(var obj in component){
			SetComponentState(obj,true);
		}
	}
	public void Disable()
	{
		Disable(0);
	}
	public void Disable(int ind){
		if(IsValidIndex(ind)){
			SetComponentState(component[ind],false);
		}
	}
	public void DisableAll(){
		foreach(var obj in component){
			SetComponentState(obj,false);
		}
	}

	public void Toggle()
	{
		Toggle(0); //default toggle item 0;
	}
	public void Toggle(int ind) 
	{
		if(IsValidIndex(ind)){
			var obj = this.component[ind];
			SetComponentState(obj,!GetComponentState(obj));
		}
	}

	
}
}