using System.Linq;
using MyBox;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

/// <summary> Sets a background color for game objects in the Hierarchy tab</summary>
[UnityEditor.InitializeOnLoad]
public class HierarchyHighlight
{
    private static HierarchHighlightPreset _preset;

    private const string PATH = "Assets/Dev/Data/Common/HierarchyHighlight/";
    private const string FILE_NAME = "Preset.asset";

    static HierarchyHighlight()
    {
        EditorApplication.hierarchyWindowItemOnGUI += HandleHierarchyWindowItemOnGUI;
    }

    private static void BindPreset()
    {
        if (_preset) return;

        _preset = AssetDatabase.LoadAssetAtPath<HierarchHighlightPreset>($"{PATH}{FILE_NAME}");

        if (_preset == false)
        {
            if (AssetDatabase.IsValidFolder(PATH) == false)
            {
                var str = AssetDatabase.CreateFolder("Assets", PATH);
                Debug.Log(str);
            }
            
            var preset = ScriptableObject.CreateInstance<HierarchHighlightPreset>();
            AssetDatabase.CreateAsset(preset, $"{PATH}{FILE_NAME}");
            _preset = AssetDatabase.LoadAssetAtPath<HierarchHighlightPreset>($"{PATH}{FILE_NAME}");
        }
    }

    private static bool TryGetFilteredPresetItem(Object obj, out HierarchHighlightPresetItem item)
    {
        item = null;
        if (_preset == false)
        {
            return false;
        }

        item = _preset.Items?.FirstOrDefault(x =>
        {
            if (x.UsePrefab)
            {
                if (x.TargetPrefab == null) return false;
                
                var originPrefabA =
                    UnityEditor.PrefabUtility.GetCorrespondingObjectFromOriginalSource(obj) as GameObject;
                var originPrefabB =
                    UnityEditor.PrefabUtility.GetCorrespondingObjectFromOriginalSource(x.TargetPrefab) as GameObject;
                
                if (originPrefabA == null || originPrefabB == null) return false;

                return originPrefabA == originPrefabB;
            }
            else
            {
                return string.IsNullOrEmpty(x.TargetObjectName) == false && x.TargetObjectName == obj.name;
            }
        });
        
        return item != null;
    }

    private static void HandleHierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
    {
        BindPreset();

        if (_preset == false) return;

        var obj = EditorUtility.InstanceIDToObject(instanceID);
        if (obj != null && TryGetFilteredPresetItem(obj, out var item))
        {
            Color backgroundColor = item.BackgroundColor;
            Color textColor = item.TextColor;
            Texture2D texture = item.Icon;
            
            Rect offsetRect = new Rect(selectionRect.position + _preset.RectOffset, selectionRect.size);
            Rect bgRect = new Rect(selectionRect.x, selectionRect.y, selectionRect.width + 50, selectionRect.height);

            EditorGUI.DrawRect(bgRect, backgroundColor);
            EditorGUI.LabelField(offsetRect, obj.name, new GUIStyle()
                {
                    normal = new GUIStyleState() { textColor = textColor },
                    fontStyle = FontStyle.Bold
                }
            );

            if (texture != null)
                EditorGUI.DrawPreviewTexture(
                    new Rect(selectionRect.position, new Vector2(selectionRect.height, selectionRect.height)), texture);
        }
    }
}
#endif