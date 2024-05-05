using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour, IPlayerStrategy
{
    private InputAction _movementAction;
    
    private Rigidbody _rigidbody;
    private PlayerMovementData _movementData;
    private Animator _animator;
    private static readonly int HashMovement = Animator.StringToHash("F_Movement");

    public void Init(PlayerController controller)
    {
        _movementData = controller.MovementData;
        _rigidbody = controller.Rigidbody;
        _animator = controller.Animator;
        
        BindInputAction();
    }

    private void BindInputAction()
    {
        _movementAction = InputManager.Actions.Movement;
    }
    
    public void Move()
    {
        var input = _movementAction.ReadValue<Vector2>();
        
        Vector3 dir = new Vector3(
            input.x,
            0f,
            input.y
        );

        dir *= _movementData.MovementSpeed;

        _rigidbody.velocity = dir;

        _animator.SetFloat(HashMovement, dir.magnitude);
    }
}
