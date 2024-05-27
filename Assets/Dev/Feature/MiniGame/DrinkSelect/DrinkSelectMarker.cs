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
        => new SelectMiniGameBehaviour("AlcoholSelector");
}