using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestA : MonoBehaviour
{
    public Transform Target;
    public Transform MirrorTarget;
    public Transform TargetPivot;
    public Transform Jigger;
    public Transform Bottle;
    
    public float Angle;
    public float JiggerToPointDistance;

    public bool Toggle;

    private Vector3 _targetPivot;
    private Vector3 _target;
    private float _length;
    private float _bottleToPivotlength;
    private float _targetToPivotlength;


    private void Awake()
    {
        var position = TargetPivot.position;
        var position1 = Target.position;
        
        _targetPivot = position;
        _target = position1;
        
        _length = (_target - Bottle.position).magnitude;
        _bottleToPivotlength = (_targetPivot - Bottle.position).magnitude;
        _targetToPivotlength = (_targetPivot - _target).magnitude;
    }

    private void Update()
    {
        SetToOrigin();
    }

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

    private void SetToOrigin()
    {
        var bottlePos = Jigger.position + Vector3.up * JiggerToPointDistance;
        var pivot = bottlePos + Quaternion.AngleAxis(Angle, Vector3.forward) * Vector3.down * _bottleToPivotlength;
        var m = GetInverseMatrix(bottlePos, pivot, Angle);

        var targetPos = m.GetPosition() + Vector3.down * _length;

        Debug.DrawLine(m.GetPosition(),bottlePos);
        Debug.DrawLine(m.GetPosition(), targetPos, Color.yellow);

        Target.position = targetPos;
        Target.rotation = m.rotation;
        
        var mm = GetMatrix(targetPos,  targetPos + Vector3.up * _targetToPivotlength, Angle);
        
        MirrorTarget.position = mm.GetPosition();
        MirrorTarget.rotation = mm.rotation;
        
    }
}
