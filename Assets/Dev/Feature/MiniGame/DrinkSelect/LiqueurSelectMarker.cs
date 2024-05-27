using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Timeline;



public class LiqueurSelectMarker : Marker, IMiniGameMarker
{
    public PropertyName id => "LiqueurSelect";

    public IMiniGameBehaviour Create()
        => new SelectMiniGameBehaviour("LiqueurSelector");
}