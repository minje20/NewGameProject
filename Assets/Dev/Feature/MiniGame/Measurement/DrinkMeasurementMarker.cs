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
    public static Matrix4x4 GetMatrix(Vector3 position, Vector3 pivot, float angle)
    {
        Vector3 vector3 = Quaternion.AngleAxis(angle, Vector3.forward) * (position - pivot);
        position = pivot + vector3;
        
        var m =Matrix4x4.TRS(
            position,
            Quaternion.Euler(0f, 0f, angle),
            Vector3.one
        );

        return m;
    }
    public static Matrix4x4 GetInverseMatrix(Vector3 position, Vector3 pivot, float angle)
    {
        Vector3 vector3 = Quaternion.AngleAxis(-angle, Vector3.forward) * (position - pivot);
        position = pivot + vector3;
        
        var m =Matrix4x4.TRS(
            position,
            Quaternion.identity,
            Vector3.one
        );
        return m;
    }
    public async UniTask Invoke(IMiniGameBinder binder, CancellationTokenSource source)
    {
        var controller = binder.GetComponentT<DrinkMeasurementMiniGame>("DrinkMeasurement");
        var drinkPosition = binder.GetComponentT<DrinkPosition>("DrinkPosition");
        var jigger = binder.GetComponentT<Transform>("Jigger");
        var transform = drinkPosition.transform;

        var backupPosition = drinkPosition.transform.position;

        if (drinkPosition.Data == false || drinkPosition.Data is NoSelectedDrinkData)
        {
            return;
        }

        controller.Jigger = jigger;
        controller.Drink = drinkPosition;

        controller.Reset();
        await controller.Calculate();
        
        controller.Started = true;

        await UniTask.Delay((int)(controller.GameDuration * 1000f), DelayType.DeltaTime, PlayerLoopTiming.Update,GlobalCancelation.PlayMode);
        
        controller.Started = false;

        await UniTask.WhenAll(
            transform.DOMove(backupPosition, controller.EndOfRollbackDuration).AsyncWaitForCompletion().AsUniTask(),
            transform.DORotate(Vector3.zero, controller.EndOfRollbackDuration).AsyncWaitForCompletion().AsUniTask()
        )
        .WithCancellation(GlobalCancelation.PlayMode);
        
        controller.Reset();
    }
}