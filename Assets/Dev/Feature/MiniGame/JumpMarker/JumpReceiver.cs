using UnityEngine;
using UnityEngine.Playables;

public class JumpReceiver : MonoBehaviour, INotificationReceiver
{
    public bool Skip { get; set; }
    public void OnNotify(Playable origin, INotification notification, object context)
    {
        var jumpMarker = notification as JumpMarker;
        if (jumpMarker == null) return;

        var destinationMarker = jumpMarker.destinationMarker;
        if (destinationMarker != null && destinationMarker.active)
        {
            if (Skip)
            {
                return;
            }
            var timelinePlayable = origin.GetGraph().GetRootPlayable(0);
            timelinePlayable.SetTime(destinationMarker.time);
        }
    }
}

