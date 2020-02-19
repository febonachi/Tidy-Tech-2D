using UnityEngine;
using UnityEditor;

//[CustomEditor(typeof(Maze))]
public class MazeEditor : Editor {
    #region public
    public override void OnInspectorGUI() {
        Maze generator = (Maze)target;
        /*
        generator.dimension = EditorGUILayout.Vector2IntField("Dimension", generator.dimension);

        generator.cellPrefab = (MazeCell)EditorGUILayout.ObjectField("Cell Prefab", generator.cellPrefab, typeof(MazeCell), true);

        if (GUILayout.Button("Generate")) generator.generate();
        if (GUILayout.Button("Clear")) generator.clear();

        EditorUtility.SetDirty(generator);
        */
    }
    #endregion
}
