using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using MyBox;
using UnityEditor;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class DrinkMeasurementMarker : Marker, IMiniGameMarker
{
    public PropertyName id => "DrinkMeasurement";

    public IMiniGameBehaviour Create()
        => new DrinkMeasurementBehaviour();
}

public class DrinkMeasurementBehaviour : IMiniGameBehaviour
{
    public async UniTask Invoke(IMiniGameBinder binder, CancellationTokenSource source)
    {
        var controller = binder.GetComponentT<DrinkMeasurementMiniGame>("DrinkMeasurement");
        var drinkPosition = binder.GetComponentT<DrinkPosition>("DrinkPosition");
        var scoreController = binder.GetComponentT<CountLiquidScoreController>("CountLiquidScoreController");
        var jigger = binder.GetComponentT<Transform>("Jigger");
        var transform = drinkPosition.transform;

        var backupPosition = drinkPosition.transform.position;

        if (drinkPosition.Data == false || drinkPosition.Data is NoSelectedDrinkData)
        {
            return;
        }

        controller.Jigger = jigger;
        controller.Drink = drinkPosition;

        controller.GameReset();
        scoreController.Setup();
        //TODO: scoreController.ShowLine 더미 코드.
        await controller.Calculate();
        
        controller.Started = true;
        
        await UniTask.WhenAny(
            UniTask.Delay((int)(controller.Data.GameDuration * 1000f), DelayType.DeltaTime, PlayerLoopTiming.Update,
                GlobalCancelation.PlayMode),
            UniTask.Create(async () =>
            {
                while (true)
                {
                    int count = await controller.LiquidCount.WaitAsync(GlobalCancelation.PlayMode);
                    scoreController.SetCount(count);
                }
            }),
            UniTask.Create(async () =>
            {
                while (true)
                {
                    scoreController.ShowLine(controller.Data.NormalizedLinePosition);
                    await UniTask.NextFrame(PlayerLoopTiming.Update, GlobalCancelation.PlayMode);
                }
            })
        );
        
        controller.Started = false;
        _ = scoreController.DisplayResult().ContinueWith(() => scoreController.Release());
        

        await UniTask.WhenAll(
            transform.DOMove(backupPosition, controller.Data.EndOfRollbackDuration).AsyncWaitForCompletion().AsUniTask(),
            transform.DORotate(Vector3.zero, controller.Data.EndOfRollbackDuration).AsyncWaitForCompletion().AsUniTask()
        )
        .WithCancellation(GlobalCancelation.PlayMode);
        
        controller.GameReset();
    }
}