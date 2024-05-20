using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MyBox;
using UnityEngine;

public class BarController : MonoBehaviour
{
    [field: SerializeField, InitializationField, MustBeAssigned]
    private SpriteRenderer _slot;
    [field: SerializeField, InitializationField, MustBeAssigned]
    private Sprite _sprite;

    private void Awake()
    {
        _slot.gameObject.SetActive(false);
    }

    public void ShowDrink()
    {
        _slot.gameObject.SetActive(true);
        _slot.sprite = _sprite;
        _slot.color = Color.black;
        _slot.DOColor(Color.white, 1f);
    }

    public void HideDrink()
    {
        _slot.gameObject.SetActive(false);
        _slot.color = Color.white;
        _slot.DOColor(Color.black, 1f).OnComplete(() =>
            _slot.sprite = null);
    }
}
