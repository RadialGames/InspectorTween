// BackgroundColorDecorator.cs  NOTE: need to be inside an editor folder

using UnityEngine;
using UnityEditor;

namespace InspectorTween {
  
    // Custom drawer for the LargeHeader attribute
    [CustomPropertyDrawer (typeof (WarningHeader))]
    public class LargeHeaderDrawer : DecoratorDrawer 
    {
        // Used to calculate the height of the box
        public static Texture2D lineTex = null;
        private GUIStyle style;
		
        WarningHeader largeHeader { get { return ((WarningHeader) attribute); } }

        // Get the height of the element
        public override float GetHeight () 
        {
            return base.GetHeight ()*1.5f;
        }
		
		
        // Override the GUI drawing for this attribute
        public override void OnGUI (Rect pos) 
        {	
            // Get the color the line should be
            Color color = Color.white;
            switch (largeHeader.color.ToString().ToLower())
            {
                case "white": color = Color.white*0.7f; break;
                case "red": color = Color.red*2; break;
                case "blue": color = Color.blue; break;
                case "green": color = Color.green; break;
                case "gray": color = Color.gray; break;
                case "grey": color = Color.grey; break;
                case "black": color = Color.black; break;
                case "yellow" : color = Color.yellow*2; break;
            }

            //color *= 0.7f;

            style = new GUIStyle(GUI.skin.label);
            style.fontSize = 12;
            style.fontStyle = FontStyle.Italic;
            style.alignment = TextAnchor.LowerLeft;
            GUI.color = color;

            Rect labelRect = pos;
            //labelRect.y += 10;
            EditorGUI.LabelField(labelRect, largeHeader.name, style);

            GUI.color = Color.white;
        }
    }
}