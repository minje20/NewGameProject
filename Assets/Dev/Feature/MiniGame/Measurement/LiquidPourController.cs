using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using MyBox;
using UnityEngine;

public class LiquidPourController : MonoBehaviour
{
    #region Inspector
    [field: SerializeField, OverrideLabel("데이터"), Foldout("데이터"), InitializationField, MustBeAssigned]
    private LiquidPourData _data;
    [field: SerializeField, Foldout("컴포넌트(건들지 마시오)"), InitializationField, MustBeAssigned]
    public MeshRenderer _liquidRenerer;
    [field: SerializeField, Foldout("컴포넌트(건들지 마시오)"), InitializationField, MustBeAssigned]
    public Transform _jigger;
    [field: SerializeField, Foldout("컴포넌트(건들지 마시오)"), InitializationField, MustBeAssigned]
    public GameObject _liquidPrefab;
    
    #endregion

    #region Getter/Setter

    public LiquidPourData Data => _data;
    public int CountOfTotalCreatingLiquid { get; set; }
    public bool IsRunning { get; private set; }
    public Material LiquidMaterial { get; set; }

    public Vector2 LiquidForceDirection => _jigger.transform.localToWorldMatrix.MultiplyVector((Quaternion.Euler(0f, 0f, Data.LiquidForceDirectionAngle) * Vector2.left).normalized);
    #endregion


    private int _currentCreatedLiquidCount;
    private CancellationTokenSource _cancellation;
    private List<GameObject> _liquidList = new(100);

    private void Awake()
    {
        GameReset();
    }

    public UniTask GameStart(CancellationToken token )
    {
        if (LiquidMaterial != null)
        {
            _liquidRenerer.sharedMaterial = LiquidMaterial;
        }

        IsRunning = true;
        _liquidRenerer.gameObject.SetActive(true);

        var t = CancellationTokenSource.CreateLinkedTokenSource(
            token,
            GlobalCancelation.PlayMode
        ).Token;

        return UniTask.WhenAny(
            UniTask.Delay((int)(Data.Timeout * 1000f), DelayType.DeltaTime, PlayerLoopTiming.Update,
                t),
            UniTask.WhenAll(
                OnLiquidUpdate(),
                UniTask.WaitUntil(
                    () =>
                    {
                        int count = 0;
                        foreach (var item in _liquidList)
                        {
                            if (item == false)
                            {
                                count++;
                            }
                        }
                        
                        return count >= CountOfTotalCreatingLiquid;
                    },
                    PlayerLoopTiming.Update, t)
            )
        ).WithCancellation(_cancellation.Token)
        .ContinueWith(x=>IsRunning = false);
    }

    public void GameReset()
    {
        IsRunning = false;
        _liquidRenerer.gameObject.SetActive(false);
        
        if (_cancellation?.IsCancellationRequested == false)
            _cancellation?.Cancel();

        _cancellation = new CancellationTokenSource();

        _liquidList.ForEach(x =>
        {
            if (x)
            {
                Destroy(x);
            }
        });
        _liquidList.Clear();
        _currentCreatedLiquidCount = 0;
        CountOfTotalCreatingLiquid = 0;
    }

    private Vector3 GetPointOfLine(float t)
    {
        var m = _jigger.transform.localToWorldMatrix;
        var a = (Vector3)Data.JiggerOffset + Vector3.right * (Data.LiquidCreationLineLength * 0.5f);
        var b = (Vector3)Data.JiggerOffset + Vector3.left * (Data.LiquidCreationLineLength * 0.5f);

        a = m.MultiplyPoint(a);
        b = m.MultiplyPoint(b);

        return Vector3.Lerp(a, b, t);
    }

    private async UniTask OnLiquidUpdate()
    {
        int length = Data.CountOfOLineCreatingLiquid;
        float step = 1f / length;
        float halfStep = step * 0.5f;

        while (true)
        {
            for (int i = 0; i <= length; i++)
            {
                var obj = CreateLiquid();
                float t = (float)i / (float)length + halfStep;

                if (obj == null) return;
                obj.transform.position = GetPointOfLine(t);
            }


            await UniTask.Delay((int)(Data.LiquidCreationDelay * 1000f), DelayType.DeltaTime, PlayerLoopTiming.Update,
                GlobalCancelation.PlayMode);
        }
    }

    [CanBeNull]
    private GameObject CreateLiquid()
    {
        if (_currentCreatedLiquidCount >= CountOfTotalCreatingLiquid)
        {
            return null;
        }

        var obj = Instantiate(_liquidPrefab);
        obj.SetActive(true);
        obj.GetComponent<Rigidbody2D>()?.AddForce(Data.LiquidForce * LiquidForceDirection, ForceMode2D.Impulse);

        _currentCreatedLiquidCount++;
        
        _liquidList.Add(obj);


        return obj;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("LiquidParticle"))
        {
            GameObject.Destroy(other.gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        if (_jigger == false) return;

        Gizmos.DrawLine(
            GetPointOfLine(0f),
            GetPointOfLine(1f)
        );

        Gizmos.color = Color.red;
        Gizmos.DrawRay(_jigger.transform.position, LiquidForceDirection * 1.5f);
    }
}