using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[Serializable]
public struct MinMaxFloat
{
    public float min;
    public float max;

    public MinMaxFloat(float min, float max)
    {
        this.min = min;
        this.max = max;
    }
}

[Serializable]
public struct MinMaxInt
{
    public int min;
    public int max;

    public MinMaxInt(int min, int max)
    {
        this.min = min;
        this.max = max;
    }
}
