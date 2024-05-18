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
    private InputAction _keyAction;

    public float _gameDuration;

    public float GameDuration => _gameDuration;

    public float _angle;
    public float _backToOriginSpeed;
    public float _measureSpeed;
    public float _jiggerToDistance;
    public float _scoreIncreasementSpeed;
    public float _liquidCreationStartAngle;
    public float _defaultAngle;
    public int _maxCircleCount = 100;
    public float _circleCreationDelay = 0.16f;
    public float _endOfRollbackDuration;

    public float _visualizeScoreHeightFactor;

    public DrinkPosition Drink;
    public Transform Jigger;

    public GameObject _prefab;

    public float EndOfRollbackDuration => _endOfRollbackDuration;

    private Queue<GameObject> _objectQueue = new Queue<GameObject>(100);

    public AsyncReactiveProperty<int> LiquidCount { get; private set; } = new(0);
    

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
            _jiggerToDistance,
            _angle,
            Vector3.Distance(Drink.BottleLocalPos, Drink.RotatingPivotLocalPos),
            Vector3.Distance(Drink.BottleWorldPos, Drink.transform.position)
        );

        _rPos = originDrinkPos;
        var back = Drink.transform.position;
        Drink.transform.position = _rPos;
        _measureMatrix = GetMatrix(_rPos, Drink.RotatingPivotWorldPos, _angle);
        Drink.transform.position = back;
        
        return UniTask.WhenAll(
            Drink.transform.DOMove(Vector3.Lerp(_rPos, _measureMatrix.GetPosition(), _defaultAngle / _angle), 1f).AsyncWaitForCompletion().AsUniTask(),
            Drink.transform.DORotateQuaternion(
                Quaternion.Slerp(Quaternion.identity, _measureMatrix.rotation, _defaultAngle / _angle),
                1f).AsyncWaitForCompletion().AsUniTask()
        ).WithCancellation(GlobalCancelation.PlayMode);
    }

    private Matrix4x4 _measureMatrix;
    private Vector3 _rPos;
    private float _normalizedScore;
    private float _t;
    private float _creationTimer;

    public bool Started
    {
        get => enabled;
        set
        {
            if (enabled == value)
                return;

            enabled = value;
        }
    }

    public void Reset()
    {
        while (_objectQueue.Any())
        {
            Destroy(_objectQueue.Dequeue());
        }

        _t = 0f;
        LiquidCount.Value = 0;
    }
    
    private void Update()
    {
        if (_keyAction.IsPressed())
        {
            _t += _measureSpeed * Time.deltaTime;
        }
        else
        {
            _t -= _measureSpeed * Time.deltaTime;
        }

        _t = Mathf.Max(_t, _defaultAngle / _angle);
        _t = Mathf.Min(_t, 1f);
        
        Vector3 pos = _measureMatrix.GetPosition();
        Quaternion rot = _measureMatrix.rotation;
        
        Drink.transform.position = Vector3.Lerp(_rPos, pos, _t);
        Drink.transform.rotation = Quaternion.Slerp(Quaternion.identity, rot, _t);
        
        _creationTimer += Time.deltaTime;
        

        if (_t * _angle >= _liquidCreationStartAngle)
        {
            if (_creationTimer > _circleCreationDelay)
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
        
        while (_objectQueue.Count > _maxCircleCount)
        {
            var deletionObj = _objectQueue.Dequeue();
            Destroy(deletionObj);
        }

        obj.transform.position = Drink.BottleWorldPos;
        _objectQueue.Enqueue(obj);
                
        LiquidCount.Value += 1;
    }
    
    private void CalculateScore()
    {
        float curAngle = Drink.transform.rotation.eulerAngles.z;

        if (curAngle > _liquidCreationStartAngle)
        {
            _normalizedScore += _scoreIncreasementSpeed * Time.deltaTime;
        }
    }

}
