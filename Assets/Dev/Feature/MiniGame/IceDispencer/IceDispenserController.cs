using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using MyBox;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Timeline;
using Random = UnityEngine.Random;

public class IceDispenserController : MonoBehaviour
{
    [SerializeField] private List<Vector3> _randomSizeVariation;
    [SerializeField] private float _creationDelay;
    [SerializeField] private float _gameDuration;
    [SerializeField] private Vector2 _createStepWaitDurationRange;
    [SerializeField] private Vector2Int _createStepIceCountRange;
    [SerializeField] private int _createStepCount;
    [SerializeField] private int _creationFov;
    [SerializeField] private int _creationForcePower;
    [SerializeField] private float _creationDistanceFromPosition;
    [SerializeField] private GameObject _icePrefab;
    [SerializeField] private PressedButton _button;
    [SerializeField] private IceDispenserPosition _position;
    [SerializeField] private MiniGameCircleTimer _gameTimer;

    [ButtonMethod]
    private void RemoveAll()
    {
        if (Application.isPlaying == false) return;

        while (_createdRandomIce.Any())
        {
            var obj = _createdRandomIce.Dequeue();
            Destroy(obj);
        }
    }

    private Queue<GameObject> _createdRandomIce = new();

    public async UniTask GameStart()
    {
        if (gameObject.activeSelf) return;

        GameReset();

        gameObject.SetActive(true);

        _gameTimer.TimerStart(_gameDuration);
        StopAllCoroutines();
        StartCoroutine(CoUpdate());

        await UniTask.WaitUntil(()=>_gameTimer.IsBegin == false, PlayerLoopTiming.Update, GlobalCancelation.PlayMode);
    }
    
    public void GameReset()
    {
        while (_createdRandomIce.Any())
        {
            var obj = _createdRandomIce.Dequeue();
            Destroy(obj);
        }
        
        gameObject.SetActive(false);
        _gameTimer.TimerStop();
    }
    

    private GameObject CreateIce()
    {
        var obj = Instantiate(_icePrefab);

        obj.transform.localScale = GetRandomSize();
        _createdRandomIce.Enqueue(obj);

        obj.transform.position = _position.IceSpawnPoint;

        return obj;
    }

    private Vector3 GetRandomSize()
    {
        var randomIndex = Random.Range(0, _randomSizeVariation.Count - 1);
        Debug.Assert(randomIndex >= 0 && randomIndex < _randomSizeVariation.Count);

        return _randomSizeVariation[randomIndex];
    }
    private float GetRandomCreationStepWaitDuration()
    {
        return Random.Range(_createStepWaitDurationRange.x, _createStepWaitDurationRange.y);
    }
    private int GetRandomCreationStepIceCount()
    {
        return Random.Range(_createStepIceCountRange.x, _createStepIceCountRange.y);
    }

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    IEnumerator CoUpdate()
    {
        while (true)
        {
            if (_gameTimer.IsBegin == false)
            {
                yield break;
            }
            
            if (_button.IsPressed)
            {
                int length = GetRandomCreationStepIceCount();
                for (int i = 0; i < length; i++)
                {
                    var obj = CreateIce();
                    var angle = Mathf.Lerp(_creationFov * -0.5f, _creationFov * 0.5f, Random.value);
                    obj.transform.position = _position.IceSpawnPoint + Quaternion.Euler(0f, 0f, angle) * Vector3.down * _creationDistanceFromPosition;
                    obj.transform.rotation = quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
                    
                    obj.GetComponent<Rigidbody2D>()?.AddForce(
                        _creationForcePower *
                        (Quaternion.Euler(0f, 0f, angle) * Vector3.down)
                        , ForceMode2D.Impulse);
                    
                    yield return new WaitForSeconds(_creationDelay);
                }
                yield return new WaitForSeconds(GetRandomCreationStepWaitDuration());
            }

            yield return new WaitForEndOfFrame();
        }
    }

    private void OnDrawGizmosSelected()
    {
        var pos = _position.IceSpawnPoint;
        Gizmos.DrawRay(pos, Vector3.down * _creationDistanceFromPosition);
        var el = Quaternion.Euler(0f, 0f, _creationFov * -0.5f);
        var er = Quaternion.Euler(0f, 0f, _creationFov * 0.5f);
        
        Gizmos.DrawRay(pos, er * Vector3.down * 5f);
        Gizmos.DrawRay(pos, el * Vector3.down * 5f);
    }
}