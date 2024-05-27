using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Timeline;



public class IngredientSelectMarker : Marker, IMiniGameMarker
{
    public PropertyName id => "IngredientSelect";

    public IMiniGameBehaviour Create()
        => new IngredientSelectMiniGameBehaviour();
}

public class IngredientSelectMiniGameBehaviour: IMiniGameBehaviour
{
    public async UniTask Invoke(IMiniGameBinder binder, CancellationTokenSource source)
    {
        var selector = binder.GetComponentT<ISelectorUIController>("IngredientSelector");
        var ingredient = binder.GetComponentT<IngredientPosition>("IngredientPosition");

        selector.Open(false);

        ingredient.Data = null;
        ingredient.Renderer.sprite = null;

        while (true)
        {
            SelectorButton btn = await selector.GetSelectedItemOnChanged(source.Token);

            if (btn == null)
            {
                await UniTask.NextFrame(PlayerLoopTiming.Update, GlobalCancelation.PlayMode);
            }
            else
            {
                ingredient.Data = btn.Data as IngredientData;
                
                if(ingredient.Data)
                    ingredient.Renderer.sprite = ingredient.Data.Sprite;
            }
        }
    }
}