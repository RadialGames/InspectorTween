// #Generic
/**
* Base for Radial Games Tween scripts.
*/
using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System;


namespace InspectorTween {

	//RTEase by MattRix : https://gist.github.com/MattRix/feea68fd3dae16c760d6c665fd530d46
	public static class RTEase {
		//public static Func<float, float> Linear = (t) => { return t; };
		public static Func<float, float> Instant = (t) => {
			return t < 1f ? 0f : 1f;
		};
		public static Func<float, float> QuadIn = (t) => {
			return t * t;
		};
		public static Func<float, float> QuadOut = (t) => {
			return 2f * t - t * t;
		};
		public static Func<float, float> QuadInOut = (t) => {
			return (t <= 0.5f) ? (t * t * 2f) : (-1.0f + 4f * t + -2f * t * t);
		};
		public static Func<float, float> CubeIn = (t) => {
			return t * t * t;
		};
		public static Func<float, float> CubeOut = (t) => {
			return 1f - CubeIn(1f - t);
		};
		public static Func<float, float> CubeInOut = (t) => {
			return (t <= 0.5f) ? CubeIn(t * 2f) * 0.5f : CubeOut(t * 2f - 1f) * 0.5f + 0.5f;
		};
		public static Func<float, float> BackIn = (t) => {
			return t * t * (2.70158f * t - 1.70158f);
		};
		public static Func<float, float> BackOut = (t) => {
			return 1f - BackIn(1f - t);
		};
		public static Func<float, float> BackInOut = (t) => {
			return (t <= 0.5f) ? BackIn(t * 2f) * 0.5f : BackOut(t * 2f - 1f) * 0.5f + 0.5f;
		};
		public static Func<float, float> ExpoIn = (t) => {
			return Mathf.Pow(2f, 10f * (t - 1.0f));
		};
		public static Func<float, float> ExpoOut = (t) => {
			return 1f - Mathf.Pow(2f, -10f * t);
		};
		public static Func<float, float> ExpoInOut = (t) => {
			return t < .5f ? ExpoIn(t * 2f) * 0.5f : ExpoOut(t * 2f - 1f) * 0.5f + 0.5f;
		};
		public static Func<float, float> SineIn = (t) => {
			return -Mathf.Cos(Mathf.PI * 0.5f * t) + 1f;
		};
		public static Func<float, float> SineOut = (t) => {
			return Mathf.Sin(Mathf.PI * 0.5f * t);
		};
		public static Func<float, float> SineInOut = (t) => {
			return -Mathf.Cos(Mathf.PI * t) * 0.5f + .5f;
		};
		public static Func<float, float> ElasticIn = (t) => {
			return 1f - ElasticOut(1f - t);
		};
		public static Func<float, float> ElasticOut = (t) => {
			return Mathf.Pow(2f, -10f * t) * Mathf.Sin((t - 0.075f) * (2f * Mathf.PI) / 0.3f) + 1f;
		};
		public static Func<float, float> ElasticInOut = (t) => {
			return (t <= 0.5f) ? ElasticIn(t * 2f) : ElasticOut(t * 2f - 1f);
		};
	}

	public static class ProgramaticInterpolation {
		public enum TweenTypes {
			Linear,
			Step,
			Quadratic,
			Cubic,
			Back,
			Exponential,
			Sine,
			Elastic
		};

		public enum TweenLoopMode {
			Loop,
			PingPong,
			Continuous,
			Clamp
		}

		public delegate float InterpolationFunc(float val); //pass interpolation type function as argument.

		//const float TAU = Mathf.PI * 2;
		private static float Frac(float val) {
			return val % 1f; // val - Mathf.FloorToInt(val);
		}

