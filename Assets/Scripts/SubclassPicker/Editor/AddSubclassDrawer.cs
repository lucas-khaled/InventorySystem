using UnityEditor;
using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

[CustomPropertyDrawer(typeof(AddSubclassAttribute))]
public class AddSubclassDrawer : PropertyDrawer
{
    private List<Type> _subclassTypes;
    private string[] _typeNames;
    private int _selectedIndex = -1;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        AddSubclassAttribute picker = (AddSubclassAttribute)attribute;

        if (_subclassTypes == null)
        {
            _subclassTypes = GetAllSubclassTypes(picker.BaseType);
            _typeNames = _subclassTypes.Select(t => t.Name).ToArray();
        }

        MonoBehaviour mb = property.serializedObject.targetObject as MonoBehaviour;
        if (mb == null)
        {
            EditorGUI.LabelField(position, label.text, "Use apenas em MonoBehaviours");
            return;
        }

        MonoBehaviour current = mb.GetComponent(picker.BaseType) as MonoBehaviour;
        Type currentType = current != null ? current.GetType() : null;

        _selectedIndex = Mathf.Max(0, _subclassTypes.FindIndex(t => t == currentType));

        EditorGUI.BeginChangeCheck();
        int newIndex = EditorGUI.Popup(position, label.text, _selectedIndex, _typeNames);
        if (EditorGUI.EndChangeCheck())
        {
            Type selectedType = _subclassTypes[newIndex];

            Undo.RegisterCompleteObjectUndo(mb.gameObject, "Change Subclass Component");

            foreach (var comp in mb.GetComponents(picker.BaseType))
            {
                if (Application.isPlaying)
                    GameObject.Destroy(comp);
                else
                    GameObject.DestroyImmediate(comp);
            }

            MonoBehaviour newComponent = mb.gameObject.AddComponent(selectedType) as MonoBehaviour;

            property.objectReferenceValue = newComponent;
            property.serializedObject.ApplyModifiedProperties();

            EditorUtility.SetDirty(mb);
        }
    }

    private List<Type> GetAllSubclassTypes(Type baseType)
    {
        return AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(asm =>
            {
                try { return asm.GetTypes(); }
                catch { return new Type[0]; }
            })
            .Where(t => baseType.IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface && typeof(MonoBehaviour).IsAssignableFrom(t))
            .ToList();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight;
    }
}
