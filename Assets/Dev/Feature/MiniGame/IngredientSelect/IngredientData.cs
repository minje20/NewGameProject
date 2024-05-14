using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;

[CreateAssetMenu(menuName = "IndieLINY/MiniGame/IngredientData", fileName = "New ingredient")]
public class IngredientData : MiniGameItemData
{
    [field: SerializeField, AutoProperty, InitializationField]
    private SpriteRenderer _renderer;

    private DrinkData _data;

    public SpriteRenderer Renderer => _renderer;

    public DrinkData Data
    {
        get => _data;
        set
        {
            _data = value;
            _renderer.sprite = value.Sprite;
        }
    }
}
