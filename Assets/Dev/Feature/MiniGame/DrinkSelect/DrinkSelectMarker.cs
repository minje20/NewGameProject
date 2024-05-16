using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Timeline;



public class DrinkSelectMarker : Marker, IMiniGameMarker
{
    public PropertyName id => "DrinkSelect";

    public IMiniGameBehaviour Create()
        => new SelectMiniGameBehaviour(false);
}

public class SelectMiniGameBehaviour: IMiniGameBehaviour
{
    private bool _optional;

    public SelectMiniGameBehaviour(bool optional)
    {
        _optional = optional;
    }

    public async UniTask Invoke(IMiniGameBinder binder, CancellationTokenSource source)
    {
        var selector = binder.GetComponentT<ISelectorUIController>("DrinkSelector");
        var drink = binder.GetComponentT<DrinkPosition>("DrinkPosition");

        selector.Open(_optional);

        drink.Data = null;
        drink.Renderer.sprite = null;

        while (true)
        {
            SelectorButton btn = await selector.GetSelectedItemOnChanged();

            if (btn == null)
            {
                await UniTask.NextFrame(PlayerLoopTiming.Update, GlobalCancelation.PlayMode);
            }
            else if(btn.Data is not NoSelectedDrinkData)
            {
                drink.Data = btn.Data as DrinkData;
                
                if(drink.Data)
                    drink.Renderer.sprite = drink.Data.Sprite;
            }
            else if (btn.Data is NoSelectedDrinkData noSelectedDrinkData && _optional)
            {
                drink.Data = noSelectedDrinkData;
                drink.Renderer.sprite = noSelectedDrinkData.Sprite;
            }
        }
    }
}