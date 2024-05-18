using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class TaskExtensions
{
    public static CancellationToken CancellationUntil(Func<bool> callback, bool includeGlobalCancelation = true)
    {
        var source = new CancellationTokenSource();
        if (callback == null)
        {
            source.Cancel();
            return source.Token;
        }
        
        _ = UniTask
            .WaitUntil(() => callback(),
                cancellationToken: includeGlobalCancelation ? GlobalCancelation.PlayMode : CancellationToken.None)
            .ContinueWith(() => source.Cancel());

        return source.Token;
    }
}