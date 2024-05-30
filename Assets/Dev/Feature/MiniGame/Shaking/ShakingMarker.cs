using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Timeline;

public class ShakingMarker : Marker, IMiniGameMarker
{
    [SerializeField] private int shakingIteration;

    public int ShakingIteration => shakingIteration;

    public PropertyName id => "ShakingMarker";

    public IMiniGameBehaviour Create()
        => new ShakingMiniGameBehaviour(ShakingIteration);
}

public class ShakingMiniGameBehaviour : IMiniGameBehaviour
{
    private static readonly int Shake = Animator.StringToHash("Shake");

    private int _number;

    public ShakingMiniGameBehaviour(int number)
    {
        _number = number;
    }

    public UniTask Invoke(IMiniGameBinder binder, CancellationTokenSource source)
    {
        var controller = binder.GetComponentT<ShakingMiniGameController>("ShakingController");
        var barController = binder.GetComponentT<BarController>("BarController");
        var jump = binder.GetReceiver<JumpReceiver>();

        jump.Skip = false;
        controller.GameStart(source.Token).ContinueWith(x =>
        {
            jump.Skip = true;

            if (_number == 1)
            {
                barController.Context.IsShake1End = true;
                barController.Context.ShakeScore1 = x;
            }
            else
            {
                barController.Context.IsShake2End = true;
                barController.Context.ShakeScore2 = x;
            }
        });
        
        return UniTask.CompletedTask;
    }
}
