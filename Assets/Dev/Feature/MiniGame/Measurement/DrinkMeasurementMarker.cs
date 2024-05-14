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

        if (drinkPosition.Data == false || drinkPosition.Data is NoSelectedDrinkData)
        {
            return;
        }
        
        controller.Button.gameObject.SetActive(true);
        
        var backupPosition = transform.position;
        var backupRotation = transform.rotation;
        
        
        var bottlePos = jigger.position + controller.FromJigger;
        float bottleToPivotLength = Vector3.Distance(drinkPosition.Data.BottlePosition, drinkPosition.Data.RotationPivotPosition);
        var pivot = bottlePos + Quaternion.AngleAxis(controller.MaxAngle, Vector3.forward) * Vector3.down * bottleToPivotLength;
        var inverseM = GetInverseMatrix(bottlePos, pivot, controller.MaxAngle);

        transform.position = inverseM.GetPosition() + -drinkPosition.Data.BottlePosition;
        transform.rotation = inverseM.rotation;
        
        bool pressedStarted = false;

        var m = GetMatrix(transform.position, pivot, controller.MaxAngle);

        while (pressedStarted == false || controller.Button.IsPressed)
        {
            if (pressedStarted == false && controller.Button.IsPressed)
            {
                pressedStarted = true;
            }

            float t = controller.OnPressed.Value;
            
            transform.rotation = Quaternion.Lerp(backupRotation, m.rotation, t);
            transform.position = Vector3.Lerp(backupPosition, m.GetPosition(), t);

            await UniTask.NextFrame(PlayerLoopTiming.Update, GlobalCancelation.PlayMode);
        }

        await UniTask.WhenAll(
            transform.DOMove(backupPosition, controller.EndOfRollbackDuration).AsyncWaitForCompletion().AsUniTask(),
            transform.DORotate(Vector3.zero, controller.EndOfRollbackDuration).AsyncWaitForCompletion().AsUniTask()
        )
        .WithCancellation(GlobalCancelation.PlayMode);
        
        
        controller.Button.gameObject.SetActive(false);
    }
}