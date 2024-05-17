using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Timeline;
using Random = UnityEngine.Random;

public class IceDispenserController : MonoBehaviour
{
    [SerializeField] private List<Vector3> _randomSizeVariation;
    [SerializeField] private float _creationDelay;
    [SerializeField] private float _gameDuration;
    [SerializeField] private GameObject _icePrefab;
    [SerializeField] private PressedButton _button;
    [SerializeField] private MiniGameTimer _gameTimer;
    [SerializeField] private IceDispenserPosition _position;

    private Queue<GameObject> _createdRandomIce = new();

    public async UniTask GameStart()
    {
        if (gameObject.activeSelf) return;

        GameReset();

        gameObject.SetActive(true);

        _gameTimer.TargetTime = _gameDuration;
            
        _gameTimer.Begin();
        StopAllCoroutines();
        StartCoroutine(CoUpdate());

        await UniTask.WaitUntil(()=>_gameTimer.IsBegan == false, PlayerLoopTiming.Update, GlobalCancelation.PlayMode);
    }
    
    public void GameReset()
    {
        while (_createdRandomIce.Any())
        {
            var obj = _createdRandomIce.Dequeue();
            Destroy(obj);
        }
        
        gameObject.SetActive(false);
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

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    IEnumerator CoUpdate()
    {
        while (true)
        {
            if (_gameTimer.IsBegan == false)
            {
                yield break;
            }
            
            if (_button.IsPressed)
            {
                _ = CreateIce();
                yield return new WaitForSeconds(_creationDelay);
            }

            yield return new WaitForEndOfFrame();
        }
    }
    
    
}