//http://wiki.unity3d.com/index.php/SpeedLerp
using UnityEngine;
using System.Collections.Generic;
namespace InspectorTween{
	public class MathS {
		/// <summary>
		/// Return a number from 0 to 1 based roughly on the given text string, used for pseudo-random numbers.
		/// </summary>
		/// <returns></returns>
		public static float RandomFromString(string text) {
			if (string.IsNullOrEmpty(text)) {
				return 0.5f;
			}

			// sum the values of the characters
			float hashNumber = 0;
			for (int i = 0; i < text.Length; i++) {
				// square and multiply by the position so 1salt2 is different than 2salt1
				hashNumber += (int)text[i] * (int)text[i] * i;
			}

			// wrap the int around a couple times and pseudo-ranomize by multiplying by primes
			float hashPrimed = hashNumber * 9343 * 12157 * 15307;
			float hashPercent = (hashPrimed % 2953) / 2953;

			return hashPercent;
		}

		/// <summary>
		/// Use a seed based on current timestamp to pull a random number from the range.
		/// Unity's Random may produce more predictable results in some circumstances.
		/// </summary>
		/// <param name="min">inclusive</param>
		/// <param name="max">inclusive</param>
		/// <param name="seedString">Not random at all - deterministic based on this string</param>
		/// <returns></returns>
		public static float TrulyRandomRange(float minInclusive, float maxInclusive, string seedString = null) {
			int tickseed = (int)(System.DateTime.Now.Ticks);
			int randseed = tickseed * (int)(Random.value * int.MaxValue);
			int primedseed = randseed * 9343 * 12157 * 15307;
			int seed = primedseed;

			if (!string.IsNullOrEmpty(seedString)) {
				seed = (int)(RandomFromString(seedString) * int.MaxValue);
			}
			var rand = new RandomStateChange(seed);
			float range = Random.Range(minInclusive, maxInclusive);
			rand.Revert();
			return range;
		}
		public static int TrulyRandomRange(int min, int max) {
			return TrulyRandomRange(min, max, null);
		}
		public static int TrulyRandomRange(int min, int max, string seedString) {
			int rand = Mathf.RoundToInt(TrulyRandomRange((float)min, (float)max, seedString));
			return Mathf.Clamp(rand, min, max);
		}



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
			float oneMinusT = 1 - t;
			float s0 = (oneMinusT* oneMinusT);
			float tSquare = t * t;
			float midVal = 2 * t * oneMinusT;
			
			start.x *= s0;
			start.y *= s0;
			start.z *= s0;
			
			start.x += end.x * tSquare;
			start.y += end.y * tSquare;
			start.z += end.z * tSquare;
			
			start.x += control.x * midVal;
			start.y += control.y * midVal;
			start.z += control.z * midVal;
			return start;
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
		public static float Lerp(float from, float to, float value) {
			if (value < 0.0f)
				return from;
			else if (value > 1.0f)
				return to;
			return (to - from) * value + from;
		}

		public static float LerpUnclamped(float from, float to, float value) {
			return (1.0f - value) * from + value * to;
		}

		public static float InverseLerp(float from, float to, float value) {
			if (from < to) {
				if (value < from)
					return 0.0f;
				else if (value > to)
					return 1.0f;
			} else {
				if (value < to)
					return 1.0f;
				else if (value > from)
					return 0.0f;
			}
			return (value - from) / (to - from);
		}

		public static float InverseLerpUnclamped(float from, float to, float value) {
			return (value - from) / (to - from);
		}

		public static float SmoothStep(float from, float to, float value) {
			if (value < 0.0f)
				return from;
			else if (value > 1.0f)
				return to;
			value = value * value * (3.0f - 2.0f * value);
			return (1.0f - value) * from + value * to;
		}

		public static float SmoothStepUnclamped(float from, float to, float value) {
			value = value * value * (3.0f - 2.0f * value);
			return (1.0f - value) * from + value * to;
		}

		public static float SuperLerp(float from, float to, float from2, float to2, float value) {
			if (from2 < to2) {
				if (value < from2)
					value = from2;
				else if (value > to2)
					value = to2;
			} else {
				if (value < to2)
					value = to2;
				else if (value > from2)
					value = from2;
			}
			return (to - from) * ((value - from2) / (to2 - from2)) + from;
		}

		public static float SuperLerpUnclamped(float from, float to, float from2, float to2, float value) {
			return (to - from) * ((value - from2) / (to2 - from2)) + from;
		}

		public static Color ColorLerp(Color c1, Color c2, float value) {
			if (value > 1.0f)
				return c2;
			else if (value < 0.0f)
				return c1;
			return new Color(c1.r + (c2.r - c1.r) * value,
							  c1.g + (c2.g - c1.g) * value,
							  c1.b + (c2.b - c1.b) * value,
							  c1.a + (c2.a - c1.a) * value);
		}
		public static Color ColorLerpUnclamped(Color c1, Color c2, float value) {
			return new Color(c1.r + (c2.r - c1.r) * value,
							  c1.g + (c2.g - c1.g) * value,
							  c1.b + (c2.b - c1.b) * value,
							  c1.a + (c2.a - c1.a) * value);
		}

		public static Vector2 Vector2Lerp(Vector2 v1, Vector2 v2, float value) {
			if (value > 1.0f)
				return v2;
			else if (value < 0.0f)
				return v1;
			return new Vector2(v1.x + (v2.x - v1.x) * value,
								v1.y + (v2.y - v1.y) * value);
		}

		public static Vector3 Vector3Lerp(Vector3 v1, Vector3 v2, float value) {
			if (value > 1.0f)
				return v2;
			else if (value < 0.0f)
				return v1;
			return new Vector3(v1.x + (v2.x - v1.x) * value,
								v1.y + (v2.y - v1.y) * value,
								v1.z + (v2.z - v1.z) * value);
		}
		public static Vector3 Vector3LerpUnclamped(Vector3 from, Vector3 to, float t) {
			return new Vector3(from.x + (to.x - from.x) * t, from.y + (to.y - from.y) * t, from.z + (to.z - from.z) * t);
			//return v1 + (v2 - v1)*value;
			/*return new Vector3 (v1.x + (v2.x - v1.x)*value, 
								v1.y + (v2.y - v1.y)*value, 
								v1.z + (v2.z - v1.z)*value );*/
		}
		public static Vector4 Vector4Lerp(Vector4 v1, Vector4 v2, float value) {
			if (value > 1.0f)
				return v2;
			else if (value < 0.0f)
				return v1;
			return new Vector4(v1.x + (v2.x - v1.x) * value,
								v1.y + (v2.y - v1.y) * value,
								v1.z + (v2.z - v1.z) * value,
								v1.w + (v2.w - v1.w) * value);
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
}