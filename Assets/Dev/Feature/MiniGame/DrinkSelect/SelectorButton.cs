using System;
using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;
using UnityEngine.UI;

public class SelectorButton : MonoBehaviour
{
    [field: SerializeField, AutoProperty, InitializationField, MustBeAssigned]
    private Button _button;

    [field: SerializeField, InitializationField]
    private MiniGameItemData _data;
    public MiniGameItemData Data => _data;
    public Button Button => _button;

    private void Awake()
    {
        _button.image.sprite = Data.Sprite;
    }

    private void OnValidate()
    {
        if (Data == false) return;
        if (Button == false) return;

        Button.image.sprite = Data.Sprite;
    }
}