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
        var barController = binder.GetComponentT<BarController>("BarController");
        var pourController = binder.GetComponentT<LiquidPourController>("ShakerToJiggerLiquidPourController");
        var drinkPosition = binder.GetComponentT<DrinkPosition>("DrinkPosition");
        var scoreController = binder.GetComponentT<CountLiquidScoreController>("CountLiquidScoreController");
        var jigger = binder.GetComponentT<Transform>("Jigger");
        var transform = drinkPosition.transform;

        var backupPosition = drinkPosition.transform.position;

        if (drinkPosition.Data == false || drinkPosition.Data is NoSelectedDrinkData)
        {
            return;
        }

        var token = CancellationTokenSource.CreateLinkedTokenSource(
            GlobalCancelation.PlayMode,
            source.Token
        ).Token;

        controller.Jigger = jigger;
        controller.Drink = drinkPosition;

        controller.GameReset();
        controller.ApplyLiquidMaterial();

        MiniMeasurementInfo info = null;
        RecipeData currentRecipeData = barController.CurrentRecipeData;

        if (barController.Context.MeasuredDrinkTable.TryGetValue(controller.Drink.Data, out var item))
        {
            if (item.IsEnd == false)
            {
                info = item.Info;
            }
        }
        
        scoreController.Setup(info?.CountScoreParam);
        await controller.Calculate().WithCancellation(token);
        
        controller.Started = true;
        
        await UniTask.WhenAny(
            UniTask.Delay((int)(controller.Data.GameDuration * 1000f), DelayType.DeltaTime, PlayerLoopTiming.Update,
                token),
            UniTask.Create(async () =>
            {
                while (true)
                {
                    int count = await controller.LiquidCount.WaitAsync(token);
                    scoreController.SetCount(count);
                }
            }),
            UniTask.Create(async () =>
            {
                while (true)
                {
                    if (info != null)
                    {
                        scoreController.ShowLine(info.LiquidScoreParam.LinePosition);
                    }
                    else
                    {
                        scoreController.HideLine();
                    }
                    
                    await UniTask.NextFrame(PlayerLoopTiming.Update, token);
                }
            })
        );
        
        if (barController.Context.MeasuredDrinkTable.TryGetValue(drinkPosition.Data, out var data))
        {
            if (data.IsEnd == false)
            {
                data.Score = scoreController.CurrentScore;
            }
            
            data.IsEnd = true;
        }
        
        controller.Started = false;
        _ = scoreController.DisplayResult().ContinueWith(() => scoreController.Release());
        

        await UniTask.WhenAll(
            transform.DOMove(backupPosition, controller.Data.EndOfRollbackDuration).AsyncWaitForCompletion().AsUniTask(),
            transform.DORotate(Vector3.zero, controller.Data.EndOfRollbackDuration).AsyncWaitForCompletion().AsUniTask()
        )
        .WithCancellation(token);
        
        pourController.CountOfTotalCreatingLiquid = controller.LiquidCount.Value;
        pourController.LiquidMaterial = drinkPosition.Data.LiquidMaterial;
        
        controller.GameReset();

    }
}