// #Generic
/**
* Base for Radial Games Tween scripts.
*/
using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System;
namespace InspectorTween{
	//RTEase by MattRix : https://gist.github.com/MattRix/feea68fd3dae16c760d6c665fd530d46
	public static class RTEase
	{
		//public static Func<float, float> Linear = (t) => { return t; };
		public static Func<float, float> Instant = (t) => { return t < 1f ? 0f : 1f; };
		public static Func<float, float> QuadIn = (t) => { return t * t; };
		public static Func<float, float> QuadOut = (t) => { return 2f * t - t * t; };
		public static Func<float, float> QuadInOut = (t) => { return (t <= 0.5f) ? (t * t * 2f) : (-1.0f + 4f * t + -2f * t * t); };
		public static Func<float, float> CubeIn = (t) => { return t * t * t; };
		public static Func<float, float> CubeOut = (t) => { return 1f - CubeIn(1f - t); };
		public static Func<float, float> CubeInOut = (t) => { return (t <= 0.5f) ? CubeIn(t * 2f) * 0.5f : CubeOut(t * 2f - 1f) * 0.5f + 0.5f; };
		public static Func<float, float> BackIn = (t) => { return t * t * (2.70158f * t - 1.70158f); };
		public static Func<float, float> BackOut = (t) => { return 1f - BackIn(1f - t); };
		public static Func<float, float> BackInOut = (t) => { return (t <= 0.5f) ? BackIn(t * 2f) * 0.5f : BackOut(t * 2f - 1f) * 0.5f + 0.5f; };
		public static Func<float, float> ExpoIn = (t) => { return Mathf.Pow(2f, 10f * (t - 1.0f)); };
		public static Func<float, float> ExpoOut = (t) => { return 1f - Mathf.Pow(2f, -10f * t); };
		public static Func<float, float> ExpoInOut = (t) => { return t < .5f ? ExpoIn(t * 2f) * 0.5f : ExpoOut(t * 2f - 1f) * 0.5f + 0.5f; };
		public static Func<float, float> SineIn = (t) => { return -Mathf.Cos(Mathf.PI * 0.5f * t) + 1f; };
		public static Func<float, float> SineOut = (t) => { return Mathf.Sin(Mathf.PI * 0.5f * t); };
		public static Func<float, float> SineInOut = (t) => { return -Mathf.Cos(Mathf.PI * t) * 0.5f + .5f; };
		public static Func<float, float> ElasticIn = (t) => { return 1f - ElasticOut(1f - t); };
		public static Func<float, float> ElasticOut = (t) => { return Mathf.Pow(2f, -10f * t) * Mathf.Sin((t - 0.075f) * (2f * Mathf.PI) / 0.3f) + 1f; };
		public static Func<float, float> ElasticInOut = (t) => { return (t <= 0.5f) ? ElasticIn(t * 2f)  : ElasticOut(t * 2f - 1f) ; };
	}

	public static class ProgramaticInterpolation
	{
		public enum TweenTypes { Linear, Step, Quadratic, Cubic, Back, Exponential, Sine, Elastic };
		public enum TweenLoopMode {Loop,PingPong,Continuous,Clamp }
		public delegate float InterpolationFunc(float val);//pass interpolation type function as argument.
		const float TAU = Mathf.PI * 2;
		private static float Frac(float val){
			return val % 1f;// val - Mathf.FloorToInt(val);
		}
		private static float Linear(float val){
			return (val);
		}
		private static float Step(float val) {
			return RTEase.Instant(val);
		}

		private static float Quadratic(float val) {
			return RTEase.QuadInOut(val);
		}
		private static float QuadraticIn(float val) {
			return RTEase.QuadIn(val);
		}
		private static float QuadraticOut(float val) {
			return RTEase.QuadOut(val);
		}

		private static float Cubic(float val) {
			return RTEase.CubeInOut(val);
		}
		private static float CubicIn(float val) {
			return RTEase.CubeIn(val);
		}
		private static float CubicOut(float val) {
			return RTEase.CubeOut(val);
		}

		private static float Back(float val) {
			return RTEase.BackInOut(val);
		}
		private static float BackIn(float val) {
			return RTEase.BackIn(val);
		}
		private static float BackOut(float val) {
			return RTEase.BackOut(val);
		}

