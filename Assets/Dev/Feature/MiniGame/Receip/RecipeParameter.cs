using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;

public class RecipeParameter
{

    [field: SerializeField, MinValue(0)] private int _iceCount;
    [field: SerializeField, Range(0f, 1f)] private float _step1Measurement;
    [field: SerializeField, Range(0f, 1f)] private float _step2Measurement;
    [field: SerializeField, Range(0f, 1f)] private float _step3Measurement;
    [field: SerializeField, Range(0f, 1f)] private float _step4Measurement;

    public int IceCount => _iceCount;

    public float Step1Measurement => _step1Measurement;

    public float Step2Measurement => _step2Measurement;

    public float Step3Measurement => _step3Measurement;

    public float Step4Measurement => _step4Measurement;
}