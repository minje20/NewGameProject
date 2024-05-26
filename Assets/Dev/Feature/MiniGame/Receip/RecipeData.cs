using System;
using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;

[CreateAssetMenu(menuName = "IndieLINY/MiniGame/RecipeData", fileName = "new recipe data")]
public class RecipeData : ScriptableObject
{
    #region Timeline

    [field: SerializeField,
            PropertyName("미니게임 시퀸스"),
            DefinedValues(
                "DummyTimeline",
                "None"
            )]
    private string _timelineKey;

    #endregion

    [field: SerializeField] private CocktailData _cocktail;

    [field: SerializeField, Multiline] private string _recipeText;

    [SerializeField] private MiniMeasurementInfo _Iceparameter;
    [SerializeField] private MiniMeasurementInfo _MeansurementParameter1;
    [SerializeField] private MiniMeasurementInfo _MeansurementParameter2;


    public string TimelineKey => _timelineKey;
    public CocktailData Cocktail => _cocktail;

    public string RecipeText => _recipeText;

    public MiniMeasurementInfo Iceparameter => _Iceparameter;

    public MiniMeasurementInfo MeansurementParameter1 => _MeansurementParameter1;

    public MiniMeasurementInfo MeansurementParameter2 => _MeansurementParameter2;

    // 해당 레시피를 선택했을 때 필요한 정보를 작성
    // 예: 완성된 칵테일, 획득 명성, 돈 등 
}

[Serializable]
public class MiniMeasurementInfo
{
    [SerializeField] private CountScoreBehaviour.Parameter _countScoreParam;
    [SerializeField] private LiquidScoreDisplayer.ValueParameter _liquidScoreParam;

    public CountScoreBehaviour.Parameter CountScoreParam => _countScoreParam;

    public LiquidScoreDisplayer.ValueParameter LiquidScoreParam => _liquidScoreParam;
}