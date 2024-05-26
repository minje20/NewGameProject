using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

public static class TMPProDotween
{
    public static UniTask DoTextUniTask(this TMPro.TMP_Text text, string endValue, float duration,
        CancellationToken? token = null)
    {
        return _DoText(text, endValue, duration, token);
    }

    private static async UniTask _DoText(TMPro.TMP_Text text, string endValue, float duration,
        CancellationToken? token = null)
    {
        string tempString = "";
        CancellationToken t = token == null
                ? GlobalCancelation.PlayMode
                : CancellationTokenSource.CreateLinkedTokenSource(GlobalCancelation.PlayMode, token.Value).Token
            ;

        text.text = "";

        for (int i = 0; i < endValue.Length; i++)
        {
            tempString += endValue[i];
            text.text = tempString;

            await UniTask.Delay((int)(duration / endValue.Length * 1000f), DelayType.DeltaTime, PlayerLoopTiming.Update,
                t);
        }
    }
}