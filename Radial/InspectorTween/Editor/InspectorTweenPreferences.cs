#pragma warning disable CS0618
//Preferences

using UnityEngine;
using UnityEditor;


public class InspectorTweenPreferences : MonoBehaviour {
    public const string MASK_DEFINE = "TWEEN_MASKS_ENABLED";
    // Have we loaded the prefs yet
    private static bool prefsLoaded;

    // The Preferences
    public static bool isDeveloper;
    public static bool defaultToSimpleMode;

    public static bool enableTweenMasks = true;
    // Add preferences section named "My Preferences" to the Preferences Window
    [PreferenceItem("InspectorTween")]
    public static void PreferencesGUI()
    {
        // Load the preferences
        if (!prefsLoaded)
        {
            isDeveloper = EditorPrefs.GetBool("IsDeveloper", false);
            defaultToSimpleMode = EditorPrefs.GetBool("SimpleMode", false);
            enableTweenMasks = EditorPrefs.GetBool("TweenMasks", true);
            prefsLoaded = true;
        }

        // Preferences GUI
        isDeveloper = EditorGUILayout.Toggle("Is Developer", isDeveloper);
        defaultToSimpleMode = EditorGUILayout.Toggle("Default to Simple Mode", defaultToSimpleMode);
        bool maskResult = EditorGUILayout.Toggle("Enable Tween Masks", enableTweenMasks);
        if ( maskResult != enableTweenMasks ) {
            EnableMasks(maskResult);
        }
        enableTweenMasks = maskResult;
        // Save the preferences
        if (GUI.changed) {
            EditorPrefs.SetBool("IsDeveloper", isDeveloper);
            EditorPrefs.SetBool("SimpleMode", defaultToSimpleMode);
            EditorPrefs.SetBool("TweenMasks", enableTweenMasks);
        }
    }

    public static void EnableMasks(bool state) {
        BuildTarget x  = EditorUserBuildSettings.activeBuildTarget;
        //int[] possibleBuildTargetGroups = {1, 4, 7, 13, 14, 19, 21, 25, 26, 27, 28};
        int[] realisticBuildTargetGroups = {1, 4, 7, 19, 21, 27};
        foreach ( int targetGroup in realisticBuildTargetGroups ) {
            string existingDefines = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone);
            if ( state == false ) {
                string removeString = MASK_DEFINE;
                if ( existingDefines.Contains(";") ) {
                    removeString = $";{MASK_DEFINE}";
                }
                existingDefines = existingDefines.Replace(removeString,"");
            } else {
                if ( existingDefines.Contains(MASK_DEFINE) == false ) {
                    existingDefines += $";{MASK_DEFINE}";
                }
            }
            PlayerSettings.SetScriptingDefineSymbolsForGroup((BuildTargetGroup)targetGroup,existingDefines);
        }

    }
}