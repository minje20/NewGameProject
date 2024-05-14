using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class MeasurementDrink : MonoBehaviour
{

    private InputAction _keyAction;

    public float _angle;
    public float _backToOriginSpeed;
    public float _measureSpeed;
    public float _jiggerToDistance;
    public float _scoreIncreasementSpeed;
    public float _scoreIncreasementStartAngle;

    public float _visualizeScoreHeightFactor;
    
    public Transform _jigger;
    public Transform _rotationPivot;
    public Transform _bottle;
    public Transform _drink;

    public TMP_Text _text;

    private Vector3 _backupDrinkPos;
    

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
    
    public static (Vector3, Quaternion) GetMeasurementDrinkPosition(
        Vector3 drinkPos,
        Vector3 rotationPivot,
        float angle
        )
    {
        
        var m = GetMatrix(drinkPos, rotationPivot, angle);
        
        return (
            m.GetPosition(),
            m.rotation
        );
    }
    
    private void Awake()
    {
        _keyAction = InputManager.Actions.ShakingMiniGameInteraction;
        _backupDrinkPos = _drink.position;
        
        var originDrinkPos = GetOriginDrinkPosition(
            _jigger.position,
            _jiggerToDistance,
            _angle,
            Vector3.Distance(_bottle.position, _rotationPivot.position),
            Vector3.Distance(_bottle.position, _drink.position)
        );

        _rPos = originDrinkPos;
        _measureMatrix = GetMatrix(_backupDrinkPos, _rotationPivot.position, _angle);
    }

    private Matrix4x4 _measureMatrix;
    private Vector3 _rPos;
    private float _normalizedScore;
    
    private void Update()
    {
        if (_keyAction.IsPressed())
        {
            Measure();

            CalculateScore();
            _text.text = _normalizedScore.ToString();
        }
        else
        {
            BackToOrigin();
        }
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
        
        _drink.position = Vector3.Lerp(_drink.position, pos, _measureSpeed * Time.deltaTime);
        _drink.rotation = Quaternion.Lerp(_drink.rotation, rot, _measureSpeed * Time.deltaTime);
    }

    private void BackToOrigin()
    {
        _drink.position = 
            Vector3.Lerp(_drink.position, _rPos, _backToOriginSpeed * Time.deltaTime);
        _drink.rotation = 
            Quaternion.Lerp(_drink.rotation, Quaternion.identity, _backToOriginSpeed * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawCube(
            _jigger.position + Vector3.up * _normalizedScore * _visualizeScoreHeightFactor * 0.5f,
             Vector3.up * _normalizedScore * _visualizeScoreHeightFactor + new Vector3(0.1f, 0f, 0.1f));
    }
}
