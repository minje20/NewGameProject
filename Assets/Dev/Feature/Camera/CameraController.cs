using System;
using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;
using UnityEngine.Events;




public class CameraController : MonoBehaviour
{
    [field: SerializeField, Foldout("비헤비어"), OverrideLabel("Dolly(패스 이동)"), MustBeAssigned, DisplayInspector] 
    private DollyBehaviour _dollyBehaviour;

    public DollyBehaviour DollyBehaviour => _dollyBehaviour;

    private void Awake()
    {
        _dollyBehaviour.Init(this);
    }

    
    // ****
    // used by unity event
    public void MoveDolly(string key)
    {
        DollyBehaviour.Move(key);
    }
}
