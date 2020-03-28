// #Generic

using System;
using UnityEngine;
namespace InspectorTween{
	[AddComponentMenu("InspectorTween/TweenRotation",2)]
	[HelpURL("https://github.com/RadialGames/InspectorTween/wiki/TweenRotation")]
	public class TweenRotation : TweenTransform
	{
		public enum rotationTypes {Euler,Slerp,Lerp};
		[SerializeField]private rotationTypes rotationType;

		public rotationTypes RotationType {
			set {
				rotationType = value;
				
			}
		}

		[SerializeField]private Vector3[] moveRotations = {new Vector3(0,0,-180),new Vector3(0,0,180)};
		private Quaternion intitalRotation;
		private Quaternion[] rotationList = new Quaternion[2];
		private Quaternion[] randomRotations= new Quaternion[2];
		private Quaternion[] newRandomRotations= new Quaternion[2];
		private bool useRandomOffset;
		[NonSerialized]private Quaternion[] reversedQuaternions= new Quaternion[2];
		public override Vector3[] values {
			get { return moveRotations; }
			set { moveRotations = value; }
		}

		protected override void Awake()
		{
			base.Awake();
			SetInitial();
		}

		protected void Start() {
			rotationList = new Quaternion[moveRotations.Length];
			for(int i = 0; i<moveRotations.Length;i++){
				rotationList[i] = Quaternion.Euler(moveRotations[i]);
			}
			if(this.addRandomToTargets == Vector3.one * -1f){
				useRandomOffset = false;
			} else{
				useRandomOffset = true;
			}
			
			CacheReversedTweenValues(moveRotations);
			reversedQuaternions = new Quaternion[rotationList.Length];
			int lengthMinusOne = rotationList.Length - 1;
			for ( int i = 0; i < rotationList.Length; i++ ) {
				reversedQuaternions[lengthMinusOne - i] = rotationList[i];
			}			
		}

		public override void MatchStartToCurrent() {//Used by Context menu
			moveRotations[0] = this.transform.localRotation.eulerAngles;
		}
		public override void MatchEndToCurrent() {//Used by Context menu
			moveRotations[moveRotations.Length-1] = this.transform.localRotation.eulerAngles;
		}

		public override void SetInitial() {
			if (!targetTransform) {
				targetTransform = transform;
			}
			intitalRotation = targetTransform.localRotation;
		}
		protected Quaternion LerpQuaternionArray( Quaternion[] vArr, float lerp)
		{
			if(lerp == 0f){
				return vArr[0];
			}
			int topArrayIndex = vArr.Length-1;
			if(lerp == 1f){
				return vArr[topArrayIndex];
			}
			int toIndex=1;
			int fromIndex=0;
			if(lerp > 0){
				toIndex = Mathf.Min(topArrayIndex,Mathf.CeilToInt(lerp * (topArrayIndex)));
				fromIndex = toIndex -1;
			}
			float sectionLerp = (lerp*topArrayIndex) - fromIndex;

	
			Quaternion val;
			if(rotationType == rotationTypes.Lerp){
				val = Quaternion.Lerp(vArr[fromIndex],vArr[toIndex],sectionLerp);
			}
			else{
				val = Quaternion.Slerp(vArr[fromIndex],vArr[toIndex],sectionLerp);
			}
			return val;
		}

		protected Quaternion LerpParameter(Quaternion[] tweenArr,float lerp)
		{
			if (useRandomOffset){
                var testLerp = Mathf.FloorToInt(this.count/time);
				if (testLerp != randomSetAtLoop) {
					SetRandom(tweenArr.Length);
					if (randomRotations==null || randomRotations.Length != oldRandomOffsets.Length) {
						randomRotations = new Quaternion[oldRandomOffsets.Length];
						newRandomRotations = new Quaternion[oldRandomOffsets.Length];
					}
					for(int i=0;i<oldRandomOffsets.Length;i++){
						randomRotations[i] = Quaternion.Euler(oldRandomOffsets[i]);
						newRandomRotations[i] = Quaternion.Euler(newRandomOffsets[i]);
					}
				}
			}
			Quaternion scaleArrayLerp;
			if(tweenArr.Length == 2){
				if(rotationType == rotationTypes.Lerp){
					scaleArrayLerp = Quaternion.Lerp(tweenArr[0],tweenArr[1],lerp);
				}else{
					scaleArrayLerp = Quaternion.Slerp(tweenArr[0],tweenArr[1],lerp);
				}
			}else{
				scaleArrayLerp = this.LerpQuaternionArray(tweenArr,lerp); //don't use LerpArray as it adds the random and here we need multiply.
			}
			if(EndIsRelativeOffsetFromStart && startRelative){
				scaleArrayLerp = GetRelative(scaleArrayLerp,lerp);
			}else if(EndIsRelativeOffsetFromStart){
				scaleArrayLerp = GetEndRelative(scaleArrayLerp,lerp);//+initial;//Vector3.Scale(scaleArrayLerp,initial);
			}else if(this.startRelative){
				scaleArrayLerp = GetStartRelative(scaleArrayLerp,lerp);//MathS.Vector3Lerp(initial,scaleArrayLerp,lerp);
			}
			if (useRandomOffset) {
				scaleArrayLerp *= Quaternion.Lerp(LerpQuaternionArray(randomRotations, lerp) ,
				                                  LerpQuaternionArray(newRandomRotations, lerp), count / time % 1);
			}
			return (scaleArrayLerp);
		}

