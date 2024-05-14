using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using MyBox;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class DrinkMeasurementMiniGame : MonoBehaviour
{
    [field: SerializeField, AutoProperty, InitializationField, MustBeAssigned]
    private DrinkMeasurementButton _button;

    [field: SerializeField, OverrideLabel("기울이기 속도(초)")] 
    private float _measurementDuration;
    [field: SerializeField, OverrideLabel("원상복귀 속도(초)")] 
    private float _rollbackDuration;
    [field: SerializeField, OverrideLabel("최대 기울이기 각도(degree)")] 
    private float _maxAngle;
    [field: SerializeField, OverrideLabel("게임 끝나고 원상복귀 속도(초)")] 
    private float _endOfrollbackDuration = 1f;
    [field: SerializeField, OverrideLabel("병 입구와 Jigger 간의 거리(최대로 기울였을 때)")] 
    private Vector3 _fromJigger = new Vector3(0f, 2f, 0f);

    public float EndOfRollbackDuration => _endOfrollbackDuration;

    public float MaxAngle => _maxAngle;

    public DrinkMeasurementButton Button => _button;

    public Vector3 FromJigger => _fromJigger;

    public AsyncReactiveProperty<float> OnPressed { get; private set; } = new(0f);

    private void Awake()
    {
        
        Button.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (_button.IsPressed)
        {
            OnPressed.Value += Time.deltaTime * _measurementDuration;

            if (OnPressed.Value > 1f)
            {
                OnPressed.Value = 1f;
            }
        }
        else if(OnPressed.Value > 0f)
        {
            OnPressed.Value = 0f;
        }
    }
}
