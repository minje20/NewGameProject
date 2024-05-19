using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class MiniGameCircleTimer : MonoBehaviour
{
    [SerializeField] private Image _fillImage;
    [SerializeField] private bool _isOneMius;

    public float TargetTime { get; private set; }

    public bool IsBegin { get; private set; }

    public AsyncReactiveProperty<float> Timer { get; private set; }
    public AsyncReactiveProperty<float> NormalizedTimer { get; private set; }

    private void Awake()
    {
        TimerStop();
    }

    public void TimerStart(float time)
    {
        if (IsBegin)
        {
            Debug.LogError($"CircleTimer({name})가 이미 시작되었음.");
            return;
        }

        Timer = new AsyncReactiveProperty<float>(0f);
        NormalizedTimer = new AsyncReactiveProperty<float>(0f);
        TargetTime = time;
        IsBegin = true;
        gameObject.SetActive(true);
        
        StartCoroutine(OnUpdate());
    }

    public void TimerStop()
    {
        StopAllCoroutines();
        
        IsBegin = false;
        TargetTime = -1f;
        Timer = null;
        NormalizedTimer = null;
        gameObject.SetActive(false);
    }


    private IEnumerator OnUpdate()
    {
        float timer = 0f;

        TargetTime = Mathf.Max(TargetTime, Mathf.Epsilon);
        while (true)
        {
            _fillImage.fillAmount = _isOneMius ? 
                1f - timer / TargetTime :
                timer / TargetTime
                ;

            Timer.Value = timer;
            NormalizedTimer.Value = _fillImage.fillAmount;

            timer += Time.deltaTime;
            
            if (timer >= TargetTime)
            {
                break;
            }
            
            yield return new WaitForEndOfFrame();
        }

        TimerStop();
    }
    
}