		private static float Exponential(float val) {
			return RTEase.ExpoInOut(val);
		}
		private static float ExponentialIn(float val) {
			return RTEase.ExpoIn(val);
		}
		private static float ExponentialOut(float val) {
			return RTEase.ExpoOut(val);
		}

		private static float Sine(float val){
			return RTEase.SineInOut(val);
		}
		private static float SineIn(float val) {
			return RTEase.SineIn(val);
		}
		private static float SineOut(float val) {
			return RTEase.SineOut(val);
		}

		private static float Elastic(float val) {
			return RTEase.ElasticInOut(val);
		}
		private static float ElasticIn(float val) {
			return RTEase.ElasticIn(val);
		}
		private static float ElasticOut(float val) {
			return RTEase.ElasticOut(val);
		}

		/**
		 * Get an interpolation function from enum
		 */
		public static InterpolationFunc GetInterpolator(TweenTypes type,bool inOnly = false, bool outOnly = false) {
			InterpolationFunc rFunc = ProgramaticInterpolation.Linear;
			switch (type) {
				case TweenTypes.Linear:
					rFunc = Linear;
					break;
				case TweenTypes.Step:
					rFunc = Step;
					break;
				case TweenTypes.Quadratic:
					rFunc = Quadratic;
					if (inOnly) {
						rFunc = QuadraticIn;
					}
					else if (outOnly) {
						rFunc = QuadraticOut;
					}
					break;
				case TweenTypes.Cubic:
					rFunc = Cubic;
					if (inOnly) {
						rFunc = CubicIn;
					} else if (outOnly) {
						rFunc = CubicOut;
					}
					break;
				case TweenTypes.Back:
					rFunc = Back;
					if (inOnly) {
						rFunc = BackIn;
					} else if (outOnly) {
						rFunc = BackOut;
					}
					break;
				case TweenTypes.Exponential:
					rFunc = Exponential;
					if (inOnly) {
						rFunc = ExponentialIn;
					} else if (outOnly) {
						rFunc = ExponentialOut;
					}
					break;
				case TweenTypes.Sine:
					rFunc = Sine;
					if (inOnly) {
						rFunc = SineIn;
					} else if (outOnly) {
						rFunc = SineOut;
					}
					break;
				case TweenTypes.Elastic:
					rFunc = Elastic;
					if (inOnly) {
						rFunc = ElasticIn;
					} else if (outOnly) {
						rFunc = ElasticOut;
					}
					break;
			}
			return rFunc;
		}

		/**
		 *Get an interpolationblended between two specified functions.
		*/
		private static float Interpolate(InterpolationFunc inF, InterpolationFunc outF,float val,TweenLoopMode mode = TweenLoopMode.Loop) {
			switch (mode) {
				case TweenLoopMode.Loop:
					val = Frac(val);
					break;
				case TweenLoopMode.PingPong:
					val = (val % 2) > 1f ? 1f-Frac(val) : Frac(val);
					break;
				case TweenLoopMode.Continuous:
					//val = val; Do nothing
					break;
				case TweenLoopMode.Clamp:
					val = Mathf.Clamp01(val);
					break;
			}
			//val = Frac(val);
			if (inF == outF) { //if the in and out are the same, then just do the one withoug interpolating them.
				return inF(val);
			}
			if (val == 0f) {//Only Do one if at 0
				return inF(val);
			}
			if (val == 1f) {//Only do one interpolation if at 1
				return outF(val);
			}
			float startVal = inF(val);
			float endVal = outF(val);
			float blendLerpVal = Mathf.Clamp01((2f * Frac(val)) - 0.5f);
			blendLerpVal = RTEase.QuadInOut(blendLerpVal);
			return MathS.Lerp(startVal, endVal, blendLerpVal); //lerp between the two interpolated values.
		}
		/**
		 *Get an interpolation from two specified types
		*/
		public static float Interpolate(TweenTypes inType, TweenTypes outType, float val) {
			if(inType == outType) {
				return Interpolate(GetInterpolator(inType), GetInterpolator(outType), val);
			}
			return Interpolate(GetInterpolator(inType,true,false), GetInterpolator(outType,false,true), val);
		}
		/**
		 * Setup a function to mix to interpolators 
		*/
		public struct InterpolationMixer
		{
			//TweenTypes m_inType;
			//TweenTypes m_outType;
			TweenLoopMode m_loopMode;
			InterpolationFunc inTween;
			InterpolationFunc outTween;