		private static float Linear(float val) {
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

		private static float Sine(float val) {
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
		public static InterpolationFunc GetInterpolator(TweenTypes type, bool inOnly = false, bool outOnly = false) {
			InterpolationFunc rFunc;
			switch ( type ) {
				case TweenTypes.Linear:
					rFunc = Linear;
					break;
				case TweenTypes.Step:
					rFunc = Step;
					break;
				case TweenTypes.Quadratic:
					rFunc = Quadratic;
					if ( inOnly ) {
						rFunc = QuadraticIn;
					} else if ( outOnly ) {
						rFunc = QuadraticOut;
					}

					break;
				case TweenTypes.Cubic:
					rFunc = Cubic;
					if ( inOnly ) {
						rFunc = CubicIn;
					} else if ( outOnly ) {
						rFunc = CubicOut;
					}

					break;
				case TweenTypes.Back:
					rFunc = Back;
					if ( inOnly ) {
						rFunc = BackIn;
					} else if ( outOnly ) {
						rFunc = BackOut;
					}

					break;
				case TweenTypes.Exponential:
					rFunc = Exponential;
					if ( inOnly ) {
						rFunc = ExponentialIn;
					} else if ( outOnly ) {
						rFunc = ExponentialOut;
					}

					break;
				case TweenTypes.Sine:
					rFunc = Sine;
					if ( inOnly ) {
						rFunc = SineIn;
					} else if ( outOnly ) {
						rFunc = SineOut;
					}

					break;
				case TweenTypes.Elastic:
					rFunc = Elastic;
					if ( inOnly ) {
						rFunc = ElasticIn;
					} else if ( outOnly ) {
						rFunc = ElasticOut;
					}

					break;
				default:
					rFunc = Linear;
					break;
			}

			return rFunc;
		}

		/**
		 *Get an interpolationblended between two specified functions.
		*/
		private static float Interpolate(InterpolationFunc inF, InterpolationFunc outF, float val, TweenLoopMode mode = TweenLoopMode.Loop) {
			switch ( mode ) {
				case TweenLoopMode.Loop:
					val = Frac(val);
					break;
				case TweenLoopMode.PingPong:
					val = (val % 2) > 1f ? 1f - Frac(val) : Frac(val);
					break;
				case TweenLoopMode.Continuous:
					//val = val; Do nothing
					break;
				case TweenLoopMode.Clamp:
					val = Mathf.Clamp01(val);
					break;
			}

			//val = Frac(val);
			if ( inF == outF ) { //if the in and out are the same, then just do the one withoug interpolating them.
				return inF(val);
			}

			if ( val == 0f ) { //Only Do one if at 0
				return inF(val);
			}

			if ( val == 1f ) { //Only do one interpolation if at 1
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
			if ( inType == outType ) {
				return Interpolate(GetInterpolator(inType), GetInterpolator(outType), val);
			}

			return Interpolate(GetInterpolator(inType, true), GetInterpolator(outType, false, true), val);
		}

		/**
		 * Setup a function to mix to interpolators 
		*/
		public struct InterpolationMixer {
			private readonly TweenLoopMode m_loopMode;
			private readonly InterpolationFunc inTween;
			private readonly InterpolationFunc outTween;

			public InterpolationMixer(TweenTypes inType, TweenTypes outType, TweenLoopMode loopMode) {
				if ( inType == outType ) {
					inTween = GetInterpolator(inType);
					outTween = GetInterpolator(outType);
				} else {
					inTween = GetInterpolator(inType, true);
					outTween = GetInterpolator(outType, false, true);
				}

				m_loopMode = loopMode;
			}

			public float Tween(float val) {
				if ( outTween == null ) {
					if ( inTween == null ) {
						return val;
					}

					inTween(val);
				} else if ( inTween == null ) {
					return outTween(val);
				}

				return Interpolate(inTween, outTween, val, m_loopMode);
			}
		}
	}

	public struct AnimationCurves {//Should this be a static class???
		public enum AnimationCurveType {
			Custom = -1,
			Linear=0,
			Hermite=1,

			EaseIn,
			EaseOut,

			Step=4,
			BackIn,
			BackOut,
			Sin
		};
		public static AnimationCurve[] savedCurves = {
			new AnimationCurve(new Keyframe(0,0,1,1),new Keyframe(1,1,1,1)){postWrapMode = WrapMode.Loop,preWrapMode = WrapMode.Loop}//Linear
			,new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1)){postWrapMode = WrapMode.Loop,preWrapMode = WrapMode.Loop}//Hermite
			 #if UNITY_2018_1_OR_NEWER                         
			,new AnimationCurve(new Keyframe(0f,0f,2f,2f,0.3333333f,0.3333333f),new Keyframe(1f,1f,0f,0f,0.3333333f,0.3333333f))//EaseIn
			,new AnimationCurve(new Keyframe(0f,0f,0f,0f,0.3333333f,0.3333333f),new Keyframe(1f,1f,2f,2f,0.3333333f,0.3333333f))//EaseOut
			 #else
			,new AnimationCurve(new Keyframe(0f,0f,2f,2f),new Keyframe(1f,1f,0f,0f))//EaseIn
			,new AnimationCurve(new Keyframe(0f,0f,0f,0f),new Keyframe(1f,1f,2f,2f))//EaseOut
			#endif
			,new AnimationCurve(new Keyframe(0f,0f,0f,float.PositiveInfinity),new Keyframe(0.5f,1f,0f,0f),new Keyframe(1f,1f,0f,0f)){postWrapMode = WrapMode.Loop,preWrapMode = WrapMode.Loop}//Step                       
			,new AnimationCurve(new Keyframe(0f,0f,0f,0f),new Keyframe(0.5032355f,-0.1706448f,0.1443379f,0.1443379f),new Keyframe(1f,1f,3.219463f,3.219463f))//Back In
			,new AnimationCurve(new Keyframe(0f,0f,5.243961f,5.243961f),new Keyframe(0.500102f,1.065629f,0.06565601f,0.06565601f),new Keyframe(1f,1f,0f,0f))//BackOut 
			,new AnimationCurve(new Keyframe(0f,0f),new Keyframe(0.5f,0.5f,1.854149f,1.854149f),new Keyframe(1f,1f)){postWrapMode = WrapMode.PingPong,preWrapMode = WrapMode.PingPong}//Sin
		 };

		public static void SetAnimationCurve(ref TweenBase.InterpolationInterface interpolationInterface, AnimationCurveType curveType) {
			if ( curveType == AnimationCurveType.Custom ) {
				
				return;
			}

			int index = (int) curveType;
			if ( index < 0 || index >= savedCurves.Length ){
				Debug.LogWarning("Invalid curve specified. index out of bounds");
				return;
			}
			AnimationCurve refCurve = savedCurves[index];
			interpolationInterface.interpolation = new AnimationCurve(refCurve.keys){postWrapMode = refCurve.postWrapMode,preWrapMode = refCurve.preWrapMode};
		}
	}
	
	//public static class Tween {
	//	public static T AddTween<T>(GameObject t) where T : TweenBase {
	//		T newTween = t.AddComponent<T>();
	//		newTween.AwakeInit();
	//		return newTween;
	//	}
	//}
	public abstract class TweenBase : MonoBehaviour {
		/// <summary>
		/// Create a Tween programatically
		/// </summary>
		/// <param name="t">GameObject to add the tween to</param>
		/// <param name="play">set enabled state of tween</param>
		/// <param name="loop">set looping state</param>
		/// <typeparam name="T">InspectorTween Tween class</typeparam>
		/// <returns></returns>
		public static T AddTween<T>(GameObject t,bool play = true,bool loop = true) where T : TweenBase {
			T newTween = t.AddComponent<T>();
			newTween.enabled = false;
			//These don't get magically init'd like in inspector.
			newTween._updateSettings = new UpdateInterface();
			newTween._interpolation=  new InterpolationInterface();
			newTween._timeSettings = new TimeInterface();
			newTween.events = new EventInterface();
			newTween.Reset();//Reset usuallt gets called on add in editor, and adds some sane defaults vs environment.
			newTween.AwakeInit();//Re-do the awake stuff after initializing the 
			newTween.loop = loop;
			if ( play ) {
				newTween.enabled = true;
			}

			var renderers = t.GetComponentsInChildren<Renderer>();
			if ( renderers.Length == 0 ) {
				newTween.updateSettings.pauseOffscreen = VisibilityPause.None;
			}
;			return newTween;
		}

