using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal; //Deal with this namespace is a little risky since it's not supported by Unity for User usage.
using System.Reflection; //Reflection should be avoided in real time. Here, it's used only on Editor so It's ok. If you don't know what reflection does, please refer to https://msdn.microsoft.com/en-us/library/f7ykdhsy(v=vs.110).aspx

[CanEditMultipleObjects()] //So you can set multiple layers at the same time.

#if UNITY_5
[CustomEditor(typeof(LineRenderer), true)]
public class SortLayerLineRendererExtension : SortLayerRendererExtension { }

[CustomEditor(typeof(TrailRenderer), true)]
public class SortLayerTrailRendererExtension : SortLayerRendererExtension { }

[CustomEditor(typeof(SkinnedMeshRenderer), true)]
public class SortLayerSkinnedMeshRendererExtension : SortLayerRendererExtension { }

[CustomEditor(typeof(ParticleRenderer), true)]
public class SortLayerParticleRendererExtension : SortLayerRendererExtension { }

[CustomEditor(typeof(MeshRenderer), true)]
public class SortLayerMeshRendererExtension : SortLayerRendererExtension { }
#endif
[CustomEditor(typeof(Renderer), true)] //In Unity 4, we can override almost all renderers at once overriding Renderer class. In Unity 5 you'll need more more specific approuch.
public class SortLayerRendererExtension : Editor
{
    Renderer renderer;
    Renderer[] childsRenderer;
    string[] sortingLayerNames;

    int selectedOption;
    bool applyToChild = false;
    bool applyToChildOldValue = false;

    void OnEnable()
    {
        sortingLayerNames = GetSortingLayerNames();
        renderer = (target as Renderer).gameObject.GetComponent<Renderer>();
        if ((target as Renderer).transform.childCount > 1)
            childsRenderer = (target as Renderer).transform.GetComponentsInChildren<Renderer>();

        for (int i = 0; i < sortingLayerNames.Length; i++)
        {
            if (sortingLayerNames[i] == renderer.sortingLayerName)
                selectedOption = i;
        }
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (!renderer)
        {
            return;
        }

        EditorGUILayout.LabelField("\n");

        selectedOption = EditorGUILayout.Popup("Sorting Layer", selectedOption, sortingLayerNames);
        if (sortingLayerNames[selectedOption] != renderer.sortingLayerName)
        {
            Undo.RecordObject(renderer, "Sorting Layer");
            if (!applyToChild)
                renderer.sortingLayerName = sortingLayerNames[selectedOption];
            else
            {
                for (int i = 0; i < childsRenderer.Length; i++)
                {
                    childsRenderer[i].sortingLayerName = sortingLayerNames[selectedOption];
                }
            }
            EditorUtility.SetDirty(renderer);
        }

        int newSortingLayerOrder = EditorGUILayout.IntField("Order in Layer", renderer.sortingOrder);
        if (newSortingLayerOrder != renderer.sortingOrder)
        {
            Undo.RecordObject(renderer, "Edit Sorting Order");
            renderer.sortingOrder = newSortingLayerOrder;
            EditorUtility.SetDirty(renderer);
        }

        applyToChild = EditorGUILayout.ToggleLeft("Apply to Childs", applyToChild);
        if (applyToChild != applyToChildOldValue)
        {
            for (int i = 0; i < childsRenderer.Length; i++)
            {
                childsRenderer[i].sortingLayerName = sortingLayerNames[selectedOption];
            }
            Undo.RecordObject(renderer, "Apply Sort Mode To Child");
            applyToChildOldValue = applyToChild;
            EditorUtility.SetDirty(renderer);
        }
    }

    // Get the sorting layer names
    public string[] GetSortingLayerNames()
    {
        Type internalEditorUtilityType = typeof(InternalEditorUtility);
        PropertyInfo sortingLayersProperty = internalEditorUtilityType.GetProperty("sortingLayerNames", BindingFlags.Static | BindingFlags.NonPublic);
        return (string[])sortingLayersProperty.GetValue(null, new object[0]);
    }

    // Get the unique sorting layer IDs -- tossed this in for good measure
    public int[] GetSortingLayerUniqueIDs()
    {
        Type internalEditorUtilityType = typeof(InternalEditorUtility);
        PropertyInfo sortingLayerUniqueIDsProperty = internalEditorUtilityType.GetProperty("sortingLayerUniqueIDs", BindingFlags.Static | BindingFlags.NonPublic);
        return (int[])sortingLayerUniqueIDsProperty.GetValue(null, new object[0]);
    }
}