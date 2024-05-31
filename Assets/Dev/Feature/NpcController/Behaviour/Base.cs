using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseNpcBehaviour : MonoBehaviour
{
    public abstract bool PlayAnimation(string key);
    public abstract void Stop();
    protected abstract void Init(); 

    public NpcData Data { get; private set; }

    public static T Create<T>(GameObject parent, NpcData data) where T : BaseNpcBehaviour
    {
        var obj = new GameObject(typeof(T).Name);

        var com = obj.AddComponent<T>();
        obj.transform.SetParent(parent.transform);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.rotation = Quaternion.identity;
        obj.transform.localScale = Vector3.one;
        
        com.Data = data;
        com.Init();

        return com;
    }
}
