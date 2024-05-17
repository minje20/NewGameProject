using System.Collections;
using UnityEngine;


// TODO: 비주얼적인 부분도 구현
public class MiniGameTimer : MonoBehaviour
{
    [SerializeField] private float _targetTime = 1f;

    public float TargetTime
    {
        get => _targetTime;
        set => _targetTime = value;
    }

    public bool IsBegan { get; private set; }
    
    public void Begin()
    {
        IsBegan = true;
        
        StopAllCoroutines();
        StartCoroutine(CoUpdate());
    }

    public void End()
    {
        StopAllCoroutines();
        IsBegan = false;
    }

    private IEnumerator CoUpdate()
    {
        IsBegan = true;
        yield return new WaitForSeconds(_targetTime);
        IsBegan = false;
    }
}