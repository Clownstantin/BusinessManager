#if UNITY_EDITOR
using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace BusinessManager.Core.Editor
{
    [CustomPropertyDrawer(typeof(ModuleInfo))]
    public class ModuleInfoDrawer : PropertyDrawer
    {
        private static Type[] _moduleTypesCache = Array.Empty<Type>();
        private static string[] _moduleTypeNamesCache = Array.Empty<string>();
        private static readonly GUIContent _enabledLabel = new("On");
        private static readonly GUIContent _typeLabel = new("Module");
        private static readonly GUIContent _dataLabel = new("Data");

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            EnsureTypesCache();

            SerializedProperty FindRelative(string name) => property.FindPropertyRelative(name) ?? property.FindPropertyRelative($"<{name}>k__BackingField");

            SerializedProperty enabledProp = FindRelative(nameof(ModuleInfo.IsEnabled));
            SerializedProperty moduleInstanceProp = FindRelative(nameof(ModuleInfo.ModuleInstance));
            SerializedProperty moduleDataProp = FindRelative(nameof(ModuleInfo.ModuleData));

            float spacing = 6f;
            float onWidth = 40f;
            float labelWidth = 52f;

            Rect row = new(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            Rect onRect = new(row.x, row.y, onWidth, row.height);
            enabledProp.boolValue = EditorGUI.ToggleLeft(onRect, _enabledLabel, enabledProp.boolValue);

            Rect moduleLabelRect = new(onRect.xMax + spacing, row.y, labelWidth, row.height);
            EditorGUI.LabelField(moduleLabelRect, _typeLabel);

            IECSModule currentInstance = moduleInstanceProp.managedReferenceValue as IECSModule;
            Type dataType = currentInstance?.ModuleDataType;

            float dataSectionWidth = 0f;
            if (dataType != null && typeof(ScriptableObject).IsAssignableFrom(dataType))
                dataSectionWidth = row.width * 0.45f;

            float popupWidth = Mathf.Max(60f, row.width - (onWidth + spacing + labelWidth + spacing + dataSectionWidth + spacing));
            Rect modulePopupRect = new(moduleLabelRect.xMax + spacing, row.y, popupWidth, row.height);

            int currentIndex = GetCurrentTypeIndex(currentInstance);
            EditorGUI.BeginChangeCheck();
            if (_moduleTypeNamesCache.Length != 0)
            {
                int newIndex = EditorGUI.Popup(modulePopupRect, currentIndex, _moduleTypeNamesCache);
                if (EditorGUI.EndChangeCheck())
                {
                    Type selectedType = _moduleTypesCache[newIndex];
                    object newInstance = Activator.CreateInstance(selectedType);
                    moduleInstanceProp.managedReferenceValue = newInstance;
                    moduleDataProp.objectReferenceValue = null;
                    property.serializedObject.ApplyModifiedProperties();
                    GUI.changed = true;
                }
            }
            else
                EditorGUI.LabelField(modulePopupRect, new GUIContent("No modules found"));

            currentInstance = moduleInstanceProp.managedReferenceValue as IECSModule;
            dataType = currentInstance?.ModuleDataType;

            if (dataType != null && typeof(ScriptableObject).IsAssignableFrom(dataType))
            {
                Rect dataLabelRect = new(row.xMax - (row.width * 0.45f), row.y, 44f, row.height);
                EditorGUI.LabelField(dataLabelRect, _dataLabel);

                Rect dataFieldRect = new(dataLabelRect.xMax + spacing, row.y, row.xMax - (dataLabelRect.xMax + spacing), row.height);
                UnityEngine.Object newData = EditorGUI.ObjectField(dataFieldRect, moduleDataProp.objectReferenceValue, dataType, false);

                if (newData != moduleDataProp.objectReferenceValue)
                    moduleDataProp.objectReferenceValue = newData;
            }
            else
            {
                if (moduleDataProp != null)
                    moduleDataProp.objectReferenceValue = null;
            }

            EditorGUI.EndProperty();
        }

        private static void EnsureTypesCache()
        {
            if (_moduleTypesCache.Length > 0)
                return;

            _moduleTypesCache = AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(a => SafeGetTypes(a))
            .Where(t => typeof(IECSModule).IsAssignableFrom(t) && t.IsClass && !t.IsAbstract)
            .OrderBy(t => t.Name)
            .ToArray();

            if (_moduleTypesCache.Length == 0)
            {
                _moduleTypesCache = Array.Empty<Type>();
                _moduleTypeNamesCache = Array.Empty<string>();
                return;
            }

            string[] names = new string[_moduleTypesCache.Length];
            for (int i = 0; i < _moduleTypesCache.Length; i++)
            {
                Type t = _moduleTypesCache[i];
                names[i] = t.Name;
            }
            _moduleTypeNamesCache = names;
        }

        private static Type[] SafeGetTypes(Assembly a)
        {
            try { return a.GetTypes(); }
            catch (ReflectionTypeLoadException e) { return e.Types.Where(t => t != null).ToArray(); }
        }

        private static int GetCurrentTypeIndex(IECSModule instance)
        {
            if (instance == null || _moduleTypesCache.Length == 0)
                return 0;

            Type type = instance.GetType();
            for (int i = 0; i < _moduleTypesCache.Length; i++)
                if (_moduleTypesCache[i] == type)
                    return i;

            return 0;
        }
    }
}
#endif