using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using Cysharp.Threading.Tasks;
using MyBox;
using UnityEngine;

public abstract class CameraBehaviour : ScriptableObject
{
    public abstract void Init(CameraController controller);
}

