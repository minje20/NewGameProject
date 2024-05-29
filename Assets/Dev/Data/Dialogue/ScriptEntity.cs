using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ScriptEntity
{
    public int order;
    public string script_code;
    public int is_master;
    public string script;

    public int Order => order;

    public string ScriptCode => script_code;

    public bool IsMaster => is_master == 1;

    public string Script => script;

    public ScriptEntity Clone()
        => new ()
        {
            order = order,
            script_code = script_code,
            is_master = is_master,
            script = script,
        };
}