			public InterpolationMixer(TweenTypes inType, TweenTypes outType, TweenLoopMode loopMode) {
				if (inType == outType) {
					inTween = GetInterpolator(inType);
					outTween = GetInterpolator(outType);
				} else {
					inTween = GetInterpolator(inType, true, false);
					outTween = GetInterpolator(outType, false, true);
				}
				//m_inType = inType;
				//m_outType = outType;
				m_loopMode = loopMode;
			}
			public float Tween(float val) {
				if(outTween == null) {
					if(inTween == null) {
						return val;
					}
					inTween(val);
				}else if(inTween == null) {
					return outTween(val);
				}
				return Interpolate(inTween, outTween, val, m_loopMode);
			}
		}
	}

	public abstract class TweenBase : MonoBehaviour 
	{
		private Coroutine tweenCoroutine;
			static WaitForSeconds pauseWait = new WaitForSeconds(0.3f);
			static WaitForFixedUpdate fixedWait = new WaitForFixedUpdate();
			private WaitForSeconds setWait = null;// new WaitForSeconds(1f/30f);
			WaitForSeconds startDelayWait;
			new public string name;
		public bool useNameAsRandomSeed = false;
			public enum UpdateType :int{Update,FixedUpdate,GlobalTime};
			public enum VisibilityPause {None,Self,AllChildren};
			[System.Serializable]
			public class UpdateInterface{
				public UpdateType updateType;
				public VisibilityPause pauseOffscreen = VisibilityPause.AllChildren;
				public bool respectGlobalTimeScale = true;
				[Tooltip("Stop interpolation at current value on script cancel")]
				public bool allowInterupt = false;
				public enum PlaySpeed : int { All=0,_90_FPS=90, _60_FPS = 60, _30_FPS = 30, _24_FPS = 24, _12_FPS = 12 , _6_FPS = 6, _1_FPS = 1};
				public PlaySpeed playSpeed;
			}
			public UpdateInterface updateSettings;
			private UpdateType updateType {get{return updateSettings.updateType;} set{updateSettings.updateType = value;}}
			private VisibilityPause pauseOffscreen {get{return updateSettings.pauseOffscreen;} set{updateSettings.pauseOffscreen = value;}}
			protected float count; //Current tween time. 
			protected float loopCount;
			protected int currentLoop;
			protected float timeAtLastUpdate;
			protected bool respectGlobalTimeScale {get{return updateSettings.respectGlobalTimeScale;}}
			protected bool allowInterupt {get{return updateSettings.allowInterupt;}}
			[System.NonSerialized][HideInInspector]public bool interupt = false; //flag set to force stop of Coroutine.


		//tween types to choose from.
		//public enum TweenTypes { Linear, Step, Quadratic, Cubic, Back, Exponential, Sine, Elastic };
		//public enum TweenFX {None,Bounce,Step,Random,Elastic,Anticipate};

		[System.Serializable]
		public class TimeInterface{
			public bool reverse;
			[Space(5)]
			[Tooltip("Time in seconds to play")]
			public float time = 1f;
			[Tooltip("multiply TIME by amount per instance. random between values specified. Good for multiple objects you want some variation on.")]
			public Vector2 timeRandomScale = Vector2.one;
			[Tooltip("Sets to start interpolation at script start (before delay)")]
			public bool setInitialAtStart = false;
			[Tooltip("Reset to first value at end of playing or script cancel if allowed.")]
			public bool resetToBegining = false;
			[Tooltip("Time in seconds before animation starts playing")]
			public float startDelay = 0f;
			[Tooltip("Randomize and run the delay every loop iteration")]
			public bool delayEveryLoop = false;
			[Tooltip("Set Y above 0 for random start time. Time in Seconds")]
			public Vector2 startAtTime = new Vector2(0f,-1f);

		}
		public TimeInterface timeSettings;

		protected bool reverse {get{return timeSettings.reverse;} set {timeSettings.reverse=value;} }

		protected float time {get{return timeSettings.time;}}
		protected Vector2 timeRandomScale {get{return timeSettings.timeRandomScale;}}

