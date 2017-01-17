// #Generic

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
	namespace InspectorTween {
	[AddComponentMenu("InspectorTween/TweenProperty",9)]
	public class TweenProperty : TweenBase {
	
		public string propertyName = "";
		public string methodName = "";
		public bool useIndex;
		public int index;
		[Header("Currently only supports floats")]
		public Vector2 valueStartAndEnd = new Vector2(0,1);
		public Component target;

		protected System.Reflection.PropertyInfo property;
		protected System.Reflection.MethodInfo method;
		protected object[] inValues;
		protected override void LerpParameters(float lerp)
		{
			float value = Mathf.Lerp(valueStartAndEnd.x, valueStartAndEnd.y, lerp);
			if (property != null) {
				property.SetValue(target, value, null);
				//Debug.Log("Set " + propertyName + " to : " + value);
			}
			if(method != null) {
				if (useIndex) {
					inValues[1] = value;
				} else {
					inValues[0] = value;
				}
				method.Invoke(target, inValues);
			}
		}
		protected override bool HasValidParameters()
		{
				return (interpolation.interpolation.length>0 && (property != null || method != null) );
		}
		new void Awake()
		{
			if(target == null) {
				Debug.LogWarning("TweenProperty : " + name + " target is null. Must be assigned for this tween type");
				return;
			}
			if (!string.IsNullOrEmpty(methodName)){
				method = target.GetType().GetMethod(methodName.Trim());
				if (useIndex) {
					inValues = new object[] { index, valueStartAndEnd.x };
				} else {
					inValues = new object[] { valueStartAndEnd.x };
				}
				if(method == null) {
					Debug.LogWarning("TweenProperty : " + name + " method not found: " + method);
				}
			}
			if (!string.IsNullOrEmpty(propertyName)) {
				property = target.GetType().GetProperty(propertyName.Trim());
				if (property == null) { 
					Debug.LogWarning("TweenProperty : " + name + " property not found: " + propertyName);
				}
			}
			renderer = GetComponent<Renderer>();
			base.Awake();
			if (!HasValidParameters()) {
				Debug.LogWarning("Parameter validation error : " + name + " ");
			}
		}
	}
}