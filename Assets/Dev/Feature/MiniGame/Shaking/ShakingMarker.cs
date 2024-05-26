using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Timeline;

public class ShakingMarker : Marker, IMiniGameMarker
{
    public PropertyName id => "ShakingMarker";

    public IMiniGameBehaviour Create()
        => new ShakingMiniGameBehaviour();
}

public class ShakingMiniGameBehaviour : IMiniGameBehaviour
{
    private static readonly int Shake = Animator.StringToHash("Shake");

    public UniTask Invoke(IMiniGameBinder binder, CancellationTokenSource source)
    {
        var controller = binder.GetComponentT<ShakingMiniGameController>("ShakingController");
        var barController = binder.GetComponentT<BarController>("BarController");
        var jump = binder.GetReceiver<JumpReceiver>();

        jump.Skip = false;
        controller.GameStart().ContinueWith(x =>
        {
            jump.Skip = true;
            barController.Context.IsShakeEnd = true;
            barController.Context.ShakeScore = x;
        });
        
        return UniTask.CompletedTask;
    }
}
