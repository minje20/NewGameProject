using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;

public class MiniGameItemData : ScriptableObject
{
    [field: SerializeField, Foldout("이미지 정보"), OverrideLabel("병 스프라이트")] 
    private Sprite _sprite;

    public Sprite Sprite => _sprite;
}

public interface INoSelectedData
{
}


[CreateAssetMenu(menuName = "IndieLINY/MiniGame/DirnkData", fileName = "New drink")]
public class DrinkData : MiniGameItemData
{
    [field: SerializeField, Foldout("미니게임 좌표 정보"), OverrideLabel("병 입구 위치(로컬 좌표계)")] 
    private Vector3 _bottlePosition;
    
    [field: SerializeField, Foldout("미니게임 좌표 정보"), OverrideLabel("병 회전 기준 위치(로컬 좌표계)")] 
    private Vector3 _rotationPivotPosition;
    
    [field: SerializeField, Foldout("UI 좌표 정보"), OverrideLabel("병 위치 Offset")] 
    private Vector3 _selectionUiOffset;
    
    [field: SerializeField, Foldout("기타"), OverrideLabel("액체의 머테리얼")] 
    private Material _liquidMaterial;
    
    [field: SerializeField, Foldout("기타"), PropertyName("재료 이름")] 
    private string _name;

    public string Name => _name;
    public Vector3 BottlePosition => _bottlePosition;

    public Material LiquidMaterial => _liquidMaterial;

    public Vector3 RotationPivotPosition => _rotationPivotPosition;

    public Vector2 SelectionUiOffset => _selectionUiOffset;
}
