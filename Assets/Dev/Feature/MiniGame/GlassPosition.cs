using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using MyBox;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class GlassPosition : MonoBehaviour
{
    [field: SerializeField, Foldout("속도"), OverrideLabel("페이드아웃 속도(초)")]
    private float _fadeoutSpeed = 0.5f;
    [field: SerializeField, Foldout("속도"), OverrideLabel("페이드인 속도(초)")]
    private float _fadeinSpeed = 0.5f;
    
    [field: SerializeField, Foldout("색상"), OverrideLabel("페이드아웃 색상")]
    private Color _fadeoutColor = Color.black;
    [field: SerializeField, Foldout("색상"), OverrideLabel("페이드인 색상")]
    private Color _fadeinColor = Color.white;
    
    [field: SerializeField, AutoProperty, InitializationField, MustBeAssigned]
    private SpriteRenderer _renderer;
    
    
    public UniTask AnimateFadein()
    {
        if (_renderer == false) return UniTask.CompletedTask;

        _renderer.color = _fadeoutColor;
        return _renderer.DOColor(_fadeinColor, _fadeinSpeed)
            .AsyncWaitForCompletion().AsUniTask().WithCancellation(GlobalCancelation.PlayMode);
    }
    public UniTask AnimateFadeout()
    {
        if (_renderer == false) return UniTask.CompletedTask;

        _renderer.color = _fadeinColor;
        return _renderer.DOColor(_fadeoutColor, _fadeoutSpeed)
            .AsyncWaitForCompletion().AsUniTask().WithCancellation(GlobalCancelation.PlayMode);
    }
}
