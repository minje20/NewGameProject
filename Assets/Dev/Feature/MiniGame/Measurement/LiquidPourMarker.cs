using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Timeline;

public class LiquidPourMarker : Marker, IMiniGameMarker
{
    public PropertyName id => "LiquidPour";

    public IMiniGameBehaviour Create()
        => new LiquidPour();
}

public class LiquidPour : IMiniGameBehaviour
{
    public UniTask Invoke(IMiniGameBinder binder, CancellationTokenSource source)
    {
        var controller = binder.GetComponentT<LiquidPourController>("ShakerToJiggerLiquidPourController");
        
        _ = controller.GameStart();
        
        return UniTask.CompletedTask;
    }
}
