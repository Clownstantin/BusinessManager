#if UNITY_EDITOR
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Core
{
    public class SavesEditor : EditorWindow
    {
        private Rect contentRect;

        [MenuItem("Tools/Saves Editor")]
        private static void OpenWindow()
        {
            var window = GetWindow<SavesEditor>("Saves");
            window.minSize = new Vector2(160f, 62f);
            window.titleContent.image = EditorGUIUtility.IconContent("SaveActive").image;
        }

        private void OnGUI()
        {
            string saveFilename = Strings.SavePath;
            string storedFilename = Strings.StoredPath;
            bool saveFileExists = File.Exists(saveFilename);
            bool storedFileExists = File.Exists(storedFilename);
            var buttonLayout = GUILayout.Width(contentRect.width / 2f);

            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Open folder", EditorStyles.miniButtonLeft, buttonLayout))
                if (saveFileExists)
                    EditorUtility.RevealInFinder(saveFilename);
                else
                    Process.Start(Application.persistentDataPath);

            EditorGUI.BeginDisabledGroup(!saveFileExists);

            if (GUILayout.Button("Open file", EditorStyles.miniButtonRight, buttonLayout))
                Process.Start(saveFilename);

            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUI.BeginDisabledGroup(!saveFileExists);

            if (GUILayout.Button("Store save", EditorStyles.miniButtonLeft, buttonLayout))
                File.Copy(saveFilename, storedFilename, true);

            EditorGUI.EndDisabledGroup();
            EditorGUI.BeginDisabledGroup(!storedFileExists);

            if (GUILayout.Button("Restore save", EditorStyles.miniButtonRight, buttonLayout))
                File.Copy(storedFilename, saveFilename, true);

            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            EditorGUI.BeginDisabledGroup(!saveFileExists);

            if (GUILayout.Button("Delete save", EditorStyles.miniButtonLeft, buttonLayout) &&
               EditorUtility.DisplayDialog("Delete all saves", "Delete all saves?", "Yes", "No"))
                File.Delete(saveFilename);

            EditorGUI.EndDisabledGroup();

            if (GUILayout.Button("Delete prefs", EditorStyles.miniButtonRight, buttonLayout) &&
               EditorUtility.DisplayDialog("Delete PlayerPrefs", "Delete all PlayerPrefs?", "Yes", "No"))
                PlayerPrefs.DeleteAll();

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();

            if (Event.current.type == EventType.Repaint)
                contentRect = GUILayoutUtility.GetLastRect();
        }
    }
}
#endif