		public TweenBase SetAnimationCurve(AnimationCurves.AnimationCurveType curve) {
			AnimationCurves.SetAnimationCurve(ref _interpolation,curve);
			return this;
		}
		
		
		[SerializeField]protected bool simpleMode;
	#if TWEEN_MASKS_ENABLED
		/// <summary>
		/// assign a TweenMask to override interface settings.
		/// </summary>
		[ConditionalHide("simpleMode", true, true)]
		public TweenMask settingsMask;
	#else
		[HideInInspector][SerializeField]private TweenMask settingsMask; //Keep around for toggling and keeping state.
	#endif
		private Coroutine tweenCoroutine;
		static readonly WaitForSeconds pauseWait = new WaitForSeconds(0.3f);
		static readonly WaitForFixedUpdate fixedWait = new WaitForFixedUpdate();
		private WaitForSeconds setWait; // new WaitForSeconds(1f/30f);
		private WaitForSeconds startDelayWait;
		/// <summary>
		/// name to label the component. Can be used as random seed if flag enabled so that tweens share randomness.
		/// </summary>
		public new string name;

		/// <summary>
		/// Mark so that any random parameters are seeded by the name on the component
		/// </summary>
		public bool useNameAsRandomSeed {
			get { return _useNameAsRandomSeed; }
			set { _useNameAsRandomSeed = value; }
		}

		[ConditionalHide("simpleMode", true, true)]
		[UnityEngine.Serialization.FormerlySerializedAs("useNameAsRandomSeed")]
		[SerializeField]protected bool _useNameAsRandomSeed;

		public enum UpdateType {
			Update,
			FixedUpdate,
			GlobalTime
		};

		public enum VisibilityPause {
			None,
			Self,
			AllChildren
		};

		[Serializable]
		public class UpdateInterface {
			public UpdateType updateType;
			public VisibilityPause pauseOffscreen = VisibilityPause.AllChildren;
			public bool respectGlobalTimeScale = true;

			public enum PlaySpeed {
				All = 0,
				_90_FPS = 90,
				_60_FPS = 60,
				_30_FPS = 30,
				_24_FPS = 24,
				_12_FPS = 12,
				_6_FPS = 6,
				_1_FPS = 1
			};

			public PlaySpeed playSpeed;

			[Header("On Disable")] [Tooltip("Stop interpolation at current value on disable")]
			public bool allowInterupt;
		}

		[UnityEngine.Serialization.FormerlySerializedAs("updateSettings")] [ConditionalHide("simpleMode", true, true)]
		[SerializeField]protected UpdateInterface _updateSettings;
		/// <summary>
		/// Settings which affect update speed, pausing, and tween end resolution.
		/// </summary>
		public UpdateInterface updateSettings {
			get {
				#if TWEEN_MASKS_ENABLED
				return settingsMask == null ? _updateSettings : settingsMask.updateSettings;
				#else
				return _updateSettings;
				#endif
			}
		}

		#region updateAccessors

		private UpdateType updateType {
			get { return settingsMask == null ? updateSettings.updateType : settingsMask.updateSettings.updateType; }
		}

		private VisibilityPause pauseOffscreen {
			get { return settingsMask == null ? updateSettings.pauseOffscreen : settingsMask.updateSettings.pauseOffscreen; }
		}

		protected bool respectGlobalTimeScale {
			get { return settingsMask == null ? updateSettings.respectGlobalTimeScale : settingsMask.updateSettings.respectGlobalTimeScale; }
		}

		private UpdateInterface.PlaySpeed playSpeed {
			get { return settingsMask == null ? updateSettings.playSpeed : settingsMask.updateSettings.playSpeed; }
		}

		protected bool allowInterupt {
			get { return settingsMask == null ? updateSettings.allowInterupt : settingsMask.updateSettings.allowInterupt; }
		}

		#endregion

		#region updateRuntimeVariables

		protected float count;
		protected float loopCount;
		protected int currentLoop;
		protected float timeAtLastUpdate;

		#endregion

		[Serializable]
		public class TimeInterface {
			[Tooltip("Play backwards")] [ConditionalHide("simpleMode", true, true)]
			public bool reverse;

			[Tooltip("Play curve forwards over value in reverse order")] [ConditionalHide("simpleMode", true, true)]
			public bool reverseValues;

			[Tooltip("Time in seconds to play")] 
			public float time = 1f;

			[Tooltip("multiply TIME by amount per instance. random between values specified. Good for multiple objects you want some variation on.")]
			public Vector2 timeRandomScale = Vector2.one;

			[Header("Tween Start")] [Tooltip("Sets to start interpolation at script start (before delay)")] [ConditionalHide("simpleMode", true, true)]
			public bool initBeforeDelay;

			[Tooltip("Time in seconds before animation starts playing [depricating in favour of below]")] [ConditionalHide("simpleMode", true, true)]
			public float startDelay;

			[Tooltip("Start Delay within random range. Trumps `start Delay.`")]
			public Vector2 randomStartDelay;

			[Tooltip("Randomize and run the delay every loop iteration")] [ConditionalHide("interpolation.loop", true, false)]
			public bool delayEveryLoop;

			[Tooltip("Set Y above 0 for random start time. Time in Seconds")]
			public Vector2 startAtTime = new Vector2(0f, -1f);

