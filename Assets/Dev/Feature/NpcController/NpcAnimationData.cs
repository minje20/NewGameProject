using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;

[CreateAssetMenu(menuName = "IndieLINY/Npc/Animation Data", fileName = "New animation data")]
public class NpcAnimationData : ScriptableObject
{
    [field: SerializeField, Foldout("속도"), OverrideLabel("페이드아웃 속도(초)")]
    private float _fadeoutSpeed = 0.5f;
    [field: SerializeField, Foldout("속도"), OverrideLabel("페이드인 속도(초)")]
    private float _fadeinSpeed = 0.5f;
    [field: SerializeField, Foldout("속도"), OverrideLabel("Npc 이동 속도(초)")]
    private float _npcMoveToSlotSpeed = 1f;
    
    [field: SerializeField, Foldout("색상"), OverrideLabel("페이드아웃 색상")]
    private Color _fadeoutColor = Color.black;
    [field: SerializeField, Foldout("색상"), OverrideLabel("페이드인 색상")]
    private Color _fadeinColor = Color.white;

    public float FadeoutSpeed => _fadeoutSpeed;

    public float NpcMoveToSlotSpeed => _npcMoveToSlotSpeed;

    public float FadeinSpeed => _fadeinSpeed;
    public Color FadeoutColor => _fadeoutColor;
    public Color FadeinColor => _fadeinColor;

    public static NpcAnimationData CreateErrorData()
    {
        var data = ScriptableObject.CreateInstance<NpcAnimationData>();

        return data;
    }
}
