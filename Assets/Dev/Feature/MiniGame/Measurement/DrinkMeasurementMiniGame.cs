using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using MyBox;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;


public class DrinkMeasurementMiniGame : MonoBehaviour
{
    #region Inspector
    [field: SerializeField, OverrideLabel("계량 미니게임 데이터"), Foldout("데이터"), InitializationField, MustBeAssigned]
    private DrinkMeasurementData _data;
    
    [field: SerializeField, Foldout("컴포넌트(건들지 마시오)"), InitializationField, MustBeAssigned]
    private MiniGameCircleTimer _circleTimer;
    
    [field: SerializeField, Foldout("컴포넌트(건들지 마시오)"), InitializationField, MustBeAssigned]
    private HUDController _moneyHud;
    
    [field: SerializeField, Foldout("컴포넌트(건들지 마시오)"), InitializationField, MustBeAssigned]
    private GameObject _prefab;
    #endregion


    #region Getter/Setter
    public AsyncReactiveProperty<int> LiquidCount { get; private set; } = new(0);
    public DrinkMeasurementData Data => _data;
    public Transform Jigger { get; set; }
    public DrinkPosition Drink { get; set; }
    #endregion


    
    private InputAction _keyAction;
    private Queue<GameObject> _liquidQueue = new (100);

    private Matrix4x4 _measureMatrix;
    private Vector3 _originPos;
    private float _normalizedScore;
    private float _t;
    private float _creationTimer;

    public static Matrix4x4 GetMatrix(Vector3 position, Vector3 pivot, float angle)
    {
        Vector3 vector3 = Quaternion.AngleAxis(angle, Vector3.forward) * (position - pivot);
        position = pivot + vector3;
        
        var m =Matrix4x4.TRS(
            position,
            Quaternion.Euler(0f, 0f, angle),
            Vector3.one
        );

        return m;
    }
    public static Matrix4x4 GetInverseMatrix(Vector3 position, Vector3 pivot, float angle)
    {
        Vector3 vector3 = Quaternion.AngleAxis(-angle, Vector3.forward) * (position - pivot);
        position = pivot + vector3;
        
        var m =Matrix4x4.TRS(
            position,
            Quaternion.identity,
            Vector3.one
        );

        return m;
    }

    public static Vector3 GetOriginDrinkPosition(Vector3 jiggerPos, float jiggerToPointDistance,
        float angle, float bottleToRotationPivotLength, float bottleToPivotLength)
    {
        var bottlePos = jiggerPos + Vector3.up * jiggerToPointDistance;
        var pivot = bottlePos + Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.down * bottleToRotationPivotLength;
        var m = GetInverseMatrix(bottlePos, pivot, angle);

        return m.GetPosition() + Vector3.down * bottleToPivotLength;
    }
    
    private void Awake()
    {
        _keyAction = InputManager.Actions.ShakingMiniGameInteraction;

        Started = false;
        LiquidCount.Value = 0;
    }

    public UniTask Calculate()
    {
        var originDrinkPos = GetOriginDrinkPosition(
            Jigger.position,
            Data.JiggerToDistance,
            Data.Angle,
            Vector3.Distance(Drink.BottleLocalPos, Drink.RotatingPivotLocalPos),
            Vector3.Distance(Drink.BottleWorldPos, Drink.transform.position)
        );

        _originPos = originDrinkPos;
        var back = Drink.transform.position;
        Drink.transform.position = _originPos;
        _measureMatrix = GetMatrix(_originPos, Drink.RotatingPivotWorldPos, Data.Angle);
        Drink.transform.position = back;
        
        return UniTask.WhenAll(
            Drink.transform.DOMove(Vector3.Lerp(_originPos, _measureMatrix.GetPosition(), Data.DefaultAngle / Data.Angle), 1f).AsyncWaitForCompletion().AsUniTask(),
            Drink.transform.DORotateQuaternion(
                Quaternion.Slerp(Quaternion.identity, _measureMatrix.rotation, Data.DefaultAngle / Data.Angle),
                1f).AsyncWaitForCompletion().AsUniTask()
        ).WithCancellation(GlobalCancelation.PlayMode);
    }

    public bool Started
    {
        get => enabled;
        set
        {
            if (enabled == value)
                return;

            enabled = value;
            
            if(enabled)
                _circleTimer.TimerStart(Data.GameDuration);
        }
    }

    public void GameReset()
    {
        while (_liquidQueue.Any())
        {
            Destroy(_liquidQueue.Dequeue());
        }

        _t = 0f;
        LiquidCount.Value = 0;
        
        if(_circleTimer)
            _circleTimer.TimerStop();
    }
    
    private void Update()
    {
        if (_keyAction.IsPressed())
        {
            _t += Data.MeasurementSpeed * Time.deltaTime;
        }
        else
        {
            _t -= Data.BackToOriginDuration * Time.deltaTime;
        }

        _t = Mathf.Max(_t, Data.DefaultAngle / Data.Angle);
        _t = Mathf.Min(_t, 1f);
        
        Vector3 pos = _measureMatrix.GetPosition();
        Quaternion rot = _measureMatrix.rotation;
        
        Drink.transform.position = Vector3.Lerp(_originPos, pos, _t);
        Drink.transform.rotation = Quaternion.Slerp(Quaternion.identity, rot, _t);
        
        _creationTimer += Time.deltaTime;
        

        if (_t * Data.Angle >= Data.LiquidCreationStartAngle)
        {
            if (_creationTimer > Data.CircleCreationDelay)
            {
                CreateCircle();
                _creationTimer = 0f;
            }
        }
    }

    private void CreateCircle()
    {
        var obj = Instantiate(_prefab);
        obj.SetActive(true);
        
        while (_liquidQueue.Count > Data.MaxCircleCount)
        {
            var deletionObj = _liquidQueue.Dequeue();
            Destroy(deletionObj);
        }

        obj.transform.position = Drink.BottleWorldPos;
        _liquidQueue.Enqueue(obj);
        _moneyHud.SetValue( _moneyHud.Value - 10, false);
        LiquidCount.Value += 1;
    }

}
