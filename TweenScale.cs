// #Generic
/*
Tween Transform scale
*/

using System;
using UnityEngine;
using System.Collections;
	namespace InspectorTween{
	[AddComponentMenu("InspectorTween/TweenScale",3)]

	public class TweenScale : TweenTransform
	{
		[SerializeField]private Vector3[] scales = new Vector3[2]{Vector3.zero,Vector3.one};
		public Vector3? initialScale;
		[Obsolete]
		private bool scaleRelativeEndRelativeToStart;

		protected override void Awake() {
			base.Awake();
			CacheReversedTweenValues(scales);
		}
		public override Vector3[] values {
			get { return scales; }
			set { scales = value; }
		}

		public override void  MatchStartToCurrent() {
			scales[0] = targetTransform.localScale;
		}
		public override void MatchEndToCurrent() {
			scales[scales.Length-1] = targetTransform.localScale;
		}
		public override void SetInitial(){
			if (!targetTransform) {
				targetTransform = transform;
			}
			initialScale = targetTransform.localScale;
		}
		protected override void LerpParameters(float lerp)
		{
			if(!initialScale.HasValue) {
				SetInitial();
			}
			if ( timeSettings.reverseValues) {
				targetTransform.localScale = LerpParameter(reversedValues,lerp);
			} else {
				targetTransform.localScale = LerpParameter(this.scales,lerp);
			}
		}

		protected override bool HasValidParameters()
		{
			return base.HasValidParameters() && scales.Length > 0;
		}
		protected override Vector3 GetRelative (Vector3 lerpedVal, float lerp)
		{
			return Vector3.Scale(initialScale.Value,lerpedVal);
		}
		protected override Vector3 GetStartRelative(Vector3 prelerped,float lerp)//Actually adjusts end point to be relative to this start point?
		{
			Vector3 endRelative = initialScale.Value;
			//if(scaleRelativeEndRelativeToStart && scales.Length > 0 && scales[0].x != 0 && scales[0].y !=0 && scales[0].z != 0){
			//	endRelative = Vector3.Scale(endRelative, new Vector3(1/scales[0].x ,1/scales[0].y ,1/scales[0].z) );
			//}
			return Vector3.Scale(MathS.Vector3Lerp(endRelative,Vector3.one,lerp),prelerped);
		}
		protected override Vector3 GetEndRelative(Vector3 prelerped,float lerp)
		{
			Vector3 endRelative = initialScale.Value;
			//if(scaleRelativeEndRelativeToStart && scales.Length > 0 && scales[0].x != 0 && scales[0].y !=0 && scales[0].z != 0){
			//	endRelative = Vector3.Scale(endRelative, new Vector3(1/scales[0].x ,1/scales[0].y ,1/scales[0].z) );
			//}
			return Vector3.Scale( prelerped,MathS.Vector3Lerp(Vector3.one,endRelative,lerp) );
		}
		public new TweenScale SetTime(float val) {
			return (TweenScale) base.SetTime(val);
		}
		/// <summary>
		/// Set first position in tween values
		/// </summary>
		public TweenScale SetStartValue(Vector3 val) {
			this.scales[0] = val;
			return this;
		}
		public TweenScale SetStartValue(float x,float y,float z) {
			scales[0].x =x;
			scales[0].y =y;
			scales[0].z =z;
			return this;
		}
		/// <summary>
		/// Set last position in tween values
		/// </summary>
		public TweenScale SetEndValue(Vector3 val) {
			scales[scales.Length-1] = val;
			return this;
		}
		public TweenScale SetEndValue(float x,float y,float z) {
			scales[scales.Length-1].x =x;
			scales[scales.Length-1].y =y;
			scales[scales.Length-1].z =z;
			return this;
		}
	}
}
