// #Generic
/**
* Base for Radial Games Tween scripts.
*/

using System;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace InspectorTween{
	public abstract class TweenTransform : TweenBase 
	{
		[Space(10)]
		[Tooltip("Leave this as null for current object.")]
		[ConditionalHide("simpleMode",true,true)]
		public Transform targetTransform;

		[Space(10)] [Tooltip("Start is relative from initial transform")]

		private const bool hideElements = true;//Ugh. Just slap this here so that I don't have to make another property drawer.
		//[ContextMenuItem("Set Array 0  to current","MatchStartToCurrent")]
		[ConditionalHide("hideElements",true,true)]
		public bool startRelative;
		[Tooltip("End is relative from initial transform")]
	
		//[ContextMenuItem("Set Array End  to current", "MatchEndToCurrent")]
		[ConditionalHide("hideElements",true,true)]
		public bool EndIsRelativeOffsetFromStart;
		
		public enum TransformValueMode{AbsoluteLocal=0,RelativeToStart=1,ManualDebugSet=1000}

		public TransformValueMode tranformMode = TransformValueMode.AbsoluteLocal;
		
		
		
		
		[ConditionalHide("simpleMode",true,true)]
		public Vector3 addRandomToTargets = new Vector3(-1,-1,-1);
		protected int randomSetAtLoop = -1;

		protected bool doRandomOffset;
		protected Vector3[] oldRandomOffsets;
		protected Vector3[] newRandomOffsets;
		protected Vector3 randomValue = Vector3.zero;
		protected Vector3[] reversedValues;
		//private Vector3[] tweenValues;
		public abstract Vector3[] values { get; set; }
		public abstract void SetInitial();

		protected override void Awake() {
			switch ( tranformMode ) {
				case TransformValueMode.AbsoluteLocal:
					EndIsRelativeOffsetFromStart = false;
					startRelative = false;
					break;
				case TransformValueMode.RelativeToStart:
					EndIsRelativeOffsetFromStart = true;
					startRelative = true;
					break;

				case TransformValueMode.ManualDebugSet:
					//Do nothing, respecting manually set paramters.
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
			base.Awake();
			if (targetTransform == null) {
				targetTransform = this.transform;
			}
		}

		protected void CacheReversedTweenValues(Vector3[] values) {
			reversedValues = new Vector3[values.Length];
			int lengthMinusOne = values.Length - 1;
			for ( int i = 0; i < values.Length; i++ ) {
				reversedValues[lengthMinusOne - i] = values[i];
			}
		}
		/// <summary>
		/// Set which transform this tween is affecting. Initialized initial transform.
		/// </summary>
		/// <param name="newTarget"></param>
		public void SetTargetTransform(Transform newTarget) {
			if ( !newTarget ) {
				return;
			}
			targetTransform = newTarget;
			SetInitial();//make sure initial position is reset correctly to match the new target
		}
		protected void SetRandom(int tweenArrLength){
			//Determine if randomness is needed
			if (addRandomToTargets.x < 0 && addRandomToTargets.y < 0f && addRandomToTargets.z < 0f) {
				doRandomOffset = false;//nope.
				return;
            }
			doRandomOffset = true;//it is!
			
			if(currentLoop == randomSetAtLoop){//we've already set up for this loop.
				return;
			}
			randomSetAtLoop = currentLoop;

			if (newRandomOffsets == null || newRandomOffsets.Length != tweenArrLength) {//Initialize
				newRandomOffsets = new Vector3[tweenArrLength];
				oldRandomOffsets = new Vector3[tweenArrLength];
				for (int i = 0; i < tweenArrLength; i++) {//Starting points are base
					oldRandomOffsets[i] = Vector3.zero;
				}
			} else {
				for ( int i = 0; i < oldRandomOffsets.Length; i++ ) {
					oldRandomOffsets[i] = new Vector3(newRandomOffsets[i].x,newRandomOffsets[i].y,newRandomOffsets[i].z);
				}
			}
			
			for (int i=0;i<tweenArrLength;i++){ //New random targets to move to for each 'keyframe'/array value
				var xrand=	addRandomToTargets.x>=0?Random.Range(-addRandomToTargets.x,addRandomToTargets.x):0;
				var yrand = addRandomToTargets.y>=0?Random.Range(-addRandomToTargets.y,addRandomToTargets.y):0;
				var zrand = addRandomToTargets.z>=0?Random.Range(-addRandomToTargets.z,addRandomToTargets.z):0;
				newRandomOffsets[i] = new Vector3(xrand,yrand,zrand);
			}
			
		}
		protected static Vector3 LerpRandomTargets(Vector3[] randomTargets,Vector3[] newRandomTargets,float arrayLerp,float timeLerp) {
			if (randomTargets == null) {
				return Vector3.zero;
			}
			return MathS.Vector3Lerp(LerpArray(randomTargets, arrayLerp), LerpArray(newRandomTargets, arrayLerp), timeLerp);
		}
		
		public static Vector3 LerpVector3Array( Vector3[] vArr,Vector3[] randomTargets,Vector3[] newRandomTargets, float lerp, float timeLerp)
		{
			return LerpArray(vArr,lerp) + LerpRandomTargets(randomTargets, newRandomTargets, lerp, timeLerp);
		}
		
		protected abstract Vector3 GetStartRelative(Vector3 startVal,float lerp);
		protected abstract Vector3 GetEndRelative(Vector3 endVal,float lerp);
		protected abstract Vector3 GetRelative(Vector3 endVal,float lerp);
		/// <summary>
		/// Interpolate an array of Vector3 values
		/// </summary>
		/// <param name="tweenArr">Array of values</param>
		/// <param name="lerp"> lerp will likely be a ~0-1 value from AnimationCurve.Evaluate</param>
		/// <returns></returns>
		protected Vector3 LerpParameter(Vector3[] tweenArr,float lerp)
		{
			if (( useCurve && interpolationCurve.postWrapMode == WrapMode.Clamp )|| (!useCurve && interpolation.nonCurveLoopMode == ProgramaticInterpolation.TweenLoopMode.Clamp)){
				//We don't want random changing ever in this case
				if ( currentLoop == 0 ) {
					SetRandom(tweenArr.Length);
				}
			} else {
				SetRandom(tweenArr.Length);
			}
			
			Vector3 scaleArrayLerp;
			if(tweenArr.Length == 2){
				if (doRandomOffset) {
					if ((useCurve && interpolation.interpolation.postWrapMode == WrapMode.PingPong )||(!useCurve && interpolation.nonCurveLoopMode == ProgramaticInterpolation.TweenLoopMode.PingPong)) {
						if(currentLoop == 0){
							randomValue = MathS.Vector3LerpUnclamped(newRandomOffsets[0], newRandomOffsets[1],lerp);
						} else if ( currentLoop % 2 == 1 ) {
							randomValue = MathS.Vector3LerpUnclamped(newRandomOffsets[0], oldRandomOffsets[1],lerp);
						} else {
							randomValue = MathS.Vector3LerpUnclamped(oldRandomOffsets[0], newRandomOffsets[1],lerp);
						}
					} 
					else{
						// if (!useCurve && interpolation.nonCurveLoopMode == ProgramaticInterpolation.TweenLoopMode.Continuous) {
						if(currentLoop == 0){
							randomValue = MathS.Vector3LerpUnclamped(newRandomOffsets[0], newRandomOffsets[1],lerp);
						} else {
							randomValue = MathS.Vector3LerpUnclamped(oldRandomOffsets[0], newRandomOffsets[1],lerp%1);
						}
					}
					//else {
					//	randomValue = MathS.Vector3LerpUnclamped(oldRandomOffsets[0], newRandomOffsets[1],lerp);
					//}

					//randomValue = MathS.Vector3LerpUnclamped(newRandomOffsets[0], newRandomOffsets[1], lerp);
					//
					//MathS.Vector3Lerp(MathS.Vector3LerpUnclamped(randomOffsets[0], randomOffsets[1], lerp), 
					//MathS.Vector3LerpUnclamped(newRandomOffsets[0], newRandomOffsets[1], lerp), count / timeSettings.time % 1);
				}
                scaleArrayLerp = MathS.Vector3LerpUnclamped(tweenArr[0],tweenArr[1],lerp) + randomValue;
			}else{
				scaleArrayLerp = LerpVector3Array(tweenArr,oldRandomOffsets,newRandomOffsets,lerp, count / timeSettings.time % 1);
			}

			if(EndIsRelativeOffsetFromStart && startRelative){
				scaleArrayLerp = GetRelative(scaleArrayLerp,lerp);
			}else if(EndIsRelativeOffsetFromStart){
				scaleArrayLerp = GetEndRelative(scaleArrayLerp,lerp);//+initial;//Vector3.Scale(scaleArrayLerp,initial);
			}else if(this.startRelative){
				scaleArrayLerp = GetStartRelative(scaleArrayLerp,lerp);//MathS.Vector3Lerp(initial,scaleArrayLerp,lerp);
			}
	
			return scaleArrayLerp;
		}

		/// <summary>
		/// Set to point in current tween
		/// </summary>
		/// <param name="lerp">0-1 value of where along tween to set transform</param>
		private float lastLerpPoint = float.MinValue;
		public override void SetToLerpPoint(float lerp){
			if (Application.isPlaying && lerp == lastLerpPoint ) {
				return;
			}
			lastLerpPoint = lerp;
			this.LerpParameters(lerp);
		}

		public abstract void MatchStartToCurrent();
		public abstract void MatchEndToCurrent();
	}
}