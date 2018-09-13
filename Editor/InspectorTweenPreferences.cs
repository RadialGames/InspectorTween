//Preferences

using UnityEngine;
using UnityEditor;


public class InspectorTweenPreferences : MonoBehaviour
{
    // Have we loaded the prefs yet
    private static bool prefsLoaded;

    // The Preferences
    public static bool isDeveloper;
    public static bool defaultToSimpleMode;
    
    // Add preferences section named "My Preferences" to the Preferences Window
    [PreferenceItem("InspectorTween")]
    public static void PreferencesGUI()
    {
        // Load the preferences
        if (!prefsLoaded)
        {
            isDeveloper = EditorPrefs.GetBool("IsDeveloper", false);
            defaultToSimpleMode = EditorPrefs.GetBool("SimpleMode", false);
            prefsLoaded = true;
        }

        // Preferences GUI
        isDeveloper = EditorGUILayout.Toggle("Is Developer", isDeveloper);
        defaultToSimpleMode = EditorGUILayout.Toggle("Default to Simple Mode", defaultToSimpleMode);
        // Save the preferences
        if (GUI.changed) {
            EditorPrefs.SetBool("IsDeveloper", isDeveloper);
            EditorPrefs.SetBool("SimpleMode", defaultToSimpleMode);
        }
    }
}