using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using IndieLINY.MessagePipe;
using MyBox;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Serialization;

public interface IMiniGameMarker : INotification
{
    public IMiniGameBehaviour Create();
}

public interface IMiniGameBehaviour
{
    public UniTask Invoke(IMiniGameBinder binder, CancellationTokenSource source);
}

public abstract class MiniGameData : ScriptableObject
{
}

public interface IMiniGameBinder
{
    public T GetReceiver<T>() where T : class, INotificationReceiver;
    public T GetComponentT<T>(string key) where T : class;
}

public interface ISelectorUIController
{
    public void Open(bool canNoSelect);
    public UniTask<DrinkData> GetSelectedItemOnChanged(CancellationToken token);
}

public class MiniGameController : MonoBehaviour, INotificationReceiver, IMiniGameBinder
{
    [field: SerializeField, AutoProperty, InitializationField, MustBeAssigned]
    private PlayableDirector _director;

    [field: SerializeField, InitializationField, MustBeAssigned]
    private List<MonoBehaviour> _receivers;

    [field: SerializeField, InitializationField, MustBeAssigned]
    private List<BindedMonobehaviour> _components;

    public PlayableDirector Director => _director;

    private CancellationTokenSource _cancellationTokenSource;
    
    public bool IsGameStarted { get; private set; }

    private void Awake()
    {
        Director.playOnAwake = false;
        IsGameStarted = false;
    }

    public void GameStart()
    {
        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource = new CancellationTokenSource();

        BroadcastReset();
        Director.Play();
        IsGameStarted = true;
    }

    public void GameStop()
    {
        if (_cancellationTokenSource == null) return;

        _cancellationTokenSource.Cancel();
        _cancellationTokenSource = null;

        BroadcastReset();
        Director.Stop();
        IsGameStarted = false;
    }

    private void BroadcastReset()
    {
        foreach (var com in _components)
        {
            com.BroadcastMessage("__MiniGame_Reset__", null, SendMessageOptions.DontRequireReceiver);
        }
        foreach (var com in _receivers)
        {
            com.BroadcastMessage("__MiniGame_Reset__", null, SendMessageOptions.DontRequireReceiver);
        }
    }

    public void GameRestart()
    {
        if (_cancellationTokenSource == null) return;

        _cancellationTokenSource.Cancel();
        _cancellationTokenSource = null;
        
        Director.Stop();
        Director.time = 0f;

        BroadcastReset();
        GameStart();
    }

    // timeline receive method
    public void OnNotifyGameEnded()
    {
        IsGameStarted = false;
        _cancellationTokenSource = null;
    }

    public void OnNotify(Playable origin, INotification notification, object context)
    {
        if (notification is IMiniGameMarker marker)
        {
            var instance = marker.Create();

            if (instance is IMiniGameBehaviour behaviour)
            {
                Invoke(behaviour).Forget();
            }
        }
    }

    private async UniTask Invoke(IMiniGameBehaviour behaviour)
    {
        if (behaviour == null) return;

        try
        {
            var task = behaviour.Invoke(this, _cancellationTokenSource);

            if (task.Status == UniTaskStatus.Pending)
            {
                _director.Pause();
                await task;
                _director.Resume();
            }
            else if (task.Status == UniTaskStatus.Faulted)
            {
                Debug.LogException(task.AsTask().Exception);
            }
        }
        catch (OperationCanceledException)
        {
            if (_director == false) return;
            
            if (_director.state == PlayState.Paused)
            {
                _director.Resume();
            }
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    public T GetReceiver<T>() where T : class, INotificationReceiver
        => _receivers.Find(x => x is T) as T;

    public T GetComponentT<T>(string key) where T : class
        => _components.Find(x => x.Key == key)?.Component as T;
}