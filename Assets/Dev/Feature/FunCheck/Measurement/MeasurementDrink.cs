using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class MeasurementDrink : MonoBehaviour
{

    private InputAction _keyAction;

    public float _angle;
    public float _backToOriginSpeed;
    public float _measureSpeed;
    public float _jiggerToDistance;
    public float _scoreIncreasementSpeed;
    public float _scoreIncreasementStartAngle;
    public int _maxCircleCount = 100;
    [FormerlySerializedAs("_maxCircleCreationDelay")] public float _circleCreationDelay = 0.16f;

    public float _visualizeScoreHeightFactor;
    
    public Transform _jigger;
    public Transform _rotationPivot;
    public Transform _bottle;
    public Transform _drink;

    public TMP_Text _text;
    public GameObject _prefab;


    private Queue<GameObject> _objectQueue = new Queue<GameObject>(100);
    

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
        
        var originDrinkPos = GetOriginDrinkPosition(
            _jigger.position,
            _jiggerToDistance,
            _angle,
            Vector3.Distance(_bottle.position, _rotationPivot.position),
            Vector3.Distance(_bottle.position, _drink.position)
        );

        _rPos = originDrinkPos;
        _drink.position = _rPos;
        _measureMatrix = GetMatrix(_rPos, _rotationPivot.position, _angle);
    }

    private Matrix4x4 _measureMatrix;
    private Vector3 _rPos;
    private float _normalizedScore;
    private float _t;
    private float _creationTimer;
    
    private void Update()
    {
        if (_keyAction.IsPressed())
        {
            _t += _measureSpeed * Time.deltaTime;
            
            Measure();
            CalculateScore();
            
            _text.text = _normalizedScore.ToString();
        }
        else
        {
            _t -= _measureSpeed * Time.deltaTime;
            BackToOrigin();
        }
        
        _creationTimer += Time.deltaTime;
        

        if (_t * _angle >= _scoreIncreasementStartAngle)
        {
            if (_creationTimer > _circleCreationDelay)
            {
                CreateCircle();
                _creationTimer = 0f;
            }
        }

        if (_t <= 0f)
        {
            _t = 0f;
        }
        if (_t >= 1f)
        {
            _t = 1f;
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

        obj.transform.position = _bottle.position;
        _objectQueue.Enqueue(obj);
    }
    
    private void CalculateScore()
    {
        float curAngle = _drink.rotation.eulerAngles.z;

        if (curAngle > _scoreIncreasementStartAngle)
        {
            _normalizedScore += _scoreIncreasementSpeed * Time.deltaTime;
        }
    }

    private void Measure()
    {
        Vector3 pos = _measureMatrix.GetPosition();
        Quaternion rot = _measureMatrix.rotation;
        
        _drink.position = Vector3.Lerp(_rPos, pos, _t);
        _drink.rotation = Quaternion.Lerp(Quaternion.identity, rot, _t);
    }

    private void BackToOrigin()
    {
        Vector3 pos = _measureMatrix.GetPosition();
        Quaternion rot = _measureMatrix.rotation;
        
        _drink.position = Vector3.Lerp(_rPos, pos, _t);
        _drink.rotation = Quaternion.Lerp(Quaternion.identity, rot, _t);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawCube(
            _jigger.position + Vector3.up * _normalizedScore * _visualizeScoreHeightFactor * 0.5f,
             Vector3.up * _normalizedScore * _visualizeScoreHeightFactor + new Vector3(0.1f, 0f, 0.1f));
    }
}
