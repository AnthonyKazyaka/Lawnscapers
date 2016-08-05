using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(PuzzleGenerator))]
public class PuzzleGeneratorEditor : Editor
{
    public SerializedProperty Width;
    public SerializedProperty Height;
    
    private void OnEnable()
    {
        Width = serializedObject.FindProperty("Width");
        Height = serializedObject.FindProperty("Height");
    }

	public override void OnInspectorGUI()
    {
        serializedObject.Update();
        //PuzzleGenerator generator = (PuzzleGenerator) target;

        EditorGUILayout.IntSlider(Width, 0, 20, new GUIContent("Width"));
        EditorGUILayout.IntSlider(Height, 0, 20, new GUIContent("Height"));

        //generator.
        //GUILayout.Toolbar()

        serializedObject.ApplyModifiedProperties();
    }
}
