using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;

[CreateAssetMenu(menuName = "IndieLINY/MiniGame/LiquidPour", fileName = "new liquid pour data")]
public class LiquidPourData : ScriptableObject
{
    [field: SerializeField, OverrideLabel("지거 피벗에서의 오프셋(m)"), Foldout("오프셋")]
    [Tooltip("지거의 피벗에 오프셋된 위치. 오프셋 된 위치에서 액체가 생성됨")]
    private Vector2 _jiggerOffset;
    
    [field: SerializeField, OverrideLabel("액체 생성 라인 길이(m)"), Foldout("오프셋")]
    [Tooltip("'지거 피벗에서의 오프셋(m)'에서 오프셋된 위치에서 이 값만큼 선을 그리고, 액체를 이 선을 따라 생성시킴")]
    private float _liquidCreationLineLength = 0.5f;
    
    [field: SerializeField, OverrideLabel("액체 생성 간격(초)"), Foldout("시간")]
    [Tooltip("한번 액체가 생성되고 다시 한번 생성되기 까지 걸리는 시간")]
    private float _liquidCreationDelay = 0.016f;
    
    [field: SerializeField, OverrideLabel("타임아웃(초)"), Foldout("시간")]
    [Tooltip("해당 시간이 지나면 액체가 모두 생성/파괴가 되지 않아도 종료됨")]
    private float _timeout = 5f;
    
    [field: SerializeField, OverrideLabel("액체 발사 힘"), Foldout("물리")]
    private float _liquidForce = 10f;
    [field: SerializeField, OverrideLabel("액체 발사 방향 각도"), Foldout("물리")]
    private float _liquidForceDirectionAngle = 0f;
    
    [field: SerializeField, OverrideLabel("한번에 생성될 액체 개수"), Foldout("기타")]
    [Tooltip("'액체 생성 라인 길이(m)'에서 정의된 선을 따라 생성될 액체의 개수")]
    private int _countOfOLineCreatingLiquid = 4;

    public Vector2 JiggerOffset => _jiggerOffset;

    public float LiquidCreationLineLength => _liquidCreationLineLength;

    public float LiquidCreationDelay => _liquidCreationDelay;

    public float Timeout => _timeout;

    public float LiquidForce => _liquidForce;

    public float LiquidForceDirectionAngle => _liquidForceDirectionAngle;

    public int CountOfOLineCreatingLiquid => _countOfOLineCreatingLiquid;
}