		protected float timeScale = 1f; //sets to randomized value.
		protected int loopItteration = -1; //this number tracks the number of loops run.

		protected bool setInitialAtStart {get{return timeSettings.setInitialAtStart;}}
		protected bool resetToBegining {get{return timeSettings.resetToBegining;}}
		protected float startDelay {get{return timeSettings.startDelay;}}
		protected Vector2 startAtTime {get{return timeSettings.startAtTime;}}




		[System.Serializable]
		public class InterpolationInterface{
			[Tooltip("Uncheck to use non curve interpolation")]
			public bool useCurve = true;
			[Space(-3)]
			[Header("Wrap Mode set on curve ends")]
			[Tooltip("Check set wrap settings for looping.")]
			public AnimationCurve interpolation =  new AnimationCurve(new Keyframe(0,0),new Keyframe(1,1));
			[Tooltip("Install GoTween for best results. Uses set function if useCurve is false.")]
			public ProgramaticInterpolation.TweenTypes nonCurveInterpolation = ProgramaticInterpolation.TweenTypes.Linear;
			public ProgramaticInterpolation.TweenTypes nonCurveInterpolationOut = ProgramaticInterpolation.TweenTypes.Linear;
			public ProgramaticInterpolation.TweenLoopMode nonCurveLoopMode = ProgramaticInterpolation.TweenLoopMode.Loop;
			[Space(10)]
			[Tooltip("Loops using curve loop settings if using curve")]
			public bool loop = true;
			[Tooltip("Randomizes time scaling every loop instead of once at start")]
			public bool timeRandomEveryLoop = false;
			[Tooltip("Number of times to loop. -1 for infinite.")]
			public float loopNumberOfTimes = -1;
		}
		//[Header("Interpolation")]
		public InterpolationInterface interpolation;
		protected bool useCurve {get{return interpolation.useCurve;}}
		protected ProgramaticInterpolation.TweenTypes nonCurveInterpolation {get{return interpolation.nonCurveInterpolation;}}
		//TODO: Implement more tween types.quadratic, exponential etc.
		protected bool loop {get{return interpolation.loop;}}
		protected bool timeRandomEveryLoop{get{return interpolation.timeRandomEveryLoop;}}
		protected float loopNumberOfTimes {get{return interpolation.loopNumberOfTimes;}}
		protected bool currentlyLooping = false;//Set to current loop status in coroutine and used to cancel at loop end.
		protected int currentLoopNumberOfTimes;
	
	
		protected abstract void LerpParameters(float lerp); //must be overriden to provide lerp
		protected virtual bool HasValidParameters(){ return (interpolation.interpolation.length > 0);} //validate
		protected bool isPaused = false; //pause Coroutine
		protected new Renderer renderer; //store renderer. needed for some child types.
		protected Renderer[] renderers; //store renderer. needed for some child types.
		private delegate float InterpolationFunc(float val);//pass interpolation type function as argument.

		[System.Serializable]
		public class EventInterface{
			public float eventTime = 0f;
			public UnityEvent atTime;
			public UnityEvent onLoopComplete;
		}
		private bool eventInvoked = false;
		public EventInterface events;


		protected void Awake()
		{
			//renderer = GetComponent<Renderer>();//used to be get in children, but we pretty much expect this to work on this object only, and this way is cheaper.//now just do in color base.
            startDelayWait = new WaitForSeconds(startDelay);
			if (updateSettings.playSpeed != UpdateInterface.PlaySpeed.All) {
				float waitStep = 1f / (float)updateSettings.playSpeed;
				if (waitStep > Time.fixedDeltaTime) {
					setWait = new WaitForSeconds(waitStep);
				}
			}
		}
		protected void Start() {
			if (this.updateSettings.pauseOffscreen == VisibilityPause.AllChildren) { 
				renderers = GetComponentsInChildren<Renderer>(true);
			}
			if(setInitialAtStart){
				count = startAtTime.x;
				float lerp;
				if(useCurve){
					lerp = interpolation.interpolation.Evaluate(count);
				}
				else{
					lerp = ProgramaticInterpolation.GetInterpolator(this.nonCurveInterpolation)(count);
				}
				LerpParameters(lerp);//set to start values.
			}
		}
		protected bool AnyChildVisible(){
			if(renderer && renderer.isVisible) return true;
			if(renderers!=null && renderers.Length > 0){
				foreach(var rend in renderers){
					if(rend != null && rend.isVisible)
						return true;
				}
			}
			return false;
		}
		protected delegate T LerpFunc<T>(T from,T to,float lerp);
		
