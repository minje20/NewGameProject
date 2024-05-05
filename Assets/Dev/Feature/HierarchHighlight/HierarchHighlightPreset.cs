using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using MyBox;
using UnityEngine;


[Serializable]
public class HierarchHighlightPresetItem
{
    [SerializeField] private bool _usePrefabName;
    [field: SerializeField, ConditionalField(nameof(_usePrefabName), true)]
    private string _targetObjectName;
    [field: SerializeField, ConditionalField(nameof(_usePrefabName)), MustBeAssigned]
    private GameObject _targetPrefab;
    
    [SerializeField] private Color _backgroundColor = Color.white;
    [SerializeField] private Color _textColor = Color.white;
    [SerializeField] private Texture2D _icon = null;

    public string TargetObjectName
    {
        get
        {
            if (_usePrefabName)
            {
                return _targetPrefab != null ? _targetPrefab.name : string.Empty;
            }
            else
            {
                return _targetObjectName;
            }
        }
    }

    public Color BackgroundColor => _backgroundColor;

    public Color TextColor => _textColor;

    [CanBeNull] public Texture2D Icon => _icon;
}

public class HierarchHighlightPreset : ScriptableObject
{
    [SerializeField] private Vector2 _rectOffset = new Vector2(20, 1);
    [SerializeField] private List<HierarchHighlightPresetItem> _list;

    public IReadOnlyList<HierarchHighlightPresetItem> Items => _list;

    public Vector2 RectOffset => _rectOffset;
}