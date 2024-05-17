using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using MyBox;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Timeline;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class PressedButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [field: SerializeField, AutoProperty, MustBeAssigned, InitializationField]
    private Button _button;

    [field: SerializeField]
    private float _pressionMultiplyer = 1f;

    public Button Button => _button;
    public bool IsPressed { get; private set; }

    private bool _isKey;
    
    
    public void OnPointerDown(PointerEventData eventData)
    {
        IsPressed = true;
        _isKey = false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        IsPressed = false;
        _isKey = false;
    }

    private void Update()
    {
        if (InputManager.Actions.ShakingMiniGameInteraction.IsPressed())
        {
            _isKey = true;
        }

        if (_isKey)
        {
            IsPressed = InputManager.Actions.ShakingMiniGameInteraction.IsPressed();
        }
    }
}