		protected static Vector3 LerpArray(Vector3[] inArr, float lerp){
			if(inArr == null || inArr.Length == 0){ 
				return Vector3.zero;
			}
			if(lerp == 0f){
				return inArr[0];
			}
			int topArrayIndex = inArr.Length-1;
			if(lerp == 1f){
				return inArr[topArrayIndex];
			}
			int toIndex=1;
			int fromIndex=0;
			if(lerp > 0){
				toIndex = Mathf.Min(topArrayIndex,Mathf.CeilToInt(lerp * (topArrayIndex)));
				fromIndex = toIndex -1;
			}
			float sectionLerp = (lerp*topArrayIndex) - fromIndex;
			
			Vector3 val = MathS.Vector3LerpUnclamped(inArr[fromIndex],inArr[toIndex],sectionLerp);
			return val;
		}
		protected static T LerpArray<T>(T[] inArr, float lerp,LerpFunc<T> Lerp){
			if(inArr == null){ 
				return default(T);
			}
			if(lerp == 0f){
				return inArr[0];
			}
			int topArrayIndex = inArr.Length-1;
			if(lerp == 1f){
				return inArr[topArrayIndex];
			}
			int toIndex=1;
			int fromIndex=0;
			if(lerp > 0){
				toIndex = Mathf.Min(topArrayIndex,Mathf.CeilToInt(lerp * (topArrayIndex)));
				fromIndex = toIndex -1;
			}
			float sectionLerp = (lerp*topArrayIndex) - fromIndex;

			T val = Lerp(inArr[fromIndex],inArr[toIndex],sectionLerp);
			return val;
		}


