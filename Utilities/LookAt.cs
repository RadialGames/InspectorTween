using UnityEngine;
using System.Collections;

public class LookAt : MonoBehaviour {
	public enum worldUpVectors { up, down, left, right, forward, backward, specified };
	public enum referenceSpace { world, root, parent, target };

	[Header("New mode just set .forward etc based on lookAxis")]
	public bool newMode = false;
	public worldUpVectors lookAxis;

	[Space(10)]
	public referenceSpace space;
	public worldUpVectors up;
	public Transform target;
	public Vector3? position = null;
	private Renderer rend;

	public Transform upTarget;
	public float lookAheadBy;
	[Header("Update Limiter")]
	public bool limitUpdate;
	public float targetFPS = 30;
	protected float nextRenderTime;
	[Header("OnVisible")]
	public bool updateOnlyWhenVisible;

	public bool useLateUpdate;
	[Header("Partial")] public float lookAmount = 1f;
	Vector3 GetWorldUpVector()
	{
		Vector3 rVal = Vector3.zero;
		switch(lookAxis) {
			case(worldUpVectors.up):rVal = Vector3.up;break;
			case(worldUpVectors.down):rVal = -Vector3.up;break;
			case(worldUpVectors.left):rVal = -Vector3.right;break;
			case(worldUpVectors.right):rVal = Vector3.right;break;
			case(worldUpVectors.forward):rVal = Vector3.forward;break;
			case(worldUpVectors.backward):rVal = -Vector3.forward;break;
		}
		return rVal;
	}

	Vector3 GetUpVector(Transform trans)
	{
		Vector3 rVal = Vector3.zero;
		if(trans)
		{
			switch(up){
				case(worldUpVectors.up):rVal = trans.up;break;
				case(worldUpVectors.down):rVal = -trans.up;break;
				case(worldUpVectors.left):rVal = -trans.right;break;
				case(worldUpVectors.right):rVal = trans.right;break;
				case(worldUpVectors.forward):rVal = trans.forward;break;
				case(worldUpVectors.backward):rVal = -trans.forward;break;
				case (worldUpVectors.specified):
					if (upTarget) {
						rVal = (this.transform.position - upTarget.transform.position);
					} else { rVal = Vector3.up; }
					break;

			}
		}
		else{
			rVal = GetWorldUpVector();
		}
		return rVal;
	}

	Vector3 GetUpVector()
	{
		if(space == referenceSpace.world)
		{
			return GetWorldUpVector();
		}
		Transform spaceT=this.transform.parent;
		switch(space)
		{
		case referenceSpace.parent: spaceT = this.transform.parent; break;
		case referenceSpace.root: spaceT = this.transform.root; break;
		case referenceSpace.target : if(target)spaceT = this.target; break;
		};
		return GetUpVector(spaceT);
	}
	void Awake(){
		if (updateOnlyWhenVisible) {
			rend = GetComponent<Renderer>();
		}
	}
	// Update is called once per frame
	Vector3 zeroV = Vector3.zero;

	void DoLookat() {
		if (updateOnlyWhenVisible && rend && !rend.isVisible) {
			return;
		}
		if (limitUpdate && targetFPS != 0) {
			if (Time.unscaledTime > nextRenderTime) {
				nextRenderTime = Time.unscaledTime + (1f / targetFPS);
			} else {
				return;
			}
		}

		Vector3 tpos = zeroV;
		if (target) {
			tpos = target.position + (target.forward * lookAheadBy);
		} else if (position.HasValue) {
			tpos = position.Value;
		}
		if (!newMode) {
			this.transform.LookAt(tpos, GetUpVector());
			
		} else {
			switch (lookAxis) {
				case worldUpVectors.up:
					transform.up = target.position - transform.position;
					break;
				case worldUpVectors.down:
					transform.up = transform.position - target.position;
					break;
				case worldUpVectors.left:
					transform.right = transform.position - target.position;
					break;
				case worldUpVectors.right:
					transform.right = target.position - transform.position;
					break;
				case worldUpVectors.forward:
					transform.forward = target.position - transform.position;
					break;
				case worldUpVectors.backward:
					transform.forward = transform.position - target.position;
					break;
				case worldUpVectors.specified:
					this.transform.LookAt(tpos, GetUpVector());
					break;
				default:
					transform.forward = target.position - transform.position;
					break;
			}
		}

		if ( lookAmount != 1f ) {
			transform.localRotation = Quaternion.Lerp(Quaternion.identity, transform.localRotation, lookAmount);//TODO make this cheap
		}
	}
	void Update() {
		if ( !useLateUpdate ) {
			DoLookat();
		}

	}
	void LateUpdate () {
		if ( useLateUpdate ) {
			DoLookat();
		}
	}


}
