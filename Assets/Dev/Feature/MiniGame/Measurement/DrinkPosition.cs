using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class DrinkPosition : MonoBehaviour
{
    [field: SerializeField, AutoProperty, InitializationField]
    private SpriteRenderer _renderer;


    public SpriteRenderer Renderer => _renderer;

    public DrinkData Data { get; set; }
}
