using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Cinemachine;
using Cysharp.Threading.Tasks;
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

    [field: SerializeField, OverrideLabel("보간이 끝났다가 판단할 거리(미터)")]
    private float _closerDistance = 0.001f;
    
    [field: SerializeField, Header("DollyPath key-value 설정")]
    private List<DollyPathPair> _statePairs;
    
    private CinemachineTrackedDolly _dolly;
    private CinemachineVirtualCamera _camera;

    public override void Init(CameraController controller)
    {
        _camera = controller.GetComponent<CinemachineVirtualCamera>();
        _dolly = _camera.GetCinemachineComponent<CinemachineTrackedDolly>();
    }
    
    public void Move(string state, bool immediately=false)
    {
        if (_statePairs == null) return;

        var pair = _statePairs.FirstOrDefault(x => x.Location == state);
        if (pair != null)
        {
            int position = pair.Position;
            _dolly.m_PathPosition = position;
        }
    }
    
    public async UniTask MoveAsync(string state)
    {
        if (_statePairs == null) return;

        var pair = _statePairs.FirstOrDefault(x => x.Location == state);
        if (pair != null)
        {
            int position = pair.Position;
            _dolly.m_PathPosition = position;
            Debug.Log(position);
            
            await WaitForCameraMoved(position);
        }
    }

    private async UniTask WaitForCameraMoved(float next)
    {
        while (true)
        {
            float distance = Vector3.Distance(
                _camera.transform.position,
                _dolly.m_Path.EvaluatePositionAtUnit(next, _dolly.m_PositionUnits)
            );

            if (distance <= _closerDistance)
            {
                return;
            }

            await UniTask.NextFrame(PlayerLoopTiming.Update, GlobalCancelation.PlayMode);
        }
    }
}