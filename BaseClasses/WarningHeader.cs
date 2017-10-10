using UnityEngine;
using System.Collections;

namespace InspectorTween {

	/// <summary>
	/// Warning header attribute for Editor.
	/// </summary>
	public class WarningHeader : PropertyAttribute  {

		public string name;
		public string color = "white";

		public WarningHeader (string name) {
			this.name = name;
			this.color = "white";
		}
		public WarningHeader (string name, string color) {
			this.name = name;
			this.color = color;
		}
	}
}
