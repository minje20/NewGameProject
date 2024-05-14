using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;

public class IngredientPosition : MonoBehaviour
{
    [field: SerializeField, AutoProperty, InitializationField]
    private SpriteRenderer _renderer;

    public SpriteRenderer Renderer => _renderer;

    public IngredientData Data { get; set; }
}
