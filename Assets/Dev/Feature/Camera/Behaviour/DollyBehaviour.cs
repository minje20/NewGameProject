using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using MyBox;
using UnityEngine;

[CreateAssetMenu(menuName = "IndieLINY/CameraBehaviour/Dolly", fileName = "NewDolly")]
public class DollyBehaviour : CameraBehaviour
{
    [System.Serializable]
    public class DollyPathPair
    {
        public string Location;
        public int Position;
    }
    
    [field: SerializeField, Header("DollyPath key-value 설정")]
    private List<DollyPathPair> _statePairs;
    
    private CinemachineTrackedDolly _dolly;

    public override void Init(CameraController controller)
    {
        var camera = controller.GetComponent<CinemachineVirtualCamera>();
        _dolly = camera.GetCinemachineComponent<CinemachineTrackedDolly>();
    }
    
    public void Move(string state)
    {
        if (_statePairs == null) return;

        var pair = _statePairs.FirstOrDefault(x => x.Location == state);
        if (pair != null)
        {
            int position = pair.Position;
            _dolly.m_PathPosition = position;
        }
    }

    [field: SerializeField, DefinedValues(nameof(DEBUG_TargetState))]
    private string _DEBUG_target_state;
    private string[] DEBUG_TargetState()
    {
        if (_statePairs == null) return new string[] { "Empty" };
        
        return _statePairs.Select(x => x.Location).ToArray();
    }
    
    [ButtonMethod]
    private void OnDebugMove()
    {
        if (Application.isPlaying == false) return;
        
        Move(_DEBUG_target_state);
    }
}