		protected float GetTimeScale(){
			int timeTic = (int)count;
			if(this.loopItteration < 0 || (timeTic != this.loopItteration && this.timeRandomEveryLoop)){
				loopItteration = timeTic;
				string seed = useNameAsRandomSeed ? this.name + currentLoop.ToString() : null;
                timeScale = MathS.TrulyRandomRange(this.timeRandomScale.x,timeRandomScale.y, seed);
			}
			return timeScale* (respectGlobalTimeScale?Time.timeScale:1);
		}
		private bool timeCheck() {
			return (currentlyLooping || (count <= time && count >= 0f));
		}
		protected ProgramaticInterpolation.InterpolationMixer programaticTweenMixer;
		IEnumerator Tween(float startAt)
		{
			currentlyLooping = loop;
			currentLoopNumberOfTimes = (int)loopNumberOfTimes;

			eventInvoked = false;
			if(pauseOffscreen == VisibilityPause.AllChildren){
				while( !AnyChildVisible()){
					isPaused=true;
						yield return pauseWait;
				}
				isPaused=false;
				//this.enabled = false;
				//yield break;
			}
			count = startAt;
			timeAtLastUpdate = Time.time; //initialize time for start
			ProgramaticInterpolation.InterpolationFunc getLerp = interpolation.interpolation.Evaluate;
			if(!useCurve){
				programaticTweenMixer = new ProgramaticInterpolation.InterpolationMixer(interpolation.nonCurveInterpolation, interpolation.nonCurveInterpolationOut,interpolation.nonCurveLoopMode);
				getLerp = programaticTweenMixer.Tween;
			}
			if(setInitialAtStart){
				LerpParameters(getLerp(count));//set to start values.
			}
			if(startDelay > 0f){
				yield return startDelayWait;
			}
			if(events.eventTime == 0 && !eventInvoked){ //invoke time zero events before lerp.
				events.atTime.Invoke();
				eventInvoked = true;
			}
			if(!setInitialAtStart){
				LerpParameters(getLerp(count));//set to start values.
			}

			while(timeCheck() && this.enabled && !isPaused){

				if(currentlyLooping && currentLoopNumberOfTimes != -1 && loopCount/time >= currentLoopNumberOfTimes) break; //stop loop, not coroutine.

	

				if (pauseOffscreen == VisibilityPause.AllChildren && !AnyChildVisible()){
					isPaused=true;
					//yield break;
					yield return pauseWait;
				}
				isPaused=false;

				//if(renderer && !renderer.isVisible){
				//	isPaused=true;
				//	this.enabled = false;
				//	yield break;
				//}


				float lerp;// =  interpolation.Evaluate(count/time);//getLerp(count/time);
				if(useCurve){
						lerp =  interpolation.interpolation.Evaluate(count/time);//getLerp(count/time);
				}
				else{
					lerp = getLerp(count/time);
				}
				LerpParameters(lerp);
				float timeVal = Time.unscaledDeltaTime;
                if (updateSettings.playSpeed != UpdateInterface.PlaySpeed.All) {
					float timeNow = Time.realtimeSinceStartup;
					timeVal = timeNow - timeAtLastUpdate;
					timeAtLastUpdate = timeNow;
				} else if((updateType != UpdateType.Update)) {
					timeVal =  Time.fixedDeltaTime;
				}



				float loopIncrement = timeVal * GetTimeScale();

				count += loopIncrement * (reverse?-1:1) ;
				if(updateType == UpdateType.GlobalTime) count = Time.time * (reverse?-1:1);
				loopCount += loopIncrement;

				if(loopCount >= events.eventTime && !eventInvoked){
						events.atTime.Invoke();
					eventInvoked = true;
				}
				if (currentLoop != Mathf.FloorToInt(count / time)) {//DetectStart of new loop
					if (timeSettings.delayEveryLoop && startDelay != 0) {
						string seed = useNameAsRandomSeed ? this.name + currentLoop.ToString() : null;			
						float newDelay = MathS.TrulyRandomRange(0, startDelay, seed);
                        yield return new WaitForSeconds(MathS.TrulyRandomRange(0, newDelay, this.name));
						//Debug.Log(this.gameObject.name + " : " + newDelay);
                        timeAtLastUpdate = Time.realtimeSinceStartup;
					}
					currentLoop = Mathf.FloorToInt(count / time);
				}
				if (updateType == UpdateType.Update) {
					yield return setWait;
				} else {
					yield return fixedWait;
                }
	
			}
			//this.enabled = false;

			if(isPaused){
				this.enabled = false;
				yield break;
			}
			if(!allowInterupt || (allowInterupt && !interupt)){
				var start = 0f;
				var end = 0.9999999f;
				if(reverse){
					start = 0.9999999f;
					end = 0f;
				}
				float lerp;
				if(useCurve){
						lerp =  interpolation.interpolation.Evaluate(resetToBegining?start:end);//getLerp(count/time);
				
				}
				else{
					lerp = getLerp(resetToBegining?start:end);
				}
				//Debug.Log ("end  : interrupted " + interupt);
				LerpParameters(lerp);
				if(events.onLoopComplete.GetPersistentEventCount() > 0){
					events.onLoopComplete.Invoke();
				}
			
			}
			interupt = false;
			this.enabled = false;
		}

