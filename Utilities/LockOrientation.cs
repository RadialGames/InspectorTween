using UnityEngine;
using System.Collections;
namespace InspectorTween{
[AddComponentMenu("InspectorTween/Constraints/lockorientation",0)]
[ExecuteInEditMode]

public class LockOrientation : MonoBehaviour {
	/*public bool lock_x;
	public bool lock_y;
	public bool lock_z;*/
	public Vector3 worldRotation;
	private Quaternion _worldRotation;
	// Use this for initialization
	void OnEnable () {
		_worldRotation = Quaternion.Euler(worldRotation);
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.rotation = _worldRotation;
	}
}
}