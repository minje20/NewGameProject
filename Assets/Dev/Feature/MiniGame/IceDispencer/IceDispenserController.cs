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
    #region Inspector
    [field: SerializeField, Foldout("데이터"), InitializationField, MustBeAssigned] 
    private IceDispenserData _data;
    
    [field: SerializeField, Foldout("컴포넌트(건들지 마시오)"), InitializationField, MustBeAssigned] 
    private GameObject _icePrefab;
    
    [field: SerializeField, Foldout("컴포넌트(건들지 마시오)"), InitializationField, MustBeAssigned] 
    private PressedButton _button;
    
    [field: SerializeField, Foldout("컴포넌트(건들지 마시오)"), InitializationField, MustBeAssigned] 
    private IceDispenserPosition _position;
    
    [field: SerializeField, Foldout("컴포넌트(건들지 마시오)"), InitializationField, MustBeAssigned] 
    private MiniGameCircleTimer _gameTimer;
    
    [field: SerializeField, Foldout("컴포넌트(건들지 마시오)"), InitializationField, MustBeAssigned] 
    private HUDController _moneyHud;
    #endregion

    #region Getter/Setter
    public IceDispenserData Data => _data;
    #endregion
    
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

    public AsyncReactiveProperty<int> IceCount { get; private set; } = new(0);

    public async UniTask GameStart(CancellationToken? token = null)
    {
        if (gameObject.activeSelf) return;

        GameReset();

        gameObject.SetActive(true);

        _gameTimer.TimerStart(Data.GameDuration);
        StopAllCoroutines();
        StartCoroutine(CoUpdate());

        var t =  CancellationTokenSource.CreateLinkedTokenSource(
            GlobalCancelation.PlayMode,
            token ?? GlobalCancelation.PlayMode
        ).Token;

        await UniTask.WaitUntil(()=>_gameTimer.IsBegin == false, PlayerLoopTiming.Update, t);
    }

    private void __MiniGame_Reset__()
    {
        GameReset();
    }
    
    public void GameReset()
    {
        while (_createdRandomIce.Any())
        {
            var obj = _createdRandomIce.Dequeue();
            Destroy(obj);
        }

        IceCount.Value = 0;
        
        gameObject.SetActive(false);
        _gameTimer.TimerStop();
    }
    

    private GameObject CreateIce()
    {
        var obj = Instantiate(_icePrefab);

        obj.transform.localScale = GetRandomSize();
        _createdRandomIce.Enqueue(obj);

        obj.transform.position = _position.IceSpawnPoint;
        IceCount.Value += 1;

        return obj;
    }

    private Vector3 GetRandomSize()
    {
        var randomIndex = Random.Range(0, Data.RandomSizeVariation.Count - 1);
        Debug.Assert(randomIndex >= 0 && randomIndex < Data.RandomSizeVariation.Count);

        return Data.RandomSizeVariation[randomIndex];
    }
    private float GetRandomCreationStepWaitDuration()
    {
        return Random.Range(Data.CreateStepWaitDurationRange.x, Data.CreateStepWaitDurationRange.y);
    }
    private int GetRandomCreationStepIceCount()
    {
        return Random.Range(Data.CreateStepIceCountRange.x, Data.CreateStepIceCountRange.y);
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
                    var angle = Mathf.Lerp(Data.CreationFov * -0.5f, Data.CreationFov * 0.5f, Random.value);
                    obj.transform.position = _position.IceSpawnPoint + Quaternion.Euler(0f, 0f, angle) * Vector3.down * Data.CreationDistanceFromPosition;
                    obj.transform.rotation = quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
                    
                    obj.GetComponent<Rigidbody2D>()?.AddForce(
                        Data.CreationForcePower *
                        (Quaternion.Euler(0f, 0f, angle) * Vector3.down)
                        , ForceMode2D.Impulse);
                    
                    _moneyHud.SetValue(_moneyHud.Value - 10);
                    
                    yield return new WaitForSeconds(Data.CreationDelay);
                }
                yield return new WaitForSeconds(GetRandomCreationStepWaitDuration());
            }

            yield return new WaitForEndOfFrame();
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (Data == false) return;
        
        var pos = _position.IceSpawnPoint;
        Gizmos.DrawRay(pos, Vector3.down * Data.CreationDistanceFromPosition);
        var el = Quaternion.Euler(0f, 0f, Data.CreationFov * -0.5f);
        var er = Quaternion.Euler(0f, 0f, Data.CreationFov * 0.5f);
        
        Gizmos.DrawRay(pos, er * Vector3.down * 5f);
        Gizmos.DrawRay(pos, el * Vector3.down * 5f);
    }
}