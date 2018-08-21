// #Generic
/**
* Base for Radial Games Tween scripts.
*/
using UnityEngine;
using UnityEngine.Serialization;
namespace InspectorTween{
	public abstract class TweenTransform : TweenBase 
	{
		[Space(10)]
		[Tooltip("Leave this as null for current object.")]
		public Transform targetTransform;
		[Space(10)]
		[Tooltip("Start is relative from initial transform")]
	
		[SerializeField]
		[ContextMenuItem("Set Array 0  to current","MatchStartToCurrent")]
		public bool startRelative = false;
		[Tooltip("End is relative from initial transform")]
		[SerializeField]
		[ContextMenuItem("Set Array End  to current", "MatchEndToCurrent")]
		public bool EndIsRelativeOffsetFromStart = false;
		public Vector3 addRandomToTargets = new Vector3(-1,-1,-1);
		protected int lastRandomTarget = -1;

		protected bool doRandomOffset;
		protected Vector3[] randomTargets;
		protected Vector3[] newRandomTargets;
		protected Vector3 randomValue = Vector3.zero;
		protected Vector3[] reversedValues;
		private Vector3[] tweenValues;
		
		public abstract void SetInitial();

		protected override void Awake() {
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
			if (addRandomToTargets.x < 0 && addRandomToTargets.y < 0f && addRandomToTargets.z < 0f) {
				doRandomOffset = false;
				return;
            }
			doRandomOffset = true;
			var testLerp = Mathf.FloorToInt(this.count/this.time);
			if(testLerp== lastRandomTarget){
				return;
			}
			lastRandomTarget = testLerp;

			if (newRandomTargets == null || newRandomTargets.Length != tweenArrLength) {//Initialize
				newRandomTargets = new Vector3[tweenArrLength];
				randomTargets = new Vector3[tweenArrLength];
				for (int i = 0; i < tweenArrLength; i++) {//Starting points are base
					randomTargets[i] = Vector3.zero;
				}
			} else {
				randomTargets = (Vector3[])newRandomTargets.Clone();
			}
			for (int i=0;i<tweenArrLength;i++){ //New random targets to move to.
				var xrand=	addRandomToTargets.x>=0?Random.Range(-addRandomToTargets.x,addRandomToTargets.x):0;
				var yrand = addRandomToTargets.y>=0?Random.Range(-addRandomToTargets.y,addRandomToTargets.y):0;
				var zrand = addRandomToTargets.z>=0?Random.Range(-addRandomToTargets.z,addRandomToTargets.z):0;
				newRandomTargets[i] = new Vector3(xrand,yrand,zrand);
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
		protected Vector3 LerpParameter(Vector3[] tweenArr,float lerp)
		{
			SetRandom(tweenArr.Length);
			Vector3 scaleArrayLerp;
			if(tweenArr.Length == 2){
				if (doRandomOffset) {
					randomValue = MathS.Vector3Lerp(MathS.Vector3LerpUnclamped(randomTargets[0], randomTargets[1], lerp), MathS.Vector3LerpUnclamped(newRandomTargets[0], newRandomTargets[1], lerp), count / time % 1);
				}
                scaleArrayLerp = MathS.Vector3LerpUnclamped(tweenArr[0],tweenArr[1],lerp) + randomValue;
			}else{
				scaleArrayLerp = TweenTransform.LerpVector3Array(tweenArr,randomTargets,newRandomTargets,lerp, count / time % 1);
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
		
	}
}