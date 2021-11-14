using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PyramidBakeSetting))]
public class PyramidBakeInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Create"))
        {
            var shaderGUID = AssetDatabase.FindAssets("PyramidBuilder").FirstOrDefault();

            if (string.IsNullOrEmpty(shaderGUID))
                Debug.LogError("Cannot find compute sheder: PyramidBuilder.compute");
            else
            {
                var shader = AssetDatabase.LoadAssetAtPath<ComputeShader>(AssetDatabase.GUIDToAssetPath(shaderGUID));

                EditorUtility.DisplayProgressBar("Building mesh", "", 0);

                var settings = serializedObject.targetObject as PyramidBakeSetting;
                bool success = PyramidBaker.Run(shader, settings, out var generatedMesh);

                EditorUtility.ClearProgressBar();

                if (success)
                {
                    SaveMesh(generatedMesh);
                    Debug.Log("Mesh saved successfully");
                }
                else
                {
                    Debug.LogError("Failed to create mesh");
                }
            }
        }
    }

    private void SaveMesh(Mesh mesh)
    {
        string path = EditorUtility.SaveFilePanel("Save Mesh Asset", "Assets/", name, "asset");

        if (string.IsNullOrEmpty(path))
            return;

        path = FileUtil.GetProjectRelativePath(path);

        var oldMesh = AssetDatabase.LoadAssetAtPath<Mesh>(path);
        if (oldMesh != null)
        {
            oldMesh.Clear();

            EditorUtility.CopySerialized(mesh, oldMesh);
        }
        else
        {
            AssetDatabase.CreateAsset(mesh, path);
        }

        AssetDatabase.SaveAssets();
    }
}
