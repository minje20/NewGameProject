using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "IndieLINY/MiniGame/RecipeTable", fileName = "new recipe table")]
public class RecipeTable : ScriptableObject
{
    [SerializeField] private List<RecipeData> _datas;


    public List<RecipeData> Datas => _datas;
}
