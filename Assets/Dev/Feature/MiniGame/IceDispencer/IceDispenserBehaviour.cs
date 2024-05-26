using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Timeline;

public class IceDispenserBehaviour : IMiniGameBehaviour
{
    public async UniTask Invoke(IMiniGameBinder binder, CancellationTokenSource source)
    {
        var controller = binder.GetComponentT<IceDispenserController>("IceDispenserController");
        var barController = binder.GetComponentT<BarController>("BarController");
        var scoreController = binder.GetComponentT<CountTextScoreController>("CountTextScoreController");

        RecipeData recipeData = barController.CurrentRecipeData;

        scoreController.Setup(
            recipeData.Iceparameter.CountScoreParam,
            new TextScoreDisplayer.ValueParameter() { MaxCount = recipeData.Iceparameter.CountScoreParam.TargetCount })
            ;

        scoreController.SetEnableText(true);

        await UniTask.WhenAny(
            controller.GameStart(),
            UniTask.Create(async () =>
            {
                while (true)
                {
                    int count = await controller.IceCount.WaitAsync();
                    scoreController.SetCount(count);
                    barController.Context.CurrentIceCount = count;
                }
            })
            .WithCancellation(GlobalCancelation.PlayMode)
        );

        await scoreController.DisplayResult();

        barController.Context.IceScore = scoreController.CurrentScore;
        barController.Context.IsIceEnd = true;
        
        scoreController.Release();
        controller.GameReset();
    }
}