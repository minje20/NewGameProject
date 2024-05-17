using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Timeline;

public class IceDispenserMarker : Marker, IMiniGameMarker
{
    public PropertyName id => "IceDispenserMarker";

    public IMiniGameBehaviour Create()
        => new IceDispenserBehaviour();
}