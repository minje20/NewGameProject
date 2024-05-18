using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CocktailRecipe : MonoBehaviour
{
    [SerializeField] private DrinkData _item1;
    [SerializeField] private DrinkData _item2;
    [SerializeField] private IngredientData _garnish;

    public DrinkData Item1 => _item1;

    public DrinkData Item2 => _item2;

    public IngredientData Garnish => _garnish;
    
    
}
