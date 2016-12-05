using UnityEngine;

namespace InspectorTween
{
	public class RandomStateChange
	{
#if UNITY_5_4
		Random.State oldState;
#else
	int oldState;
#endif

		public RandomStateChange() {
			SaveCurrentState();
		}
		public RandomStateChange(int newSeed) {
			SetSeed(newSeed);
		}
		public RandomStateChange(string newSeed) {
			SetSeed(newSeed);
		}

		public void SaveCurrentState() {
			// Just store the current state, we might be altering it and want to revert to this point.
#if UNITY_5_4
			oldState = Random.state;
#else
		oldState = Random.seed;
#endif
		}

		public void SetSeed(string newSeed) {
			newSeed = Md5Sum(newSeed);
			// This might break some expected behaviours:
			newSeed = newSeed.Substring(0, 8);
			SetSeed(int.Parse(newSeed, System.Globalization.NumberStyles.HexNumber));
		}
		public void SetSeed(int newSeed) {
			SaveCurrentState();
#if UNITY_5_4
			Random.InitState(newSeed);
#else
		Random.seed = newSeed;
#endif
		}

		public void Revert() {
#if UNITY_5_4
			Random.state = oldState;
#else
		Random.seed = oldState;
#endif
		}

		public static string Md5Sum(string strToEncrypt) {
			System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
			byte[] bytes = ue.GetBytes(strToEncrypt);

			// encrypt bytes
			System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
			byte[] hashBytes = md5.ComputeHash(bytes);

			// Convert the encrypted bytes back to a string (base 16)
			string hashString = "";

			for (int i = 0; i < hashBytes.Length; i++) {
				hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
			}

			return hashString.PadLeft(32, '0');
		}
	}
}