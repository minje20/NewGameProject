using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;

public class BindedMonobehaviour : MonoBehaviour
{
    [field: SerializeField, InitializationField]
    private string _key;
    [field: SerializeField, InitializationField]
    private Component _component;

    public string Key => _key;
    public Component Component => _component;
}
