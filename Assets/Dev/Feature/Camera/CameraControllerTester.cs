using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;

public class CameraControllerTester : MonoBehaviour
{
    [field: SerializeField, AutoProperty(AutoPropertyMode.Scene), InitializationField, MustBeAssigned]
    private CameraController _controller;

    [SerializeField] private string _dollyKey;
    
    [ButtonMethod]
    private void OnDebugMove()
    {
        if (Application.isPlaying == false) return;
        
        _controller.MoveDolly(_dollyKey);
    }
    
}
