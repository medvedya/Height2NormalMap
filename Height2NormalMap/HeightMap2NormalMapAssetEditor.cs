#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
namespace Height2NormalMap
{
    [CustomEditor(typeof(Height2NormalMapAsset))]
    public class Height2NormalMapAssetEditor : Editor
    {
        Height2NormalMapAsset mapCreator;
        RenderTexture rTex;
        GUIContent texContent;
        private void OnEnable()
        {
            serializedObject.Update();
            mapCreator = this.target as Height2NormalMapAsset;
            mapCreator.RevertEditProperty();
            rTex = new RenderTexture(256, 256, 0);
            EditorUtility.SetDirty(mapCreator);
            serializedObject.ApplyModifiedProperties();
        }
        private void OnDisable()
        {
            if (mapCreator == null) return;
            serializedObject.Update();
            if (!mapCreator.editGenerator.Equals(mapCreator.generator))
            {
                if (EditorUtility.DisplayDialog("Property has changed", "What do you want?", "Apply new property", "Keep old property"))
                {
                    mapCreator.ApplyEditProperty();
                }
                else
                {
                    mapCreator.RevertEditProperty();
                }
            }
            if (mapCreator != null)
            {
                EditorUtility.SetDirty(mapCreator);
            }
            mapCreator.RevertEditProperty();
            serializedObject.ApplyModifiedProperties();
            DestroyImmediate(rTex);

        }
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("editGenerator").FindPropertyRelative("generator"), true);

            EditorGUILayout.PropertyField(serializedObject.FindProperty("editGenerator").FindPropertyRelative("destinationMap"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("editGenerator").FindPropertyRelative("customDestinationWidth"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("editGenerator").FindPropertyRelative("customDestinationHeight"), true);

            EditorGUILayout.PropertyField(serializedObject.FindProperty("showPreview"));


            if (mapCreator.editGenerator.destinationMap == null)
            {
                if (GUILayout.Button("Create destination texture"))
                {
                    string targetPath = AssetDatabase.GetAssetPath(mapCreator).Substring(0, AssetDatabase.GetAssetPath(mapCreator).Length - 5) + "png";
                    mapCreator.editGenerator.destinationMap = mapCreator.editGenerator.CreateTexture(targetPath);
                    EditorUtility.SetDirty(mapCreator);

                }
            }

            EditorGUILayout.BeginHorizontal();
            EditorGUI.BeginDisabledGroup(mapCreator.generator.Equals(mapCreator.editGenerator));
            if (GUILayout.Button("Apply"))
            {
                mapCreator.ApplyEditProperty();
            }
            if (GUILayout.Button("Revert"))
            {
                mapCreator.RevertEditProperty();
            }
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndHorizontal();
            if (mapCreator.showPreview)
            {
                var s = RenderTexture.active;
                mapCreator.GenerateFromEditorGenerator(rTex);
                RenderTexture.active = s;
                GUILayout.Label(rTex);
            }
            serializedObject.ApplyModifiedProperties();

        }
    }
}
#endif
