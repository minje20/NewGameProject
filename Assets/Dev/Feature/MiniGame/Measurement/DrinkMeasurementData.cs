using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "IndieLINY/MiniGame/DrinkMeasurement", fileName = "new drink measurement data")]
public class DrinkMeasurementData : ScriptableObject
{
    private const float MAX_DEGREE = 179.999f;
    private const float MAX_FLOAT = Mathf.Infinity;

    [field: SerializeField, OverrideLabel("계량 제한시간(초)"), Foldout("시간"), MinValue(0f)]
    [Tooltip("플레이어가 계량을 할 수 있는 제한시간")]
    private float _gameDuration = 1f;

    [field: SerializeField, OverrideLabel("병 기울임 복구 속도(초)"), Foldout("시간"), MinValue(0f)]
    [Tooltip("계량 버튼을 누르지 않았을 때, 병이 원래 자리로 돌아가는 속도")]
    private float _backToOriginDuration = 1f;

    [field: SerializeField, OverrideLabel("병 기울임 속도(초)"), Foldout("시간"), MinValue(0f)]
    [Tooltip("계량 버튼을 눌렀을 때, 병이 지정된 각도('병 기울임 각도(degree)')로 회전하는 속도")]
    private float _measurementSpeed = 0.75f;
    
    [field: SerializeField, OverrideLabel("액체 생성 간격 시간(초)"), Foldout("시간"), MinValue(0f)]
    private float _circleCreationDelay = 0.16f;
    
    [field: SerializeField, OverrideLabel("게임 종료 후 병 위치 롤백 시간(초)"), Foldout("시간"),MinValue(0f)]
    [Tooltip("'계량 제한시간'이 끝나고 병이 원래 자리로 돌아가는 속도.\n'병 기울임 복구 속도(초)'와는 별개")]
    private float _endOfRollbackDuration = 1f;
    
    [field: SerializeField, OverrideLabel("게임 시작 후 병 위치 이동 시간(초)"), Foldout("시간"),MinValue(0f)]
    [Tooltip("게임이 시작되고, 병이 계산된 위치에 이동하는 시간(초)")]
    private float _startOfRollbackDuration = 1f;

    [field: SerializeField, OverrideLabel("병 기울임 각도(degree)"), Foldout("각도"), Range(0, MAX_DEGREE)]
    [Tooltip("해당 각도가 플레이어가 최대로 병을 기울였다고 가정한 각도임")]
    private float _angle = 60f;

    [field: SerializeField, OverrideLabel("병 최대 기울임 각도(degree)"), Foldout("각도"), Range(0, MAX_DEGREE)]
    [Tooltip("해당 각도가 플레이어가 최대로 기울일 수 있는 각도임")]
    private float _maxAngle = 60f;


    [field: SerializeField, OverrideLabel("병의 최소 기울기 각도(degree)"), Foldout("각도"), Range(0f, MAX_DEGREE)]
    [Tooltip("계량을 시작했을 때, 기본적인 병의 회전 각도. 이 값이 0일 경우, 병은 수직으로 세워져 있게됨\n'병 기울임 각도(degree)'를 넘지 않도록 유의")]
    private float _defaultAngle = 40f;

    [field: SerializeField, OverrideLabel("액체가 생성되기 시작하는 병 기울임 각도(degree)"), Foldout("각도"), Range(0f, MAX_DEGREE)]
    [Tooltip("병이 해당 각도를 넘어가면, 액체가 생성되기 시작함")]
    private float _liquidCreationStartAngle = 60f;

    [field: SerializeField, OverrideLabel("지거와 병 입구 간의 offset(m)"), Foldout("기타"), MinValue(0f)]
    [Tooltip("병의 입구 피봇 위치와 지거의 피봇 위치와의 offset")]
    private Vector2 _jiggerOffset = Vector2.zero;
    
    [field: SerializeField, OverrideLabel("최대 액체 sphere 생성 개수"), Foldout("기타"), MinValue(0f)]
    private float _maxCircleCount = 500f;
    
    [field: SerializeField, OverrideLabel("계량 선의 정규화 위치"), Foldout("기타"), Range(0f, 1f)]
    private float _normalizedLinePosition = 1f;

    private void OnValidate()
    {
        _maxAngle = Mathf.Clamp(_maxAngle, 0f, _angle);
        _defaultAngle = Mathf.Clamp(_defaultAngle, 0f, _maxAngle);
        
        IsValidation = true;
    }

    public bool IsValidation { get; set; }

    public float MaxAngle => _maxAngle;
    public float StartOfRollbackDuration => _startOfRollbackDuration;

    public Vector2 JiggerOffset => _jiggerOffset;

    public float GameDuration => _gameDuration;

    public float Angle => _angle;

    public float BackToOriginDuration => _backToOriginDuration;

    public float MeasurementSpeed => _measurementSpeed;

    public float LiquidCreationStartAngle => _liquidCreationStartAngle;

    public float DefaultAngle => _defaultAngle;

    public float MaxCircleCount => _maxCircleCount;

    public float CircleCreationDelay => _circleCreationDelay;

    public float EndOfRollbackDuration => _endOfRollbackDuration;

    public float NormalizedLinePosition => _normalizedLinePosition;
}
