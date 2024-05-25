using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "IndieLINY/MiniGame/CocktailData", fileName = "new cocktail data")]
public class CocktailData : ScriptableObject
{
    [SerializeField] private GameObject _prefab;

    public GameObject ClonePrefab()
    {
        return GameObject.Instantiate(_prefab);
    }
}
