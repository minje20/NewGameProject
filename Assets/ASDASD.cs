using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ASDASD : MonoBehaviour
{
    public GameObject Prefab;
    
    private void Update()
    {
        if (InputManager.Actions.ShakingMiniGameInteraction.IsPressed())
        {
            var obj = Instantiate(Prefab);
            obj.transform.position = transform.position;
        }
    }
}

