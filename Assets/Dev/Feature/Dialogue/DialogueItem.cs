using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class DialogueItem
{
    public string Text { get; private set; }
    public bool IsMaster { get; private set; }
    public string PoseKey { get; private set; }

    public DialogueItem(string text, bool isMaster, string poseKey="")
    {
        Text = text;
        IsMaster = isMaster;
        PoseKey = poseKey;
    }
}