			[Header("Tween End")] [Tooltip("Reset to first value at end of playing or script cancel if allowed.")] [ConditionalHide("simpleMode", true, true)]
			public bool resetToBegining;
			
		}
		/// <summary>
		/// Tween length adjustments, like delay and time scaling.
		/// </summary>
		[UnityEngine.Serialization.FormerlySerializedAs("timeSettings")] 
		[SerializeField] protected TimeInterface _timeSettings;
		public TimeInterface timeSettings {
			get {
				#if TWEEN_MASKS_ENABLED
				return settingsMask == null ? _timeSettings : settingsMask.timeSettings;
				#else
				return _timeSettings;
				#endif
			}
		}

		#region timeSettingsAccessors
		/// <summary>
		/// set tween to play from end to start.
		/// </summary>
		public bool reverse {
			get { return timeSettings.reverse; }
			set { timeSettings.reverse = value; }
		}

		/// <summary>
		/// set tween play direction
		/// </summary>
		/// <param name="state">play backwards</param>
		/// <returns>self</returns>
		public TweenBase ReverseValues(bool state) {
			timeSettings.reverseValues = state;
			return this;
		}
		/// <summary>
		/// set tween play length in seconds
		/// </summary>
		public float time {
			set {
				if ( time <= 0 ) {
					Debug.LogWarning("Inspector Tween : Invalid time setting "+value+ " on "+ gameObject.name+". Clamping to greater than 0");
					value = 1/60f;
				}
				timeSettings.time = value;
			}
			get {
				float timeVal = timeSettings.time;
				if ( timeVal == 0 ) {
					timeVal = 1f / 60f;
				}
				return timeVal;
			}
		}

		/// <summary>
		/// set tween play length in seconds
		/// </summary>
		/// <param name="val">time in seconds</param>
		/// <returns>self</returns>
		public TweenBase SetTime(float val) {
			if ( val <= 0 ) {
				Debug.LogWarning("INSPECTOR TWEEN : play times must be greater than 0");
				val = 1 / 60f;//lets' assume min play time to be 1/60th of a second instead of 0;
			}
			timeSettings.time = val;
			return this;
		}
		/// <summary>
		/// set tween play length to be randomly within range
		/// </summary>
		/// <param name="min">minimum time to play tween</param>
		/// <param name="max">maximum time to play tween</param>
		/// <returns></returns>
		public TweenBase SetTime(float min, float max) {
			if ( max < min ) { //Assure correct order of parameters.
				float temp = min;
				min = max;
				max = temp;
			}
			SetTime( min );
			timeSettings.timeRandomScale.x = 1;
			timeSettings.timeRandomScale.y = (max / min);
			return this;
		}
		protected Vector2 timeRandomScale {
			get { return timeSettings.timeRandomScale; }
		}

		protected bool initBeforeDelay {
			get { return timeSettings.initBeforeDelay; }
		}

		protected bool delayEveryLoop {
			get { return timeSettings.delayEveryLoop; }
		}

		protected Vector2 startAtTime {
			get { return timeSettings.startAtTime; }
		}

		protected bool resetToBegining {
			get { return timeSettings.resetToBegining; }
		}

		#endregion

		#region timeRelatedVariablesRuntime

		protected float timeScale = 1f; //sets to randomized value.
		protected int loopItteration = -1; //this number tracks the number of loops run.
		protected bool initializeCountOnEnable = true; // {get{return timeSettings.initBeforeDelay;}}

		protected float startDelay {
			get { return Mathf.Max(0, timeSettings.startDelay); }
		}

		protected Vector2 randomStartDelay {
			get { return timeSettings.randomStartDelay; }
		}

		#endregion


		[Serializable]
		public class InterpolationInterface {
			[Tooltip("Loops using curve loop settings if using curve")]
			public bool loop = true;

			[Tooltip("Randomizes time scaling every loop instead of once at start")] [ConditionalHide("_interpolation.loop", true, false)]
			public bool timeRandomEveryLoop;

			[Tooltip("Number of times to loop. -1 for infinite.")] [ConditionalHide("_interpolation.loop", true, false)]
			public float loopNumberOfTimes = -1;

			//[Header("Curve")]
			[Tooltip("Uncheck to use non curve interpolation")]
			public bool useCurve = true;

			//[WarningHeader("!!! Wrap Mode set on curve ends","yellow")]
			[Tooltip("Check set wrap settings for looping.")] [ConditionalHide("_interpolation.useCurve", true, false)]
			public AnimationCurve interpolation = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));

			//[Header("Programatic")]
			[Tooltip("Install GoTween for best results. Uses set function if useCurve is false.")] [ConditionalHide("_interpolation.useCurve", true, true)]
			public ProgramaticInterpolation.TweenTypes nonCurveInterpolation = ProgramaticInterpolation.TweenTypes.Linear;

			[ConditionalHide("_interpolation.useCurve", true, true)]
			public ProgramaticInterpolation.TweenTypes nonCurveInterpolationOut = ProgramaticInterpolation.TweenTypes.Linear;

			[ConditionalHide("_interpolation.useCurve", true, true)]
			public ProgramaticInterpolation.TweenLoopMode nonCurveLoopMode = ProgramaticInterpolation.TweenLoopMode.Loop;
			
		}

		[UnityEngine.Serialization.FormerlySerializedAs("interpolation")] [SerializeField]
		protected InterpolationInterface _interpolation;
		/// <summary>
		/// specifications of how tween is interpolated
		/// </summary>
		public InterpolationInterface interpolation {
			get {
				#if TWEEN_MASKS_ENABLED
				return settingsMask == null ? _interpolation : settingsMask.interpolation;
				#else
				return _interpolation;
				#endif
			}
		}

		#region interpolationAccessors

		protected bool loop {
			get { return interpolation.loop; }
			set { interpolation.loop = value; }
		}

		protected bool timeRandomEveryLoop {
			get { return interpolation.timeRandomEveryLoop; }
			set { interpolation.timeRandomEveryLoop = value; }
		}

		protected float loopNumberOfTimes {
			get { return interpolation.loopNumberOfTimes; }
			set { interpolation.loopNumberOfTimes = value; }
		}

		protected bool useCurve {
			get { return interpolation.useCurve; }
			set { interpolation.useCurve = value; }
		}