	IEnumerator RestartCoroutine()
	{
		this.enabled = false;
		yield return fixedWait;
		this.enabled = true;
	}
	IEnumerator RestartPlay()
	{
		this.enabled = false;
		yield return fixedWait;
		count = 0;
		loopCount = 0;
		DoTween();
		this.enabled = true;
	}
		/** resume when onscreen. won't work if not on object without renderer =( .*/
		//void OnBecameVisible()
		//{
		//	if(pauseOffscreen == VisibilityPause.Self && this.isPaused){
		//		this.isPaused = false;
		//		this.enabled = true;
		//	}
		//}
		/** pause when offscreen*/
		//void OnBecameInvisible()
		//{
		//	if(pauseOffscreen == VisibilityPause.Self && this.enabled){
		//		this.isPaused = true;
		//		this.enabled = false;
		//	}
		//}
		/** CancelTween
		 * send command to stop tweening on this script.
		  *@param doInterupt defaults to false. Force tween to stop where it is.
		*/
		//Finish now
		//Stop in place
		//Stop and Reset
		//Stop at loop end
		public enum TweenCancelType {Finish, SoftStop, HardStop, CancelAtLoopEnd, };
		public void CancelTween() {
			CancelTween(TweenCancelType.Finish);
		}
		public void CancelTween(bool hardStop) {
			if (hardStop) {
				CancelTween(TweenCancelType.HardStop);
			} else {
				CancelTween(TweenCancelType.SoftStop);
			}
		}
		public void CancelTween(TweenCancelType type)
		{
			switch (type) {
				case TweenCancelType.Finish: //let things finish naturally
					this.isPaused = false;
					this.enabled = false;
					break;
				case TweenCancelType.SoftStop: //cancel. depend on settings to reset position etc. end Events force fire.
					this.isPaused = false;
					this.enabled = false;
					if (tweenCoroutine != null) {
						StopCoroutine(tweenCoroutine);
					}
					if (this.resetToBegining) { //this resets stuff.
						LerpParameters(0f);
						this.count = 0f;
					}
					if (events.onLoopComplete.GetPersistentEventCount() > 0) {
						events.onLoopComplete.Invoke();
					}
					break;
				case TweenCancelType.HardStop: //force to stop where is. no end events.
					//No matter what force back to frame 0, reset count
					this.isPaused = false;
					this.enabled = false;
					if (tweenCoroutine != null) {
						StopCoroutine(tweenCoroutine);
					}
					LerpParameters(0f);
					this.count = 0f;
					break;
				case TweenCancelType.CancelAtLoopEnd: //finish current loop and end.
					currentLoopNumberOfTimes = Mathf.CeilToInt(count);
                    break;
				default: //this shouldn't ever happen
					Debug.LogWarning("Poorly added tween cancel state? don't use this...");
					this.enabled = false;
					if (tweenCoroutine != null) {
						StopCoroutine(tweenCoroutine);
					}
					break;
				}
			}

		/**DoTween()
		 * used to start tween by script. Will restart if already running.
		 */
		protected void DoTween()
		{
			if(this.enabled && this.gameObject.activeInHierarchy){//if we're already going, start over.
				StartCoroutine(RestartCoroutine());
			}else {//otherwise enable.
				this.enabled = true;
			}
		}
		/**
		Initiates play of tween.
		*/
		public void PlayForwards(){
			if(!this.gameObject.activeInHierarchy){
				//Debug.LogWarning("Can't play on inactive object");
				return;
			}
			if(this.enabled){
				StartCoroutine(RestartPlay());
			}
			else{
				this.reverse = false;
				count = 0;
				loopCount = 0;
				DoTween();
			}
		}
		/**
		sets the count to 0. if coroutine is running this should move the animation back to beginning.
		Doesn't re-enable events which may have already fired.
		*/
		public void ResetToStart(){
			this.count = 0f;
		}
		/**
		Play the tween backwards, starting at time length of tween.
		*/
		public void PlayReverse(){
			PlayReverse (this.time);
		}
		/**
		Play tween backwards from startTime back to 0. (unless looping)
		*/
		public void PlayReverse(float startTime)
		{
			this.reverse = true;
			timeSettings.setInitialAtStart = true; //do I want to do this really?????
			count = startTime;
			loopCount = 0;
			this.enabled = true;
			//StartCoroutine(Tween(startTime));
		}
		/**
		Go to this point in the tween Directly. Doesn't animate (or set current time if running)
		*/
		public virtual void SetToLerpPoint(float lerp){
			//used for editor scripts/
		}
		//bool wasenabled=false;
		protected void OnEnable()
		{
			//if(wasenabled) {Debug.Log ("huh?");return;}
			//wasenabled = true;
			if(HasValidParameters())
			{
				if(!setInitialAtStart){//may have already set start time...
					count = (startAtTime.x%time)/time;
					if(startAtTime.y >=0f){
						string seed = useNameAsRandomSeed ? this.name + currentLoop.ToString() : null;
						count = MathS.TrulyRandomRange((startAtTime.x%time)/time,(startAtTime.y%time)/time, seed) * time;
					}
				}
				tweenCoroutine = StartCoroutine(Tween(count));
			}
			else{
				Debug.LogWarning("Invalid Tween Parameters on " + this.gameObject.name);//this.GetPath());
			}
		}
		//protected void OnDisable(){
		//	wasenabled = false;
		//}
		/**
		Since we already (in some cases) have this stored, expose it to so we can access without another GetComponentCall.
		*/
		public Renderer GetRenderer(){
			return renderer;
		}
	}
}