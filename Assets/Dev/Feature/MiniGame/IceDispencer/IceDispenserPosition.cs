using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceDispenserPosition : MonoBehaviour
{
    [SerializeField] private Transform _iceSpawnPoint;

    public Vector3 IceSpawnPoint => _iceSpawnPoint.position;
}
