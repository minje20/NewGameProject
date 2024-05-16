using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;


public class DrinkSelectOptionalMarker : Marker, IMiniGameMarker
{
    public PropertyName id => "DrinkSelectOptional";

    public IMiniGameBehaviour Create()
        => new SelectMiniGameBehaviour(true);
}
