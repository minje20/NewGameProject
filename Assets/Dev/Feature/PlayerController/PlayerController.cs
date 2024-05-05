using System;
using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [field: SerializeField, Foldout("데이터"), OverrideLabel("플레이어 이동 데이터"), MustBeAssigned, DisplayInspector]
    private PlayerMovementData _movementData;
    
    [field: SerializeField, Separator("컴포넌트"), MustBeAssigned]
    private Rigidbody _rigidbody;
    
    
    private void Awake()
    {
        
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        var input = InputManager.Actions.Movement.ReadValue<Vector2>();
        
        Vector3 dir = new Vector3(
            input.x,
            0f,
            input.y
        );

        dir *= _movementData.MovementSpeed;

        _rigidbody.velocity = dir;
    }
}
