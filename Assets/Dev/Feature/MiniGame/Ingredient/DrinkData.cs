using System;
using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;

public class MiniGameItemData : ScriptableObject
{
    [field: SerializeField, Foldout("이미지 정보"), OverrideLabel("닫힌 병 스프라이트")] 
    private Sprite _sprite;
    [field: SerializeField, Foldout("이미지 정보"), OverrideLabel("열린 병 스프라이트")] 
    private Sprite _openedSprite;

    public Sprite Sprite => _sprite;

    public Sprite OpenedSprite
    {
        get
        {
            if (_openedSprite) return _openedSprite;

            return _sprite;
        }
    }
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
    
    [field: SerializeField, Foldout("기타"), OverrideLabel("액체의 머테리얼")] 
    private Material _liquidMaterial;
    
    [field: SerializeField, Foldout("기타"), PropertyName("재료 이름")] 
    private string _name;

    public string Name => _name;
    public Vector3 BottlePosition => _bottlePosition;

    public Material LiquidMaterial => _liquidMaterial;

    public Vector3 RotationPivotPosition => _rotationPivotPosition;


    public void EDITOR_SetPositions(Vector3 rotationPivot, Vector3 bottlePosition)
    {
        _rotationPivotPosition = rotationPivot;
        _bottlePosition = bottlePosition;
    }
}
