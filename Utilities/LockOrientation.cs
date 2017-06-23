using UnityEngine;
namespace InspectorTween{
	[AddComponentMenu("InspectorTween/Constraints/Lock Orientation",0)]
	[ExecuteInEditMode]
	public class LockOrientation : MonoBehaviour {
		public Vector3 worldRotation;
		private Quaternion _worldRotation;
		void OnEnable () {
			_worldRotation = Quaternion.Euler(worldRotation);
		}
		void LateUpdate () {
			this.transform.rotation = _worldRotation;
		}
	}
}