		/// <summary>
		/// Animation curve which is evaluated to interpolate between values of tween is useCurve is true.
		/// </summary>
		public AnimationCurve interpolationCurve {
			get { return interpolation.interpolation; }
			set { interpolation.interpolation = value; }
		}

		protected ProgramaticInterpolation.TweenTypes nonCurveInterpolation {
			get { return interpolation.nonCurveInterpolation; }
		}

		protected ProgramaticInterpolation.TweenTypes nonCurveInterpolationOut {
			get { return interpolation.nonCurveInterpolationOut; }
		}

		protected ProgramaticInterpolation.TweenLoopMode nonCurveLoopMode {
			get { return interpolation.nonCurveLoopMode; }
		}

		#endregion

		protected bool currentlyLooping; //Set to current loop status in coroutine and used to cancel at loop end.
		protected int currentLoopNumberOfTimes;


		protected abstract void LerpParameters(float lerp); //must be overriden to provide lerp

		protected virtual bool HasValidParameters() {
			return (interpolationCurve.length > 0);
		} //validate

		protected bool isPaused; //pause Coroutine

#if UNITY_EDITOR
		protected new Renderer renderer; //store renderer. needed for some child types.
#else
		protected Renderer renderer;
#endif
		protected Renderer[] renderers; //store renderer. needed for some child types.

		[Serializable]
		public class EventInterface {
			public enum EventFireCondition{Always,ForwardOnly,ReverseOnly,AtReverseTime,EveryLoop}
			public float eventTime;
			public EventFireCondition atTimeCondition;
			public UnityEvent atTime = new UnityEvent();
			public EventFireCondition onCompleteCondition;
			public UnityEvent onLoopComplete= new UnityEvent();
		}

		private bool eventInvoked;
		/// <summary>
		/// Events which fire on tween play
		/// </summary>
		public EventInterface events;
		protected float eventTime {
			get { return events.eventTime; }
		}

		protected UnityEvent atTime {
			get { return events.atTime; }
		}

		protected UnityEvent onLoopComplete {
			get { return events.onLoopComplete; }
		}


		protected virtual void Reset() { //Called Editor only when Component is first added.
			if ( _interpolation == null ) {
				_interpolation = new InterpolationInterface();
				//Debug.Log("no class yet");
			}

			interpolation.interpolation.postWrapMode = WrapMode.Loop;
			interpolation.interpolation.preWrapMode = WrapMode.Loop;

			RectTransform rt = GetComponent<RectTransform>();
			if ( rt != null ) {
				//Default UI components to have a sane default
				_updateSettings = new UpdateInterface {pauseOffscreen = VisibilityPause.None, respectGlobalTimeScale = false}; //this hasn't been created yet as that 'usually' happens in UI
			}
#if UNITY_EDITOR
			simpleMode = UnityEditor.EditorPrefs.GetBool("SimpleMode", false);
#endif
		}

		internal TweenBase AwakeInit() {
			
			if ( randomStartDelay.x > 0 && randomStartDelay.y > 0 ) {
				string seed = useNameAsRandomSeed ? name + currentLoop.ToString() : null;
				float newDelay = MathS.TrulyRandomRange(Mathf.Max(0, randomStartDelay.x), Mathf.Max(0, randomStartDelay.y), seed);
				startDelayWait = new WaitForSeconds(newDelay);
				timeAtLastUpdate = Time.realtimeSinceStartup;
			} else {
				startDelayWait = new WaitForSeconds(startDelay);
			}


			if ( playSpeed != UpdateInterface.PlaySpeed.All ) {
				float waitStep = 1f / (float) playSpeed;
				if ( waitStep > Time.fixedDeltaTime ) {
					setWait = new WaitForSeconds(waitStep);
				}
			}

			switch ( settingsMask == null ? updateSettings.pauseOffscreen : settingsMask.updateSettings.pauseOffscreen ) {
				case VisibilityPause.AllChildren:
					renderers = GetComponentsInChildren<Renderer>(true);
					break;
				case VisibilityPause.Self:
					renderer = GetComponent<Renderer>();
					break;
			}

			return this;
		}
		protected virtual void Awake() {
			if ( _updateSettings == null ) {//if added with Add component, classes like timeSettings will be null.
				return;
			}
			AwakeInit();
		}

		/// <summary>
		/// Determine visibility of collective renderers.
		/// </summary>
		/// <returns>any renderer on self or in children are visible to any cameras</returns>
		protected bool AnyChildVisible() {
			if ( renderer != null && renderer.isVisible ) {
				return true;
			}

			if ( renderers == null || renderers.Length == 0 ) {
				return false;
			}

			foreach ( Renderer rend in renderers ) {
				if ( rend != null && rend.isVisible ) {
					return true;
				}
			}

			return false;
		}

		protected delegate T LerpFunc<T>(T from, T to, float lerp);

		protected static Vector3 LerpArray(Vector3[] inArr, float lerp) {
			if ( inArr == null || inArr.Length == 0 ) {
				return Vector3.zero;
			}

			if ( lerp == 0f ) {
				return inArr[0];
			}

			int topArrayIndex = inArr.Length - 1;
			if ( lerp == 1f ) {
				return inArr[topArrayIndex];
			}

			int toIndex = 1;
			int fromIndex = 0;
			if ( lerp > 0 ) {
				toIndex = Mathf.Min(topArrayIndex, Mathf.CeilToInt(lerp * (topArrayIndex)));
				fromIndex = toIndex - 1;
			}

			float sectionLerp = (lerp * topArrayIndex) - fromIndex;

			Vector3 val = MathS.Vector3LerpUnclamped(inArr[fromIndex], inArr[toIndex], sectionLerp);
			return val;
		}