        protected override void LerpParameters(float lerp)
        {
			Transform tform = targetTransform != null? targetTransform : transform;
            if (rotationType == rotationTypes.Euler) {
	            if ( timeSettings.reverseValues ) {
		            tform.localRotation = Quaternion.Euler(base.LerpParameter(this.reversedValues, lerp));
	            } else {
		            tform.localRotation = Quaternion.Euler(base.LerpParameter(this.moveRotations, lerp));
	            }
            }else {
#if UNITY_EDITOR
				if (!Application.isPlaying) {
					rotationList = new Quaternion[moveRotations.Length];
					for (int i = 0; i < moveRotations.Length; i++) {
						rotationList[i] = Quaternion.Euler(moveRotations[i]);
					}
				}
#endif
	            if ( timeSettings.reverseValues ) {
		            tform.localRotation = LerpParameter(this.reversedQuaternions,lerp);
	            } else {
		            tform.localRotation = LerpParameter(this.rotationList,lerp);
	            }

			}
		}



		protected override bool HasValidParameters()
		{
			return moveRotations.Length > 0;
		} 

		protected float FixForClosestRotation(float match,float inRot){
			if(match < 0 && inRot > 180) return inRot -360;
			if(match >180 && inRot < 0) return inRot + 360;
			return inRot;
		}
		protected Vector3 FixForClosestRotation(Vector3 match,Vector3 inRot){
			return new Vector3( FixForClosestRotation(match.x,inRot.x),FixForClosestRotation(match.y,inRot.y),FixForClosestRotation(match.z,inRot.z) );
		}
		protected override Vector3 GetRelative (Vector3 endVal, float lerp)
		{
			return endVal;
		}
		protected Quaternion GetRelative(Quaternion prelerped, float lerp)
		{
			return prelerped*intitalRotation;
		}
		protected Quaternion GetStartRelative(Quaternion prelerped,float lerp)
		{
			return prelerped*Quaternion.Lerp(intitalRotation,Quaternion.identity,lerp);
		}
		protected Quaternion GetEndRelative(Quaternion prelerped,float lerp)
		{
			return GetStartRelative(prelerped,1-lerp);
		}
		protected override Vector3 GetStartRelative(Vector3 prelerped,float lerp){
			return (prelerped + (intitalRotation.eulerAngles*lerp));
		}
		protected override Vector3 GetEndRelative(Vector3 prelerped,float lerp){
			return GetStartRelative(prelerped,1-lerp);
		}
		
		public new TweenRotation SetTime(float val) {
			return (TweenRotation) base.SetTime(val);
		}
		/// <summary>
		/// Set first position in tween values
		/// </summary>
		public TweenRotation SetStartValue(Vector3 val) {
			this.moveRotations[0] = val;
			rotationList[0] = Quaternion.Euler(val);
			return this;
		}
		public TweenRotation SetStartValue(float x,float y,float z) {
			moveRotations[0].x =x;
			moveRotations[0].y =y;
			moveRotations[0].z =z;
			rotationList[0] = Quaternion.Euler(x,y,z);
			return this;
		}
		/// <summary>
		/// Set last position in tween values
		/// </summary>
		public TweenRotation SetEndValue(Vector3 val) {
			moveRotations[moveRotations.Length-1] = val;
			rotationList[moveRotations.Length - 1] = Quaternion.Euler(val);
			return this;
		}
		public TweenRotation SetEndValue(float x,float y,float z) {
			moveRotations[moveRotations.Length-1].x =x;
			moveRotations[moveRotations.Length-1].y =y;
			moveRotations[moveRotations.Length-1].z =z;
			rotationList[moveRotations.Length - 1] = Quaternion.Euler(x,y,z);
			return this;
		}
	}
}
