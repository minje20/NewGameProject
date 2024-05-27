using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Timeline;

public class LiquidPourWaitMarker :  Marker, IMiniGameMarker
{
    public PropertyName id => "LiquidPourWait";

    public IMiniGameBehaviour Create()
        => new LiquidPourWait();
}

public class LiquidPourWait : IMiniGameBehaviour
{
    public async UniTask Invoke(IMiniGameBinder binder, CancellationTokenSource source)
    {
        var controller = binder.GetComponentT<LiquidPourController>("ShakerToJiggerLiquidPourController");

        await UniTask.WaitUntil(() => controller.IsRunning == false || source.IsCancellationRequested);
        controller.GameReset();
    }
}