		protected static T LerpArray<T>(T[] inArr, float lerp, LerpFunc<T> Lerp) {
			if ( inArr == null ) {
				return default(T);
			}

			if ( lerp == 0f ) {
				return inArr[0];
			}

			int topArrayIndex = inArr.Length - 1;
			if ( lerp == 1f ) {
				return inArr[topArrayIndex];
			}

			int toIndex = 1;
			int fromIndex = 0;
			if ( lerp > 0 ) {
				toIndex = Mathf.Min(topArrayIndex, Mathf.CeilToInt(lerp * (topArrayIndex)));
				fromIndex = toIndex - 1;
			}

			float sectionLerp = (lerp * topArrayIndex) - fromIndex;

			T val = Lerp(inArr[fromIndex], inArr[toIndex], sectionLerp);
			return val;
		}


		protected float GetTimeScale() {
			int timeTic = (int) count;
			if ( loopItteration >= 0 && (timeTic == loopItteration || !timeRandomEveryLoop) ) {
				return timeScale * (respectGlobalTimeScale ? Time.timeScale : 1);
			}

			loopItteration = timeTic;
			string seed = useNameAsRandomSeed ? name + currentLoop.ToString() : null;
			timeScale = MathS.TrulyRandomRange(timeRandomScale.x, timeRandomScale.y, seed);
			return timeScale * (respectGlobalTimeScale ? Time.timeScale : 1);
		}

		/// <summary>
		/// check if tween loop should keep going or not. 
		/// </summary>
		/// <returns></returns>
		private bool TimeCheck() {
			return (currentlyLooping || (count <= time && count >= 0f));
		}

		protected ProgramaticInterpolation.InterpolationMixer programaticTweenMixer;

		private IEnumerator Tween(float startAt) {
			currentlyLooping = loop;
			currentLoopNumberOfTimes = (int) loopNumberOfTimes;

			eventInvoked = false;

			count = startAt;
			timeAtLastUpdate = Time.time; //initialize time for start
			ProgramaticInterpolation.InterpolationFunc getLerp = interpolationCurve.Evaluate;
			if ( !useCurve ) {
				programaticTweenMixer = new ProgramaticInterpolation.InterpolationMixer(nonCurveInterpolation, nonCurveInterpolationOut, nonCurveLoopMode);
				getLerp = programaticTweenMixer.Tween;
			}

			if ( initBeforeDelay ) {
				LerpParameters(getLerp(count)); //set to start values.
			}

			if ( startDelayWait != null ) {
				yield return startDelayWait;
			}
			int eventFireLoopCount = -1;
			float eventAtTime = eventTime;
			if ( events.atTimeCondition == EventInterface.EventFireCondition.AtReverseTime ) {
				eventAtTime = time - eventTime;
			}
			if ( eventAtTime == 0 && !eventInvoked ) { //invoke time zero events before lerp.
				if ( atTime != null ) {
					atTime.Invoke();
				}
				eventInvoked = true;
				eventFireLoopCount = 0;
			}
			
			if(reverse && events.onCompleteCondition == EventInterface.EventFireCondition.AtReverseTime ) {
				//At reverse time onComplete becomes on Start
				if ( onLoopComplete!= null && onLoopComplete.GetPersistentEventCount() > 0 ) {
					onLoopComplete.Invoke();
				}
			}
			
			if ( !initBeforeDelay ) {
				LerpParameters(getLerp(count)); //set to start values.
			}


			while ( TimeCheck() && enabled ) {
				if ( currentlyLooping && currentLoopNumberOfTimes != -1 && loopCount / time >= currentLoopNumberOfTimes ) {
					break; //stop loop, not coroutine.
				}

				bool doPause = false;
				if ( pauseOffscreen == VisibilityPause.AllChildren && !AnyChildVisible() ) {
					doPause = true;
				} else if ( pauseOffscreen == VisibilityPause.Self && renderer != null && renderer.isVisible == false ) {
					doPause = true;
				}

				if ( doPause ) {
					isPaused = true;
					yield return pauseWait; //wait a little to check again.
					continue; //go back to start of loop.
				}

				isPaused = false;

				float lerp; // =  interpolation.Evaluate(count/time);//getLerp(count/time);
				if ( useCurve ) {
					lerp = interpolationCurve.Evaluate(count / time); //getLerp(count/time);
				} else {
					lerp = getLerp(count / time);
				}

				LerpParameters(lerp); //*** DO THE ACTUAL LERP***
				float timeVal = Time.unscaledDeltaTime;
				if ( playSpeed != UpdateInterface.PlaySpeed.All ) {
					float timeNow = Time.realtimeSinceStartup;
					timeVal = timeNow - timeAtLastUpdate;
					timeAtLastUpdate = timeNow;
				} else if ( (updateType != UpdateType.Update) ) {
					timeVal = Time.fixedDeltaTime;
				}


				float loopIncrement = timeVal * GetTimeScale();
				count += loopIncrement * (reverse ? -1 : 1);
				if ( updateType == UpdateType.GlobalTime ) {
					count = Time.time * (reverse ? -1 : 1);
					if ( loopNumberOfTimes < 0f ) {
						count = count % 2; //2 to account for case of pingpong...
					}
				}

				loopCount += loopIncrement;
				if ( isOkToFireEvent(events.atTimeCondition, reverse) ) {
					float checkTime = loopCount;
					if ( events.atTimeCondition == EventInterface.EventFireCondition.EveryLoop ) {
						checkTime = loopCount % time;
						//if ( reverse && checkTime >= eventAtTime && eventInvoked ) {
						//	eventInvoked = false; //reset invoked.
						//}
						//if ( !reverse && checkTime <= eventAtTime && eventInvoked ) {
						if(eventFireLoopCount != currentLoop){
							eventInvoked = false; //reset invoked.
						}
					}
					if ( reverse ) {
						if ( checkTime <= eventAtTime && !eventInvoked ) {
							if ( atTime != null ) {
								atTime.Invoke();
							}

							eventFireLoopCount = currentLoop;
							eventInvoked = true;
						}
						
					} else {
						if ( checkTime >= eventAtTime && !eventInvoked ) {
							if ( atTime != null ) {
								atTime.Invoke();
							}
							eventInvoked = true;
							eventFireLoopCount = currentLoop;
						}
					}
				}


				if ( currentLoop != Mathf.FloorToInt(count / time) ) { //DetectStart of new loop
					//this is technically the 'start' of a new loop, but can be considered the end for the sake of loop complete.
					if ( events.onCompleteCondition == EventInterface.EventFireCondition.EveryLoop && events.onLoopComplete != null ) {
						if (loopCount==-1 || (int)loopCount != currentLoop + 1 ) {//try and avoid double invoke on limited count loops.
							events.onLoopComplete.Invoke();
						}
					}
					if ( delayEveryLoop && startDelay != 0 ) { //depricate this at some point.
						string seed = useNameAsRandomSeed ? name + currentLoop.ToString() : null;
						float newDelay = MathS.TrulyRandomRange(0, startDelay, seed);
						yield return new WaitForSeconds(newDelay);
						//Debug.Log(this.gameObject.name + " : " + newDelay);
						timeAtLastUpdate = Time.realtimeSinceStartup;
					} else if ( delayEveryLoop && randomStartDelay.x != 0 && randomStartDelay.y != 0 ) {
						string seed = useNameAsRandomSeed ? name + currentLoop.ToString() : null;
						float newDelay = MathS.TrulyRandomRange(randomStartDelay.x, randomStartDelay.y, seed);
						yield return new WaitForSeconds(newDelay);
						timeAtLastUpdate = Time.realtimeSinceStartup;
					}

					currentLoop = Mathf.FloorToInt(count / time);
					
				}

				if ( updateType == UpdateType.Update ) {
					yield return setWait;
				} else {
					yield return fixedWait;
				}
			}

			if ( allowInterupt && enabled == false ) { //in the case of disabling and we want to stop where we are.
				yield break;
			}

			var start = 0f;
			var end = 1f;
			if ( reverse ) {
				start = 1f;
				end = 0f;
				count = 0;
			} else {
				count = time;
			}

			float lerpVal;
			if ( useCurve ) {
				lerpVal = interpolationCurve.Evaluate(resetToBegining ? start : end); //getLerp(count/time);
			} else {
				lerpVal = getLerp(resetToBegining ? start : end);
			}

			LerpParameters(lerpVal);
			if(isOkToFireEvent(events.onCompleteCondition,reverse) ) {
				if ( onLoopComplete!= null && onLoopComplete.GetPersistentEventCount() > 0 ) {
					onLoopComplete.Invoke();
				}
			}

			enabled = false;
		}

