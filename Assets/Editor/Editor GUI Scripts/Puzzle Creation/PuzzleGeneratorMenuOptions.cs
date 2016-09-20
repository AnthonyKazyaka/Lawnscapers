using UnityEngine;
using UnityEditor;
using System.Collections;

public class PuzzleGeneratorMenuOptions : EditorWindow
{
    [MenuItem("Puzzle Generator/Generate a new puzzle")]
    private static void MenuCreatePuzzle()
    {
        GameObject newObject = new GameObject();
        newObject.AddComponent<PuzzleGenerator>();
        newObject.name = newObject.GetComponent<PuzzleGenerator>().GeneratedName;

        newObject.AddComponent<Puzzle>();

        newObject.tag = "Puzzle";

        newObject.GetComponent<PuzzleGenerator>().GeneratePuzzle();
        Undo.RegisterCreatedObjectUndo(newObject, "Create " + newObject.name);
    }


}
