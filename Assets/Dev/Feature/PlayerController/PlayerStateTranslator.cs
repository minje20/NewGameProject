using System;
using System.Collections;
using System.Collections.Generic;
using MyBox;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerStateTranslator : MonoBehaviour, IPlayerStrategy
{
    public EPlayerControlState PeekState { get; set; }

    public void Init(PlayerController controller)
    {
    }

    public void OnUpdate()
    {
        // builder mode
        if (InputManager.Actions.BuilderMode.triggered)
        {
            if (PeekState != EPlayerControlState.Builder)
            {
                PeekState = EPlayerControlState.Builder;
            }
            else
            {
                PeekState = EPlayerControlState.Normal;
            }
        }
    }
}

public class PlayerStateTranslatorUnit : Unit
{
    private ControlInput _input;
    private ControlOutput _outputSuccess;
    private ControlOutput _outputFailure;

    private ValueInput _state;
    private ValueInput _playerController;
    
    
    protected override void Definition()
    {
        _input = ControlInput("", Update);
        _outputSuccess = ControlOutput("Success");
        _outputFailure = ControlOutput("Failure");

        _state = ValueInput<EPlayerControlState>("State");
        _playerController = ValueInput<PlayerController>("PlayerController");
    }

    private ControlOutput Update(Flow flow)
    {
        var controller = flow.GetValue<PlayerController>(_playerController);
        var translator = controller.Translator;

        if (flow.GetValue<EPlayerControlState>(_state) == translator.PeekState)
        {
            return _outputSuccess;
        }

        return _outputFailure;
    }
}