using System;
using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DrinkMeasurementButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [field: SerializeField, AutoProperty, MustBeAssigned, InitializationField]
    private Button _button;

    public Button Button => _button;
    public bool IsPressed { get; private set; }
    public void OnPointerDown(PointerEventData eventData)
    {
        IsPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        IsPressed = false;
    }

    private void Update()
    {
        IsPressed = InputManager.Actions.ShakingMiniGameInteraction.IsPressed();
    }
}