		public static bool isOkToFireEvent(EventInterface.EventFireCondition condition, bool reverseState) {
			switch ( condition ) {
				case EventInterface.EventFireCondition.Always:
					return true;
				case EventInterface.EventFireCondition.ForwardOnly:
					return !reverseState;
				case EventInterface.EventFireCondition.ReverseOnly:
					return reverseState;
				case EventInterface.EventFireCondition.AtReverseTime:
					return reverseState;
				case EventInterface.EventFireCondition.EveryLoop:
					return true;
#if UNITY_2018_1_OR_NEWER
				default:
					throw new ArgumentOutOfRangeException(nameof(condition), condition, null);
#endif
			}
			return true;
		}
		
		private IEnumerator RestartCoroutine() {
			enabled = false;
			yield return fixedWait;
			enabled = true;
		}

		private IEnumerator RestartPlay() {
			enabled = false;
			yield return fixedWait;
			count = 0;
			loopCount = 0;
			DoTween();
			enabled = true;
		}

		public enum TweenCancelType {
			/// <summary>
			/// same as .enable = false;
			/// </summary>
			Finish,
			/// <summary>
			/// Stop playback, end tween, fire unfired events
			/// </summary>
			SoftStop,
			/// <summary>
			/// Cancel playback, reset to start, don't fire events.
			/// </summary>
			HardStop,
			/// <summary>
			/// play out the rest of the current loop, then quit
			/// </summary>
			CancelAtLoopEnd,
		};

		/// <summary>
		/// Cancel tween. basic way equivalent to .enabled = false;
		/// </summary>
		public void CancelTween() {
			CancelTween(TweenCancelType.Finish);
		}

		/// <summary>
		/// Useful for Events. Cancel either Hard  or Soft stop
		/// </summary>
		/// <param name="hardStop">stop end events from firing</param>
		public void CancelTween(bool hardStop) {
			if ( hardStop ) {
				CancelTween(TweenCancelType.HardStop);
			} else {
				CancelTween(TweenCancelType.SoftStop);
			}
		}

		/// <summary>
		/// Cancel tween by one of a variety of methods. 
		/// </summary>
		/// <param name="type"></param>
		public void CancelTween(TweenCancelType type) {
			switch ( type ) {
				case TweenCancelType.Finish: //let things finish naturally
					isPaused = false;
					enabled = false; //will stop at current frame and play all the end tween events, so may go to last frame or not depending on settings. will fire end events.
					break;
				case TweenCancelType.SoftStop: //cancel. depend on settings to reset position etc. end Events force fire.
					isPaused = false;
					enabled = false; //Stops loop on next tick
					if ( tweenCoroutine != null ) {
						StopCoroutine(tweenCoroutine); //forcing end anyhoo
					}

					if ( resetToBegining ) { //force move back to beginning and reset count 
						LerpParameters(0f);
						count = 0f;
					} else {
						LerpParameters(1f);
						count = 1;
					}

					if ( onLoopComplete!= null && onLoopComplete.GetPersistentEventCount() > 0 ) { //manually fire end event.
						if(isOkToFireEvent(events.onCompleteCondition,reverse) ) {
							onLoopComplete.Invoke();
						}
					}

					break;
				case TweenCancelType.HardStop: //force to stop where is. no end events.
					//No matter what force back to frame 0, reset count
					isPaused = false;
					enabled = false;
					if ( tweenCoroutine != null ) {
						StopCoroutine(tweenCoroutine);
					}

					LerpParameters(0f);
					count = 0f;
					break;
				case TweenCancelType.CancelAtLoopEnd: //finish current loop and end.
					currentLoopNumberOfTimes = Mathf.CeilToInt(count); //this loop count will remain at this value on nect play...
					break;
				default: //this shouldn't ever happen
					Debug.LogWarning("Poorly added tween cancel state? don't use this...");
					enabled = false;
					if ( tweenCoroutine != null ) {
						StopCoroutine(tweenCoroutine);
					}

					break;
			}
		}

