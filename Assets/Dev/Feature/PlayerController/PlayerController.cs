using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private void Update()
    {
        Move();
    }

    private void Move()
    {
        Vector3 dir = new Vector3(
            Input.GetAxis("Horizontal"),
            0f,
            Input.GetAxis("Vertical")
        );
        
        
    }
}
