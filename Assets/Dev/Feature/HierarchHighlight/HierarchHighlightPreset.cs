using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using MyBox;
using UnityEngine;
using UnityEngine.Serialization;


[Serializable]
public class HierarchHighlightPresetItem
{
    [FormerlySerializedAs("_usePrefabName")] [SerializeField] private bool _usePrefab;
    [field: SerializeField, ConditionalField(nameof(_usePrefab), true)]
    private string _targetObjectName;
    [field: SerializeField, ConditionalField(nameof(_usePrefab)), MustBeAssigned]
    private GameObject _targetPrefab;
    
    [SerializeField] private Color _backgroundColor = Color.white;
    [SerializeField] private Color _textColor = Color.white;
    [SerializeField] private Texture2D _icon = null;

    public string TargetObjectName => _targetObjectName;

    public bool UsePrefab => _usePrefab;

    public GameObject TargetPrefab => _targetPrefab;

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