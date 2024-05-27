using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

public class SelectMiniGameBehaviour: IMiniGameBehaviour
{
    private string _selectorKey;

    public SelectMiniGameBehaviour(string selectorKey)
    {
        _selectorKey = selectorKey;
    }

    public async UniTask Invoke(IMiniGameBinder binder, CancellationTokenSource source)
    {
        var selector = binder.GetComponentT<ISelectorUIController>(_selectorKey);
        var drink = binder.GetComponentT<DrinkPosition>("DrinkPosition");

        selector.Open(false);

        drink.Data = null;

        while (true)
        {
            DrinkData data = await selector.GetSelectedItemOnChanged(source.Token);

            if (data == null)
            {
                return;
            }
            
            if(data is not NoSelectedDrinkData)
            {
                drink.Data = data;
                
                if(drink.Data)
                    drink.Renderer.sprite = drink.Data.Sprite;
            }
        }
    }
}