		/// <summary>
		/// used to start tween by script. Will restart if already running.
		/// </summary>
		protected void DoTween() {
			if ( enabled && gameObject.activeInHierarchy ) { //if we're already going, start over.
				StartCoroutine(RestartCoroutine());
			} else { //otherwise enable.
				enabled = true;
			}
		}

		/// <summary>
		/// Plays the tween from current settings
		/// </summary>
		public void PlayFromCurrentState() {
			if ( !gameObject.activeInHierarchy ) {
				//Debug.LogWarning("Can't play on inactive object");
				return;
			}

			if ( enabled ) {
				StartCoroutine(RestartPlay());
			} else {
				initializeCountOnEnable = !reverse; //in case was reversed previously//set this to respect input start time parameter on enable
				count = reverse ? time : 0;
				loopCount = 0;
				DoTween();
			}
		}

		/// <summary>
		/// Initiates play of tween from 0.
		/// </summary>
		public void PlayForwards() {
			if ( !gameObject.activeInHierarchy ) {
				//Debug.LogWarning("Can't play on inactive object");
				return;
			}

			if ( enabled ) {
				StartCoroutine(RestartPlay());
			} else {
				reverse = false;
				initializeCountOnEnable = true; //in case was reversed previously
				count = 0;
				loopCount = 0;
				DoTween();
			}
		}

		public void PlayForwards(bool cancelActive) {
			if ( tweenCoroutine != null && cancelActive ) {
				CancelTween(true);
			}

			PlayForwards();
		}

		/// <summary>
		/// Sets the count to 0. if coroutine is running this should move the animation back to beginning.
		/// Doesn't re-enable events which may have already fired.
		/// </summary>
		public void ResetToStart() {
			count = 0f;
		}

		/// <summary>
		/// Play the tween backwards, starting at time length of tween.
		/// </summary>
		public void PlayReverse() {
			PlayReverse(time);
		}

		/// <summary>
		/// Play tween backwards from startTime back to 0. (unless looping)
		/// </summary>
		/// <param name="startTime">time of play in seconds at which to start the tween</param>
		public void PlayReverse(float startTime) {
			reverse = true;
			initializeCountOnEnable = false; //set this to respect input start time parameter on enable
			count = startTime;
			loopCount = 0;
			enabled = true;
		}

		/// <summary>
		/// Play the tween backwards
		/// </summary>
		/// <param name="keepTime">If true will reverse tween from the current time, else will be set to the end</param>
		public void PlayReverse(bool keepTime) {
			if ( enabled && keepTime ) {
				reverse = true;
				return;
			}

			PlayReverse(keepTime ? count : time);
		}

		/// <summary>
		/// Go to this point in the tween Directly. Doesn't animate (or set current time if running)
		/// </summary>
		/// <param name="lerp"> point in lerp to go to [0-1]</param>
		public virtual void SetToLerpPoint(float lerp) {
			//used for editor scripts/
		}

		protected void OnEnable() {
			if ( _updateSettings == null ) {
				this.enabled = false;
				return;//On enable fired on AddComponent before component is initialized
			}
			if ( HasValidParameters() ) {
				if ( initializeCountOnEnable ) { //may have already set start time...
					count = (startAtTime.x % time) / time;
					if ( startAtTime.y >= 0f && startAtTime.x >= 0f ) {
						string seed = useNameAsRandomSeed ? name + currentLoop.ToString() : null;
						float modX = Mathf.Min(startAtTime.x, time);
						float modY = Mathf.Min(startAtTime.y, time);
						count = MathS.TrulyRandomRange(modX, modY, seed);
					}
				}

				tweenCoroutine = StartCoroutine(Tween(count));
			} else {
				Debug.LogWarning("Invalid Tween Parameters on " + gameObject.name); //this.GetPath());
			}
		}

		/// <summary>
		/// Since we already (in some cases) have this stored, expose it to so we can access without another GetComponentCall.
		/// </summary>
		/// <returns></returns>
		public Renderer GetRenderer() {
			return renderer;
		}
#if UNITY_EDITOR
		public bool WarnCurveLooping(TweenBase tween) { //Function used for inspector warning.
			if ( tween.interpolation.useCurve && tween.interpolation.loop ) {
				if ( tween.timeSettings.reverse ) {
					return (tween.interpolation.interpolation.preWrapMode == WrapMode.Clamp ||
					        tween.interpolation.interpolation.preWrapMode == WrapMode.Default ||
					        tween.interpolation.interpolation.preWrapMode == WrapMode.ClampForever);
				}

				return (tween.interpolation.interpolation.postWrapMode == WrapMode.Clamp ||
				        tween.interpolation.interpolation.postWrapMode == WrapMode.Default ||
				        tween.interpolation.interpolation.postWrapMode == WrapMode.ClampForever);
			}

			return false;
		}

		public bool WarningRendererVisibilityCheck(TweenBase tween) {
			if ( Application.isPlaying == false ) {
				return false;
			}

			if ( tween.pauseOffscreen == VisibilityPause.AllChildren && (tween.renderers == null || tween.renderers.Length == 0) ) {
				return true;
			}

			if ( tween.pauseOffscreen == VisibilityPause.Self && tween.renderer == null ) {
				return true;
			}

			return false;
		}
#endif
	}
}