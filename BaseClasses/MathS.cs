//http://wiki.unity3d.com/index.php/SpeedLerp
using UnityEngine;
using System.Collections.Generic;

public class MathS {
	public static Vector3 GetClosestPoint(List<Vector3> points, Vector3 point) {
		Vector3 closestPoint = Vector3.zero;
		float closestDistance = Mathf.Infinity;
		foreach (Vector3 possiblePoint in points) {
			float distance = Vector3.Distance(point, possiblePoint);
			if (closestDistance == Mathf.Infinity || distance < closestDistance) {
				closestPoint = possiblePoint;
				closestDistance = distance;
			}
		}
		return closestPoint;
	}
	
	public static Vector3 Bezier2(Vector3 start, Vector3 control, Vector3 end, float t) {
		return (((1 - t) * (1 - t)) * start) + (2 * t * (1 - t) * control) + ((t * t) * end);
	}
	
	public static bool RandomChance(float numerator, float denominator) {
		if (float.IsNaN(numerator) || float.IsNaN(denominator) || denominator <= 0) {
			return false;
		}
		return (Random.value < (numerator / denominator));
	}
	
	public static T PickRandom<T>(List<T> list) {
		int index = Mathf.FloorToInt(Random.value * list.Count);
		return list[index];
	}
	
	public static T PickRandom<T>(params T[] list) {
		int index = Mathf.FloorToInt(Random.value * list.Length);
		return list[index];
	}
	public static float Lerp (float from, float to, float value) {
		if (value < 0.0f)
			return from;
		else if (value > 1.0f)
			return to;
		return (to - from) * value + from;
	}
	
	public static float LerpUnclamped (float from, float to, float value) {
		return (1.0f - value)*from + value*to;
	}
	
	public static float InverseLerp (float from, float to, float value) {
		if (from < to) {
			if (value < from)
				return 0.0f;
			else if (value > to)
				return 1.0f;
		}
		else {
			if (value < to)
				return 1.0f;
			else if (value > from)
				return 0.0f;
		}
		return (value - from) / (to - from);
	}
	
	public static float InverseLerpUnclamped (float from, float to, float value) {
		return (value - from) / (to - from);
	}
	
	public static float SmoothStep (float from, float to, float value) {
		if (value < 0.0f)
			return from;
		else if (value > 1.0f)
			return to;
		value = value*value*(3.0f - 2.0f*value);
		return (1.0f - value)*from + value*to;
	}
	
	public static float SmoothStepUnclamped (float from, float to, float value) {
		value = value*value*(3.0f - 2.0f*value);
		return (1.0f - value)*from + value*to;
	}
	
	public static float SuperLerp (float from, float to, float from2, float to2, float value) {
		if (from2 < to2) {
			if (value < from2)
				value = from2;
			else if (value > to2)
				value = to2;
		}
		else {
			if (value < to2)
				value = to2;
			else if (value > from2)
				value = from2;	
		}
		return (to - from) * ((value - from2) / (to2 - from2)) + from;
	}
	
	public static float SuperLerpUnclamped (float from, float to, float from2, float to2, float value) {
		return (to - from) * ((value - from2) / (to2 - from2)) + from;
	}
	
	public static Color ColorLerp (Color c1, Color c2, float value) {
		if (value > 1.0f)
			return c2;
		else if (value < 0.0f)
			return c1;
		return new Color (	c1.r + (c2.r - c1.r)*value, 
		                  c1.g + (c2.g - c1.g)*value, 
		                  c1.b + (c2.b - c1.b)*value, 
		                  c1.a + (c2.a - c1.a)*value );
	}
	public static Color ColorLerpUnclamped (Color c1, Color c2, float value) {
		return new Color (	c1.r + (c2.r - c1.r)*value, 
		                  c1.g + (c2.g - c1.g)*value, 
		                  c1.b + (c2.b - c1.b)*value, 
		                  c1.a + (c2.a - c1.a)*value );
	}
	
	public static Vector2 Vector2Lerp (Vector2 v1, Vector2 v2, float value) {
		if (value > 1.0f)
			return v2;
		else if (value < 0.0f)
			return v1;
		return new Vector2 (v1.x + (v2.x - v1.x)*value, 
		                    v1.y + (v2.y - v1.y)*value );		
	}
	
	public static Vector3 Vector3Lerp (Vector3 v1, Vector3 v2, float value) {
		if (value > 1.0f)
			return v2;
		else if (value < 0.0f)
			return v1;
		return new Vector3 (v1.x + (v2.x - v1.x)*value, 
		                    v1.y + (v2.y - v1.y)*value, 
		                    v1.z + (v2.z - v1.z)*value );
	}
	public static Vector3 Vector3LerpUnclamped (Vector3 from, Vector3 to, float t) {
		return new Vector3 (from.x + (to.x - from.x) * t, from.y + (to.y - from.y) * t, from.z + (to.z - from.z) * t);
		//return v1 + (v2 - v1)*value;
		/*return new Vector3 (v1.x + (v2.x - v1.x)*value, 
		                    v1.y + (v2.y - v1.y)*value, 
		                    v1.z + (v2.z - v1.z)*value );*/
	}
	public static Vector4 Vector4Lerp (Vector4 v1, Vector4 v2, float value) {
		if (value > 1.0f)
			return v2;
		else if (value < 0.0f)
			return v1;
		return new Vector4 (v1.x + (v2.x - v1.x)*value, 
		                    v1.y + (v2.y - v1.y)*value, 
		                    v1.z + (v2.z - v1.z)*value,
		                    v1.w + (v2.w - v1.w)*value );
	}

	//let you square negative numbers and retain the original sign
	public static float SignedPow(float a, float b) {
		if (a < 0) {
			a *= -1;
			a = Mathf.Pow(a, b);
			return a * -1;
		} else {
			return Mathf.Pow(a, b);
		}
	}
}