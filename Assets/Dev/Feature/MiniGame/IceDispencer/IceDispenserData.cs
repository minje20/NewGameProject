using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;
[CreateAssetMenu(menuName = "IndieLINY/MiniGame/IceDispenser", fileName = "new ice dispenser data")]
public class IceDispenserData : ScriptableObject
{

    [field: SerializeField, OverrideLabel("얼음 생성 간격 시간(초)"), Foldout("시간"), MinValue(0f)]
    private float _creationDelay;
    
    [field: SerializeField, OverrideLabel("계량 제한시간(초)"), Foldout("시간"), MinValue(0f)]
    private float _gameDuration;
    
    [field: SerializeField, OverrideLabel("얼음 생성 스텝간 간격 시간 랜덤범위(초)"), Foldout("랜덤범위")]
    [Tooltip("얼음이 n~m개 생성되고 다음 얼음 생성 때 까지 걸리는 시간. x~y 사이 값")]
    private Vector2 _createStepWaitDurationRange;
    
    [field: SerializeField, OverrideLabel("얼음 생성 개수 랜덤범위"), Foldout("랜덤범위")]
    [Tooltip("한 스탭 당 생성되는 얼음의 랜덤범위. x~y 사이 값")]
    private Vector2Int _createStepIceCountRange;
    
    [field: SerializeField, OverrideLabel("얼음 생성 위치 범위(degree)"), Foldout("각도"), MinValue(0f)]
    [Tooltip("부채꼴 범위에서 얼음의 생성위치가 결정됨")]
    private int _creationFov;
    
    [field: SerializeField, OverrideLabel("얼음이 생성됐을 때 가해질 힘"), Foldout("기타"), MinValue(0f)]
    [Tooltip("'얼음 생성 위치 범위(degree)'에서 결정된 위치와 방향을 바탕으로 힘이 가해져서 얼음이 발사됨")]
    private int _creationForcePower;
    
    [field: SerializeField, OverrideLabel("얼음 생성 -y 축 offset"), Foldout("기타"), MinValue(0f)]
    [Tooltip("얼음 생성 피봇 위치에서 -y 방향으로 해당 값 만큼 떨어져 얼음이 생성됨")]
    private float _creationDistanceFromPosition;
    
    [field: SerializeField, OverrideLabel("크기 다양성(단위: m)"), Foldout("기타"), InitializationField, MustBeAssigned]
    private List<Vector3> _randomSizeVariation = new () { new Vector3(1f, 1f, 1f) };

    public List<Vector3> RandomSizeVariation => _randomSizeVariation;

    public float CreationDelay => _creationDelay;

    public float GameDuration => _gameDuration;

    public Vector2 CreateStepWaitDurationRange => _createStepWaitDurationRange;

    public Vector2Int CreateStepIceCountRange => _createStepIceCountRange;

    public int CreationFov => _creationFov;

    public int CreationForcePower => _creationForcePower;

    public float CreationDistanceFromPosition => _creationDistanceFromPosition;
}
